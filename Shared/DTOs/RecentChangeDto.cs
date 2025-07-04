using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
 
    public class RecentChangeDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("bot")]
        public bool Bot { get; set; }

        [JsonPropertyName("meta")]
        public MetaDto Meta { get; set; } = new();
    }
}
