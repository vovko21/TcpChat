using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Network
{
    [Serializable]
    public class TransitionObject
    {
        public virtual int FromClientId { get; set; }
        public virtual int ToClientId { get; set; }
        public virtual string Message { get; set; }
    }
}
