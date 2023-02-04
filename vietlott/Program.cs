// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using vietlott;
using Vietlott.Services.Constants;
using Vietlott.Services.Settings;
using Vietlott.Services;

Console.WriteLine(CommonConst.Message_Name_Application_Start);
Console.WriteLine(CommonConst.Message_Program_Start);
IServiceCollection services = new ServiceCollection();
Startup startup = new Startup();
startup.ConfigureServices(services);
IServiceProvider serviceProvider = services.BuildServiceProvider();
serviceProvider.GetService<KendoServices>()!.Run();
serviceProvider.GetService<KendoServices>()!.SetInterval(action: () => serviceProvider.GetService<KendoServices>()!.Run(), TimeSpan.FromMinutes(2));
Thread.Sleep(TimeSpan.FromDays(1));
Console.WriteLine(CommonConst.Message_Program_Finished);
Console.WriteLine(CommonConst.Message_Name_Application_End);
Console.WriteLine(CommonConst.Message_Name_Application_Copyright);
