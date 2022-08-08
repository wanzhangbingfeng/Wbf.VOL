using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.Extensions.AutofacManager;
using Wbf.VOL.Core.ObjectActionValidator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IObjectModelValidator>(new NullObjectModelValidator());
//“¿¿µ◊¢»Î
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(container =>
{
    builder.Services.AddModule(container, builder.Configuration);
});

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(option => {
        option.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//ÃÌº”øÁ”Ú«Î«Û¥¶¿Ì≈‰÷√
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin()
               .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                .AllowAnyHeader().AllowAnyMethod();
            });
});

//∆Ù”√ª∫¥Ê
builder.Services.AddMemoryCache();


var app = builder.Build();

//≈‰÷√HttpContext
app.UseStaticHttpContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//∆Ù”√øÁ”Ú«Î«Û
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
