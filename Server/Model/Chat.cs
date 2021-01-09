using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Model
{
    [Serializable]
    public class Chat
    {
        public int Id { get; set; }
        public string WithClientLogin { get; set; }
        public string ThisClientLogin { get; set; }
        public virtual string Messages { get; set; }
    }
}
