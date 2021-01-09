using Client.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Client.HashCode;
using BLL.UnityOfWork;
using BLL.EntityViewModel;

namespace Client.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private LoginWindow loginWindow;
        private DelegateCommand login_cm;
        private DelegateCommand register_cm;
        private UnityOfWorkService unityOfWorkWithModel;
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
        public ICommand Login_cm => login_cm;
        public ICommand Register_cm => register_cm;

        public LoginViewModel(LoginWindow _loginWindow)
        {
            login_cm = new DelegateCommand(CheckLogin);
            register_cm = new DelegateCommand(OpenRegisterWindow);
            loginWindow = _loginWindow;
            unityOfWorkWithModel = new UnityOfWorkService();
        }
        private void OpenRegisterWindow()
        {
            RegisterWindow regWindow = new RegisterWindow();
            regWindow.Show();
            loginWindow.Close();
        }
        private void CheckLogin()
        {
            IEnumerable<ClientViewModel> clients = unityOfWorkWithModel.GetClients();
            bool isCorrect = false;
            foreach (var c in clients)
            {
                if (c.Login == ClientLoginUI && c.Password == EncryptingPassword.CreateMD5HashCode(ClientPasswordUI))
                {
                    OpenMessenger(c);
                    isCorrect = true;
                }
            }
            if (isCorrect == false)
            {
                MessageBox.Show("Login or password uncorrect");
            }
        }
        private void OpenMessenger(ClientViewModel client)
        {
            MainWindow mainWindow = new MainWindow(client);
            mainWindow.Show();
            loginWindow.Close();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
