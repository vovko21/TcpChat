using Server.Model;
using Server.Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UnityOfWork
{
    public class UnityOfWorkWithModel
    {
        private ServerDataContext serverDataContext;
        public UnityOfWorkWithModel()
        {
            serverDataContext = new ServerDataContext();
        }
        public Client GetClientById(int id)
        {
            foreach (var c in serverDataContext.Clients)
            {
                if (c.Id == id)
                {
                    return c;
                }
            }
            return null;
        }
        public void AddClient(Client client)
        {
            serverDataContext.Clients.Add(client);
            Save();
        }
        public void AddGroup(Group group)
        {
            serverDataContext.Groups.Add(group);
            Save();
        }
        public void UpdateClient(Client client)
        {
            serverDataContext.Clients.AddOrUpdate(client);
            Save();
        }
        public IEnumerable<Client> GetClients()
        {
            return serverDataContext.Clients;
        }
        public IEnumerable<Group> GetGroups()
        {
            return serverDataContext.Groups;
        }
        public void Save()
        {
            serverDataContext.SaveChanges();
        }
    }
}
