using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable 8618

namespace SecureWebApi.Infrastructure
{
    public class AppSettings
    {
        /// <summary>
        /// Secret key used to sign JWT tokens
        /// </summary>
        [Required]
        public string Secret { get; set; }
    }
}