using Microsoft.Extensions.DependencyInjection;
using Vietlott.Services.Constants;
using Vietlott.Services;

// Console: Application information
Console.WriteLine(CommonConst.Message_Name_Application_Start);
Console.WriteLine(CommonConst.Message_Program_Start);

// Build service collections
IServiceCollection services = new ServiceCollection();
Startup startup = new Startup();
startup.ConfigureServices(services);
IServiceProvider serviceProvider = services.BuildServiceProvider();

// Main function 
var predictService = serviceProvider.GetService<PredictService>();
predictService!.PredictRandomByDate();

// Console: Application end information
Console.WriteLine(CommonConst.Message_Program_Finished);
Console.WriteLine(CommonConst.Message_Name_Application_End);
Console.WriteLine(CommonConst.Message_Name_Application_Copyright);
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();