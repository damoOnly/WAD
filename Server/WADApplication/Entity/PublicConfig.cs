using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class PublicConfig
    {
        public PublicConfig()
        {
            portList = new List<string>();
            systemConfig = new StructSystemConfig();
            commonConfig = new CommonConfig();

        }
        public List<string> portList { get; set; }
        public StructSystemConfig systemConfig { get; set; }
        public CommonConfig commonConfig { get; set; }
    }
}
