using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DropNet2Sample.ViewModels;
using DropNet2.Exceptions;

namespace DropNet2Sample
{
    public partial class ContentsPage : PhoneApplicationPage
    {
        private ContentsViewModel _model;

        public ContentsPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                var str = "ASDFDFSDFSDFSDFFSDF";
                var rawBytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, rawBytes, 0, rawBytes.Length);

                var uploadResponse = await App.DropNetClient.Upload("/Test/1", "BLAH.png", rawBytes);
            }
            catch (DropboxException ex)
            {

            }
        }
    }
}