using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Dapper;
using Wbf.VOL.Core.EFDbContext;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Entity.SystemModels;

namespace Wbf.VOL.Core.BaseProvider
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// EF DBContext
        /// </summary>
        VOLContext DbContext { get; }

        ISqlDapper DapperContext { get; }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <param name="orderBy">排序字段,数据格式如:
        ///  orderBy = x => new Dictionary<object, bool>() {
        ///          { x.BalconyName,QueryOrderBy.Asc},
        ///          { x.TranCorpCode1,QueryOrderBy.Desc}
        ///         };
        /// </param>
        /// <returns></returns>
        IQueryable<TEntity> FindAsIQueryable(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, Dictionary<object, QueryOrderBy>>> orderBy = null);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties">指定更新字段:x=>new {x.Name,x.Enable}</param>
        /// <param name="saveChanges">是否保存</param>
        /// <returns></returns>

        int Update(TEntity entity, Expression<Func<TEntity, object>> properties, bool saveChanges = false);
    }
}
