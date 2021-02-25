using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Configuration;

namespace Turbo.Main.Configuration
{
    public class TurboConfig : IEmulatorConfig
    {
        public const string Turbo = "Turbo";

        public string GameHost { get; set; }

        public bool GameTCPEnabled { get; set; }

        public int GameTCPPort { get; set; }

        public bool GameWSEnabled { get; set; }

        public int GameWSPort { get; set; }

        public List<string> GameWSWhitelist { get; set; }

        public string RCONHost { get; set; }

        public int RCONPort { get; set; }

        public List<string> RCONWhitelist { get; set; }

        public string DatabaseHost { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }

        // MariaDB or MySQL (example: 10.5.9)
        public string MySqlVersion { get; set; }

        public int NetworkWorkerThreads { get; set; }
    }
}
