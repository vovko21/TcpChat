using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.Model
{
    [Serializable]
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<Client> FriendList { get; set; }
        public virtual ICollection<Group> GroupList { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
