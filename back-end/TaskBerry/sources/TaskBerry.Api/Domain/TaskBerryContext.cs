namespace TaskBerry.Api.Domain
{
    using System.Net.Http;
    using System;


    public class TaskBerryContext : IDisposable
    {
        public TaskBerryContext(string baseUrl)
        {
            this.Client = new HttpClient();
            this.Client.BaseAddress = new Uri(baseUrl);
        }

        public void Dispose()
        {
            this.Client?.Dispose();
        }

        internal HttpClient Client { get; set; }
    }
}