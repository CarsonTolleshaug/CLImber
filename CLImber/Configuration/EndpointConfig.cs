using System.Collections.Generic;

namespace CLImber.Configuration
{
    public class EndpointConfig
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public string Method { get; set; }
        public string Command { get; set; }
        public ICollection<ResponseConfig> Responses { get; set; }
    }
}