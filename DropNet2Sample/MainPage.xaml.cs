using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DropNet2Sample.Resources;
using DropNet2Sample.Extensions;
using DropNet2Sample.ViewModels;

namespace DropNet2Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        //TODO - Your callback url here
        private const string _tokenCallbackUrl = "http://dkdevelopment.net/";

        private LoginViewModel _model;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            loginBrowser.LoadCompleted += loginBrowser_LoadCompleted;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Show the splash screen
            MessageBox.Show("Welcome to the DropNet Sample App. Something about it being open source...");

            _model = new LoginViewModel(this.GetProgressIndicator(), Dispatcher);
            this.DataContext = _model;            
        }

        private async void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _model.SetStatus("Loading...", true);
            _model.ShowBrowser = true;

            //Get the request token
            var requestToken = await App.DropNetClient.GetRequestToken();

            var tokenUrl = App.DropNetClient.BuildAuthorizeUrl(requestToken, _tokenCallbackUrl);
            //Open a browser with the URL
            loginBrowser.Navigate(new Uri(tokenUrl));
        }

        private async void loginBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //check for the call back url here
            if (e.Uri.Host == "dkdevelopment.net")
            {
                //SUCCESS!
                _model.SetStatus("Getting Access Token...", true);
                var accessToken = await App.DropNetClient.GetAccessToken();

                //TODO - Save this token/Secret for remember me function

                //Set the user Token/Secret in the current instance of DropNetClient
                App.DropNetClient.SetUserToken(accessToken);

                _model.SetStatus("Login successful", false, 5000);

                NavigationService.Navigate(new Uri("/ContentsPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //Loaded the login page
                _model.ClearStatus();
            }
        }

    }
}