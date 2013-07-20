﻿using DropNetRT.Models;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DropNetRT.Sample.WP8.Extensions;

namespace DropNetRT.Sample.WP8.ViewModels
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


        internal async void LoadPath(string p)
        {
            //get the metadata
            var metadata = await App.DropNetClient.GetMetaData(p);

            //now load the viewmodel with it
            MetaData = metadata;
            Contents = metadata.Contents.ToObservableCollection();
            SelectedMeta = null;
        }
    }
}
