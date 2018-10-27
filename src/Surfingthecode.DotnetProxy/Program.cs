using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Surfingthecode.DotnetProxy
{
    public class Program
    {
        private const int DefaultPortNumber = 8080;

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption();
            app.Command("run", config =>
            {
                var portOption = config.Option<int>("-p|--port <PORT>", "Port number", CommandOptionType.SingleValue).Accepts(v => v.Range(0, ushort.MaxValue));

                config.OnExecute(() =>
                {
                    var port = portOption.HasValue() ? portOption.ParsedValue : DefaultPortNumber;
                    CreateWebHostBuilder(args, port).Build().Run();
                });
            });

            app.OnExecute(() =>
            {
                Console.WriteLine("You must specify a subcommand");
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args, int port) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls($"http://localhost:{port}")
                .UseStartup<Startup>();
    }
}
