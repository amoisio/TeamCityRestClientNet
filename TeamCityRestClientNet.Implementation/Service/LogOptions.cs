namespace TeamCityRestClientNet.Service
{
    public class LogOptions
    {
        public LogOptions()
        {   
            LogRequest = true;
        }
        
        public bool LogRequest { get; set; }
        public bool LogRequestHeaders { get; set; }
        public bool LogRequestContent { get; set; }
        public bool LogResponse { get; set; }
        public bool LogResponseHeaders { get; set; }    
        public bool LogResponseContent { get; set; }
    }
}