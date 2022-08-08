using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Dapper;
using Wbf.VOL.Core.DBManager;
using Wbf.VOL.Core.EFDbContext;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Entity.SystemModels;

namespace Wbf.VOL.Core.BaseProvider
{
    public class RepositoryBase<TEntity> where TEntity : BaseEntity
    {
        public RepositoryBase()
        {
        }
        public RepositoryBase(VOLContext dbContext)
        {
            this.DefaultDbContext = dbContext ?? throw new Exception("dbContext未实例化。");
        }

        #region 属性
        private VOLContext DefaultDbContext { get; set; }
        private VOLContext EFContext
        {
            get
            {
                DBServerProvider.GetDbContextConnection<TEntity>(DefaultDbContext);
                return DefaultDbContext;
            }
        }

        public virtual VOLContext DbContext
        {
            get { return DefaultDbContext; }
        }

        public ISqlDapper DapperContext
        {
            get { return DBServerProvider.GetSqlDapper<TEntity>(); }
        }
        #endregion

        /// <summary>
        /// 更新表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveChanges">是否保存</param>
        /// <param name="properties">格式 Expression<Func<entityt, object>> expTree = x => new { x.字段1, x.字段2 };</param>
        public virtual int Update(TEntity entity, Expression<Func<TEntity, object>> properties, bool saveChanges = false)
        {
            return Update<TEntity>(entity, properties, saveChanges);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序规则</param>
        /// <returns></returns>
        public IQueryable<TEntity> FindAsIQueryable(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy = null)
        {
            if (orderBy != null)
                return DbContext.Set<TEntity>().Where(predicate).GetIQueryableOrderBy(orderBy.GetExpressionToDic());
            return DbContext.Set<TEntity>().Where(predicate);
        }

        public virtual IQueryable<TFind> FindAsIQueryable<TFind>(Expression<Func<TFind, bool>> predicate) where TFind : class
        {
            return EFContext.Set<TFind>().Where(predicate);
        }

        public virtual int Update<TSource>(TSource entity, Expression<Func<TSource, object>> properties, bool saveChanges = false) where TSource : class
        {
            return UpdateRange(new List<TSource>
            {
                entity
            }, properties, saveChanges);
        }

        public virtual int UpdateRange<TSource>(IEnumerable<TSource> entities, Expression<Func<TSource, object>> properties, bool saveChanges = false) where TSource : class
        {
            return UpdateRange<TSource>(entities, properties?.GetExpressionProperty(), saveChanges);
        }

        /// <summary>
        /// 更新表数据
        /// </summary>
        /// <param name="models"></param>
        /// <param name="properties">格式 Expression<Func<entityt, object>> expTree = x => new { x.字段1, x.字段2 };</param>
        public int UpdateRange<TSource>(IEnumerable<TSource> entities, string[] properties, bool saveChanges = false) where TSource : class
        {
            if (properties != null && properties.Length > 0)
            {
                PropertyInfo[] entityProperty = typeof(TSource).GetProperties();
                string keyName = entityProperty.GetKeyName();
                if (properties.Contains(keyName))
                {
                    properties = properties.Where(x => x != keyName).ToArray();
                }
                properties = properties.Where(x => entityProperty.Select(s => s.Name).Contains(x)).ToArray();
            }
            foreach (TSource item in entities)
            {
                if (properties == null || properties.Length == 0)
                {
                    DbContext.Entry<TSource>(item).State = EntityState.Modified;
                    continue;
                }
                var entry = DbContext.Entry(item);
                properties.ToList().ForEach(x =>
                {
                    entry.Property(x).IsModified = true;
                });
            }
            if (!saveChanges) return 0;

            //2020.04.24增加更新时并行重试处理
            try
            {
                // Attempt to save changes to the database
                return DbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                int affectedRows = 0;
                foreach (var entry in ex.Entries)
                {
                    var proposedValues = entry.CurrentValues;

                    var databaseValues = entry.GetDatabaseValues();
                    //databaseValues == null说明数据已被删除
                    if (databaseValues != null)
                    {
                        foreach (var property in properties == null
                            || properties.Length == 0 ? proposedValues.Properties
                            : proposedValues.Properties.Where(x => properties.Contains(x.Name)))
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                        }
                        affectedRows++;
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
                if (affectedRows == 0) return 0;

                return DbContext.SaveChanges();
            }
        }
    }
}
