using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

namespace NetGenCLI
{
    partial class Program
    {
        public class Application
        {
            private readonly Serilog.ILogger logger;
            private readonly IConfiguration config;
            private readonly MirthWebClient webClient;

            public Application(Serilog.ILogger logger, IConfiguration config, MirthWebClient webClient)
            {
                this.logger = logger;
                this.config = config;
                this.webClient = webClient;
            }

            public void Execute()
            {
                logger.Debug("Can you see me?");
            } 
        }
    }
}
