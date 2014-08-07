namespace DropNetRTWP
{
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

    public class LoginViewModel : BaseViewModel
    {

        private bool showBrowser;
        public bool ShowBrowser
        {
            get { return showBrowser; }
            set
            {
                showBrowser = value;
                NotifyPropertyChanged("ShowBrowser");
                NotifyPropertyChanged("ShowButton");
            }
        }
        public bool ShowButton
        {
            get { return !showBrowser; }
        }

        public LoginViewModel(ProgressBar prog, CoreDispatcher dispatcher)
            : base(prog, dispatcher)
        {
        }

    }
}
