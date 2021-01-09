using BLL.EntityViewModel;
using BLL.UnityOfWork;
using Client.UI.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Client.ViewModel
{
    public class SettingsWindowViewModel : INotifyPropertyChanged
    {
        private UnityOfWorkService unityOfWorkWithModel;
        private SettingsWindow settingsWindow;
        private DelegateCommand addphoto_cm;
        private DelegateCommand save_cm;
        private ClientViewModel client;
        private string photopath;
        public string PhotoPathUI
        {
            get => photopath;
            set
            {
                photopath = value;
                OnPropertyChanged();
            }
        }
        public ICommand Addphoto_cm => addphoto_cm;
        public ICommand Save_cm => save_cm;

        public event PropertyChangedEventHandler PropertyChanged;
        public SettingsWindowViewModel(SettingsWindow _settingsWindow, ClientViewModel _client)
        {
            client = _client;
            settingsWindow = _settingsWindow;
            addphoto_cm = new DelegateCommand(AddPhoto);
            save_cm = new DelegateCommand(Save);
            unityOfWorkWithModel = new UnityOfWorkService();
        }
        private void AddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                client.ImagePath = openFileDialog.FileName;
                PhotoPathUI = client.ImagePath;
            }
        }
        private void Save()
        {
            if (client.ImagePath != string.Empty || client.ImagePath != null)
            {
                foreach (var c in unityOfWorkWithModel.GetClients())
                {
                    if (c.Id == client.Id)
                    {
                        using (var ms = new MemoryStream())
                        {
                            Image image = Image.FromFile(client.ImagePath);
                            image.Save(ms, image.RawFormat);
                            byte[] imegeData = ms.ToArray();
                            client.Image = new byte[imegeData.Length];
                            for (int i = 0; i < imegeData.Length; i++)
                            {
                                client.Image[i] = imegeData[i];
                            }
                        }
                    }
                }
                unityOfWorkWithModel.UpdateClientImage(client);
            }
            settingsWindow.Close();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
