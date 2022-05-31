
using System;
using System.Text.Json.Serialization;

using JetBrains.Annotations;
using Crowdin.Xamarin.Forms.Infrastructure;

namespace Crowdin.Xamarin.Forms.Models
{
    [PublicAPI]
    public class DistributionManifest
    {
        [JsonPropertyName("files")]
        public string[] Files { get; set; }
        
        [JsonPropertyName("languages")]
        public string[] Languages { get; set; }
        
        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset Timestamp { get; set; }
    }
}