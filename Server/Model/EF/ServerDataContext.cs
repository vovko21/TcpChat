using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Server.Model.EF
{

    public class ServerDataContext : DbContext
    {
        public ServerDataContext()
            : base("name=ServerDataContext")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
    }
}