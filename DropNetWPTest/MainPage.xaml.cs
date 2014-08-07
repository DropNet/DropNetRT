using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace DropNetRTWP
{
    using Windows.UI.Popups;
    using DropNetRT;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TokenCallbackUrl = "the call back url you defined in dropbox dashboard";
        private const string ApiKey = "API_Key";
        private const string ApiSecret = "API_Secret";
        private LoginViewModel model;
        private readonly DropNetClient dropNetClient;


        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            dropNetClient = new DropNetClient(ApiKey, ApiSecret) {UseSandbox = true};
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var message = "Welcome to the DropNet Sample App.";
            message += "This app demonstrates how to authenticate with the Dropbox API";
            message += "through DropNetRT as well as basic API functionality.";
            var msgDialog = new MessageDialog(message);
            await msgDialog.ShowAsync();
            

            model = new LoginViewModel(prgBar, Dispatcher);
            DataContext = model;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            model.SetStatus("Loading...", true, 1000);
            model.ShowBrowser = true;

            //Get the request token
            var requestToken = await dropNetClient.GetRequestToken();

            var tokenUrl = dropNetClient.BuildAuthorizeUrl(requestToken, TokenCallbackUrl);
            
            loginBrowser.Navigate(new Uri(tokenUrl));
        }

        
        private async void loginBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //check for the call back url here
            if (args.Uri.Host == "cmatskas.com")
            {
                //SUCCESS!
                model.SetStatus("Getting Access Token...", true, 1000);
                var accessToken = await dropNetClient.GetAccessToken();

                //TODO - Save this token/Secret for remember me function

                //Set the user Token/Secret in the current instance of DropNetClient
                dropNetClient.SetUserToken(accessToken);


                // Upload a file
                
                /* var testString = "hello world";
                var testData = new byte[testString.Length * sizeof(char)];
                Buffer.BlockCopy(testString.ToCharArray(), 0, testData, 0, testData.Length);
                await dropNetClient.Upload(@"/", "testfile.txt", testData);

               */
                model.SetStatus("Login successful", false, 5000);

            }
            else
            {
                //Loaded the login page
                await model.ClearStatus();
            }
        }
    }
}
