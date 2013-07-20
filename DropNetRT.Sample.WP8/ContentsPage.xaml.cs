using DropNetRT.Models;
using DropNetRT.Sample.WP8.Extensions;
using DropNetRT.Sample.WP8.ViewModels;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace DropNetRT.Sample.WP8
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (_model.MetaData.Path == "/")
            {
                base.OnBackKeyPress(e);
            }
            else
            {
                var lastslash = _model.MetaData.Path.LastIndexOf("/");
                var parentPath = _model.MetaData.Path.Remove(lastslash);
                //Check if you can go up a directory
                if (string.IsNullOrEmpty(parentPath))
                {
                    //root
                    _model.LoadPath("/");
                    e.Cancel = true;
                }
                else
                {
                    _model.LoadPath(parentPath);
                    e.Cancel = true;
                }
            }
        }

        private void lsbContents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selected = (e.AddedItems[0] as MetaData);

                if (selected == null) return;

                if (selected.IsDirectory)
                {
                    //navigate to the new dir
                    _model.LoadPath(selected.Path);
                }
                else
                {
                    //Do something here for files
                }
            }
        }

    }
}