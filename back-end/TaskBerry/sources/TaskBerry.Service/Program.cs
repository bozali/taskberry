namespace TaskBerry.Service
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore;


    public class Program
    {
        public static void Main(string[] args)
        {
            Program
                .CreateWebHostBuilder(args)
#if !DEBUG
                .UseKestrel()
#endif
                .UseStartup<Startup>()
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
