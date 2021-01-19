using System.Collections.Generic;

namespace CLImber.Configuration
{
    public class ClimberConfig
    {
        public string Shell { get; set; }
        public ICollection<EndpointConfig> Endpoints { get; set; }
    }
}