using DropNet2.Models;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DropNet2Sample.ViewModels
{
    public class ContentsViewModel : BaseViewModel
    {
        private MetaData _meta;
        public MetaData MetaData
        {
            get { return _meta; }
            set
            {
                _meta = value;
                NotifyPropertyChanged("MetaData");
            }
        }

        private ObservableCollection<MetaData> _contents;
        public ObservableCollection<MetaData> Contents
        {
            get { return _contents; }
            set
            {
                _contents = value;
                NotifyPropertyChanged("Contents");
            }
        }

        private MetaData _selectedMeta;
        public MetaData SelectedMeta
        {
            get { return _selectedMeta; }
            set
            {
                _selectedMeta = value;
                NotifyPropertyChanged("SelectedMeta");
            }
        }

        public ContentsViewModel(ProgressIndicator prog, Dispatcher dispatcher)
            : base(prog, dispatcher)
        {
        }

    }
}
