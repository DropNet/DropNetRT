using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DropNetRT.Sample.WP8.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        private bool _showBrowser;
        public bool ShowBrowser
        {
            get { return _showBrowser; }
            set
            {
                _showBrowser = value;
                NotifyPropertyChanged("ShowBrowser");
                NotifyPropertyChanged("ShowButton");
            }
        }
        public bool ShowButton
        {
            get { return !_showBrowser; }
        }

        public LoginViewModel(ProgressIndicator prog, Dispatcher dispatcher)
            : base(prog, dispatcher)
        {
        }

    }
}
