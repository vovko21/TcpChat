using Client.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BLL.EntityViewModel;
using BLL.UnityOfWork;
using BLL;
using Server.Network;

namespace Client.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        //Server fields
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient tcpClient;
        static NetworkStream stream;
        private ServerHelper serverHelper;
        //ViewModel fields
        public ObservableCollection<ClientViewModel> friendList = new ObservableCollection<ClientViewModel>();
        public ObservableCollection<ClientViewModel> allClients = new ObservableCollection<ClientViewModel>();
        public ObservableCollection<string> chatMessages = new ObservableCollection<string>();
        public event PropertyChangedEventHandler PropertyChanged;
        private UnityOfWorkService unityOfWorkWithModel;
        private DelegateCommand search_cm;
        private DelegateCommand sendMessage_cm;
        private DelegateCommand showSettings_cm;
        private ClientViewModel currentClient;
        public ClientViewModel CurrentClient
        {
            get { return currentClient; }
            set
            {
                if (currentClient != value)
                {
                    currentClient = value;
                    OnPropertyChanged();
                }
            }
        }
        private ClientViewModel selectedClient;
        public ClientViewModel SelectedClient
        {
            get { return selectedClient; }
            set
            {
                if (selectedClient != value)
                {
                    selectedClient = value;
                    OnPropertyChanged();
                }
            }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }
        }
        private string clientImageUI;
        public string ClientImageUI
        {
            get { return clientImageUI; }
            set
            {
                if (clientImageUI != value)
                {
                    clientImageUI = value;
                    OnPropertyChanged();
                }
            }
        }
        private string clientNameUI;
        public string ClientNameUI
        {
            get { return clientNameUI; }
            set
            {
                if (clientNameUI != value)
                {
                    clientNameUI = value;
                    OnPropertyChanged();
                }
            }
        }
        private string globalSearch_cm;
        public string GlobalSearch_cm
        {
            get { return globalSearch_cm; }
            set
            {
                if (globalSearch_cm != value)
                {
                    globalSearch_cm = value;
                    OnPropertyChanged();
                }
            }
        }
        private Thread receivetask;
        public ICommand Search_cm => search_cm;
        public ICommand SendMessage_cm => sendMessage_cm;
        public ICommand ShowSettings_cm => showSettings_cm;
        public IEnumerable<ClientViewModel> FriendList => friendList;
        public IEnumerable<string> ChatMessages => chatMessages;
        private ChatViewModel currentChat;
        private bool s_started = false;
        //ctor
        public ViewModel(ClientViewModel client)
        {
            currentClient = client;
            CurrentClient.FriendList = new ObservableCollection<ClientViewModel>();
            unityOfWorkWithModel = new UnityOfWorkService();
            serverHelper = new ServerHelper();

            LoadClientData();

            search_cm = new DelegateCommand(Search);
            sendMessage_cm = new DelegateCommand(SendObject);
            showSettings_cm = new DelegateCommand(ShowSettings);
        }
        private void LoadClientData()
        {
            ClientNameUI = currentClient.Name;
            if (currentClient.FriendList != null && currentClient.FriendList.Count > 0)
            {
                foreach (var f in currentClient.FriendList)
                {
                    friendList.Add(f);
                }
            }
            foreach (var c in unityOfWorkWithModel.GetClients())
            {
                if (c.Image != null && c.Image.Length > 0)
                {
                    allClients.Add(LoadIconOfClient(c));
                }
                else
                {
                    allClients.Add(c);
                }
            } 
        }
        public ClientViewModel LoadIconOfClient(ClientViewModel client)
        {
            using (var ms = new MemoryStream(client.Image))
            {
                Image clientImage = Image.FromStream(ms);
                ImageFormat format = new ImageFormat(clientImage.RawFormat.Guid);
                var extension = ImageCodecInfo.GetImageEncoders()
                    .First(x => x.FormatID == format.Guid)
                    .FilenameExtension
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Trim('*')
                    .ToLower();
                string path = $"icon {DateTime.Now.Ticks}" + extension;
                clientImage.Save(path);
                client.ImagePath = Environment.CurrentDirectory + "\\" + path;
                if (client.Id == currentClient.Id)
                {
                    ClientImageUI = client.ImagePath;
                }
                return client;
            }
        }
        //UI methods
        private void ShowSettings()
        {
            var settings = new SettingsWindow(currentClient);
            settings.ShowDialog();
        }
        private void Search()
        {
            foreach (var c in allClients)
            {
                if (c.Name == GlobalSearch_cm && c.Name != currentClient.Name)
                {
                    bool isCorrect = true;
                    foreach (var flc in friendList)
                    {
                        if (flc.Login == c.Login)
                        {
                            isCorrect = false;
                        }
                    }
                    if (isCorrect == true)
                    {
                        friendList.Add(c);
                        CurrentClient.FriendList.Add(c);
                    }
                }
            }
        }
        public void SelectedChanged()
        {
            chatMessages.Clear();
            //load chats
            foreach (var c in unityOfWorkWithModel.GetClients())
            {
                if (c.Login == currentClient.Login)
                {
                    if (c.Chats.Count > 0)
                    {
                        foreach (var ct in c.Chats)
                        {
                            if ((currentClient.Login == ct.ThisClientLogin && SelectedClient.Login == ct.WithClientLogin) && ct.Messages != null)
                            {
                                string[] messagesSplitted = ct.Messages.Split('\n');
                                messagesSplitted = messagesSplitted.Where(val => val != string.Empty).ToArray();
                                foreach (var ms in messagesSplitted)
                                {
                                    chatMessages.Add(ms);
                                }
                            }
                        }
                    }
                }
            }
            //add chat
            bool find = false;
            foreach (var c in currentClient.Chats)
            {
                if (c.WithClientLogin == selectedClient.Login)
                {
                    find = true;
                    currentChat = c;
                }
            }
            if (find == false)
            {
                currentChat = new ChatViewModel();
                currentChat.WithClientLogin = selectedClient.Login;
                currentChat.ThisClientLogin = currentClient.Login;
                currentChat.Messages = string.Empty;
                currentClient.Chats.Add(currentChat);
            }
            //start chat
            if (s_started == false)
            {
                StartChat();
                s_started = true;
            }
        }
        public void SaveChatsToDTO()
        {
            unityOfWorkWithModel.UpdateClientChat(currentClient);
        }
        public void SaveFriendListToDTO()
        {
            unityOfWorkWithModel.UpdateClientFriendList(currentClient);
        }
        //private void UpdateChatLocal(string fromClient, string withClient, ChatViewModel chat)
        //{
        //    for (int i = 0; i < currentClient.Chats.Count; i++)
        //    {
        //        if (currentClient.Chats[i].WithClientLogin == withClient && currentClient.Chats[i].ThisClientLogin == fromClient)
        //        {
        //            currentClient.Chats[i] = chat;
        //        }
        //    }
        //}
        //Work with server methods
        private void StartChat()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(host, port);

                stream = tcpClient.GetStream();

                //Send Id of current clint to server
                SendString(currentClient.Id.ToString());

                receivetask = new Thread(new ThreadStart(RecieveObject));
                receivetask.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                serverHelper.RemoveConnection(currentClient.Id);
            }
        }
        private void RecieveObject()
        {
            while (true)
            {
                try
                {
                    if (stream.DataAvailable)
                    {
                        byte[] data = new byte[2048];
                        int bytes = 0;
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                        }
                        while (stream.DataAvailable);
                        TransitionObject packge = (TransitionObject)serverHelper.ByteArrayToObject(data);
                        if (packge.ToClientId == currentClient.Id || packge.FromClientId == currentClient.Id)
                        {
                            var fromClient = unityOfWorkWithModel.GetClientById(packge.FromClientId);
                            var toClient = unityOfWorkWithModel.GetClientById(packge.ToClientId);
                            if (selectedClient.Id == fromClient.Id || selectedClient.Id == toClient.Id)
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    chatMessages.Add($"{fromClient.Name}: {packge.Message}");
                                });
                            }
                            foreach (var ct in currentClient.Chats)
                            {
                                if (ct.WithClientLogin == fromClient.Login || ct.WithClientLogin == toClient.Login)
                                {
                                    ct.Messages += $"{fromClient.Name}: {packge.Message}\n";
                                }
                            }
                            if (currentChat != null && currentChat.Messages.Length > 0)
                            {
                                SaveChatsToDTO();
                            }
                        }
                        else
                        {
                            SendPackage(packge);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }
        private void SendObject()
        {
            serverHelper.SendCommand(Server.Network.Command.MessageSend, stream);
            Thread.Sleep(1);
            var packge = new TransitionObject() { Message = this.Message, FromClientId = currentClient.Id, ToClientId = selectedClient.Id };
            byte[] data = serverHelper.ObjectToByteArray(packge);
            stream.Write(data, 0, data.Length);
        }
        private void SendPackage(TransitionObject _packge)
        {
            serverHelper.SendCommand(Server.Network.Command.MessageSend, stream);
            Thread.Sleep(1);
            byte[] data = serverHelper.ObjectToByteArray(_packge);
            stream.Write(data, 0, data.Length);
        }
        private void SendString(string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            stream.Write(data, 0, data.Length);
        }
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (tcpClient != null)
                tcpClient.Close();
            serverHelper.RemoveConnection(currentClient.Id);
            //Environment.Exit(0);
        }
        //Event
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
