using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class RecentChangeDto
    {
        public string type { get; set; }
        public string title { get; set; }
        public string user { get; set; }
        public bool bot { get; set; }
        public MetaDto meta { get; set; }
    }
}
