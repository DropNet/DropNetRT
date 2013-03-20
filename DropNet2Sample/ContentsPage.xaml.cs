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
using DropNet2.Models;

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

        private void lsbContents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selected = (e.AddedItems[0] as MetaData);

                if (selected == null) return;

                if (selected.Is_Dir)
                {
                    //navigate to the new dir
                    _model.LoadPath(selected.Path);
                }
            }
        }

    }
}