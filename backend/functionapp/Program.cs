using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

// var host = Host.CreateDefaultBuilder(args)
// 	.ConfigureFunctionsWorkerDefaults(builder => {
// 		// builder.Services.Configure<FunctionExecutionContextOptions>(
// 		// 	options => options.EnableInputAndOutputBinding = true);
// 	})
// 	.Build();

// host.Run();


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
