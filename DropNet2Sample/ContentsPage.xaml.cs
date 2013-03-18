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
using DropNet2Sample.Extensions;

namespace DropNet2Sample
{
    public partial class ContentsPage : PhoneApplicationPage
    {
        private ContentsViewModel _model;

        public ContentsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _model = new ContentsViewModel(this.GetProgressIndicator(), Dispatcher);
            this.DataContext = _model;

            _model.LoadPath("/");
        }
    }
}