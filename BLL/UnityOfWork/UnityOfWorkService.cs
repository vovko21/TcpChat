using AutoMapper;
using BLL.EntityViewModel;
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
    public class UnityOfWorkService
    {
        private ServerDataContext serverDataContext;
        private Mapper mapper;
        private Mapper chatMapper;
        public UnityOfWorkService()
        {
            serverDataContext = new ServerDataContext();

            IConfigurationProvider chatConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ChatViewModel, Chat>()
                        .ForMember(d => d.Messages, map => map.MapFrom(s => s.Messages));
                    cfg.CreateMap<Chat, ChatViewModel>()
                        .ForMember(d => d.Messages, map => map.MapFrom(s => s.Messages));
                });
            chatConfig.AssertConfigurationIsValid();
            chatMapper = new Mapper(chatConfig);

            IConfigurationProvider config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ClientViewModel, Client>()
                        .ForMember(d => d.GroupList, map => map.Ignore())
                        .ForMember(m => m.Chats, map => map.MapFrom(s => chatMapper.Map<List<Chat>>(s.Chats)))
                        .ForMember(m => m.FriendList, map => map.MapFrom(s => s.FriendList));
                    cfg.CreateMap<Client, ClientViewModel>()
                        .ForMember(d => d.ImagePath, map => map.Ignore())
                        .ForMember(m => m.Chats, map => map.MapFrom(s => chatMapper.Map<List<ChatViewModel>>(s.Chats)))
                        .ForMember(m => m.FriendList, map => map.MapFrom(s => s.FriendList));
                });
            config.AssertConfigurationIsValid();
            mapper = new Mapper(config);
        }
        public ClientViewModel GetClientById(int id)
        {
            foreach (var c in serverDataContext.Clients)
            {
                if (c.Id == id)
                {
                    return mapper.Map<ClientViewModel>(c);
                }
            }
            return null;
        }
        public void AddClient(ClientViewModel client)
        {
            serverDataContext.Clients.Add(mapper.Map<Client>(client));
            Save();
        }
        public void UpdateClient(ClientViewModel client)
        {
            serverDataContext.Clients.AddOrUpdate(mapper.Map<Client>(client));
            Save();
        }
        public void UpdateClientChat(ClientViewModel client)
        {
            Client sourceClient = mapper.Map<Client>(client);
            foreach (var c in serverDataContext.Clients)
            {
                if (c.Id == sourceClient.Id)
                {
                    c.Chats = sourceClient.Chats;
                }
            }
            Save();
        }
        public void UpdateClientImage(ClientViewModel client)
        {
            Client sourceClient = mapper.Map<Client>(client);
            foreach (var c in serverDataContext.Clients)
            {
                if (c.Id == sourceClient.Id)
                {
                    c.Image = sourceClient.Image;
                }
            }
            Save();
        }
        public void UpdateClientFriendList(ClientViewModel client)
        {
            Client sourceClient = mapper.Map<Client>(client);
            foreach (var c in serverDataContext.Clients)
            {
                if (c.Id == sourceClient.Id)
                {
                    c.FriendList = sourceClient.FriendList;
                }
            }
            Save();
        }
        public List<ClientViewModel> GetClients()
        {
            return mapper.Map<List<ClientViewModel>>(serverDataContext.Clients);
        }
        public void Save()
        {
            serverDataContext.SaveChanges();
        }
    }
}
