namespace TaskBerry.Service.Configuration
{
    using Microsoft.Extensions.Configuration;


    /// <summary>
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        /// <inheritdoc cref="IConfigurationProvider"/>
        public ConfigurationProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <inheritdoc cref="IConfigurationProvider"/>
        public TokenConfiguration GetTokenConfiguration()
        {
            return this._configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
        }
    }
}