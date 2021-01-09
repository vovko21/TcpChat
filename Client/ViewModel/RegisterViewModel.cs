using BLL.EntityViewModel;
using BLL.UnityOfWork;
using Client.HashCode;
using Client.UI.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DelegateCommand register_cm;
        private UnityOfWorkService unityOfWorkWithModel;
        private RegisterWindow registerWindow;
        private string clientLoginUI;
        public string ClientLoginUI
        {
            get { return clientLoginUI; }
            set
            {
                if (clientLoginUI != value)
                {
                    clientLoginUI = value;
                    OnPropertyChanged();
                }
            }
        }
        private string clientPasswordUI;
        public string ClientPasswordUI
        {
            get { return clientPasswordUI; }
            set
            {
                if (clientPasswordUI != value)
                {
                    clientPasswordUI = value;
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
        public ICommand Register_cm => register_cm;
        public RegisterViewModel(RegisterWindow rw)
        {
            registerWindow = rw;
            unityOfWorkWithModel = new UnityOfWorkService();
            register_cm = new DelegateCommand(Register);
        }
        private void Register()
        {
            var client = new ClientViewModel();
            client.Name = ClientNameUI;
            client.Login = ClientNameUI;
            client.Password = EncryptingPassword.CreateMD5HashCode(ClientPasswordUI);

            foreach (var c in unityOfWorkWithModel.GetClients())
            {
                if (client.Login == c.Login)
                {
                    MessageBox.Show("This client already registed");
                    return;
                }
            }
            unityOfWorkWithModel.AddClient(client);

            MainWindow mainWindow = new MainWindow(client);
            mainWindow.Show();
            registerWindow.Close();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
