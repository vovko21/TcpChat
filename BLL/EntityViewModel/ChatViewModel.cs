using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }
        private string withClientLogin;
        public string WithClientLogin
        {
            get { return withClientLogin; }
            set
            {
                if (withClientLogin != value)
                {
                    withClientLogin = value;
                    OnPropertyChanged();
                }
            }
        }
        private string thisClientLogin;
        public string ThisClientLogin
        {
            get { return thisClientLogin; }
            set
            {
                if (thisClientLogin != value)
                {
                    thisClientLogin = value;
                    OnPropertyChanged();
                }
            }
        }
        private string messages;
        public string Messages
        {
            get { return messages; }
            set
            {
                if (messages != value)
                {
                    messages = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
