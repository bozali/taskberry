namespace TaskBerry.Service.Configuration
{
    using Newtonsoft.Json;


    /// <summary>
    /// </summary>
    [JsonObject]
    public class TokenConfiguration
    {
        /// <summary>
        /// </summary>
        [JsonProperty]
        public string Secret { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty]
        public string Issuer { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty]
        public string Audience { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty]
        public int AccessExpiration { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty]
        public int RefreshExpiration { get; set; }
    }
}