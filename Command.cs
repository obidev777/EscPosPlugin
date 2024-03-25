using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscPosPlugin
{

    [Serializable]
    public class Command
    {
        public string Name { get; set; }
        public List<dynamic> Args { get; set; }
    }
}
