using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    class ApplicationProperties
    {
        public int id;
        public string jsonrpc;
        public ApplicationPropertiesResult result;
    }

    class ApplicationPropertiesResult
    {
        public int volume;
    }
}
