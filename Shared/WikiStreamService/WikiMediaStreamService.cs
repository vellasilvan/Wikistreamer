using Shared.DTOs;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.WikiStreamService
{
    public class WikiMediaStreamService : IWikiStreamService
    {
        private readonly HttpClient _httpClient;

        public WikiMediaStreamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetWikiStreamsAsync()
        {

            var stream = await _httpClient.GetStreamAsync("/v2/stream/recentchange");
            using var reader = new System.IO.StreamReader(stream);

            string? eventType = null;
            string? idLine = null;
            string? dataLine = null;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    // End of event block — process the event
                    if (!string.IsNullOrEmpty(dataLine))
                    {
                        try
                        {
                            var change = JsonSerializer.Deserialize<RecentChangeDto>(dataLine);
                            Console.WriteLine($"[{change.type}] {change.title} by {change.user}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error parsing event: " + ex.Message);
                        }
                    }

                    eventType = idLine = dataLine = null;
                    continue;
                }

                if (line.StartsWith("event:"))
                    eventType = line.Substring(6).Trim();

                if (line.StartsWith("id:"))
                    idLine = line.Substring(3).Trim();

                if (line.StartsWith("data:"))
                    dataLine = line.Substring(5).Trim();
            }

            return "";
        }
    }
}
