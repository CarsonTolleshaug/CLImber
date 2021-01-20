namespace CLImber.Configuration
{
    public class ResponseConfig
    {
        public ConditionConfig Condition { get; set; }
        public int ResponseCode { get; set; }
        public object ResponseBody { get; set; }
    }
}