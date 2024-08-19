//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using WebApp.Localization;

try
{
    var builder = WebApplication.CreateBuilder(args);


    //设置 AppModule 为启动模块
    await builder.AddApplicationAsync<AppModule>();
    var app = builder.Build();
    //调用 ABP 提供的初始化应用程序方法
    //await app.InitializeApplicationAsync();
    app.MapControllers();
    await app.RunAsync();
}
catch (Exception e)
{

}
