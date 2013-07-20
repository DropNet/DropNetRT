using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DropNetRT.Sample.WP8.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ProgressIndicator _prog;
        private Dispatcher _dispatcher;

        public BaseViewModel(ProgressIndicator prog, Dispatcher dispatcher)
        {
            _prog = prog;
            _dispatcher = dispatcher;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                _dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }
                    catch { }
                });
            }
        }
        public void SetStatus(string message, bool isProgress, int? clearAfterMilliSeconds = null)
        {
            if (_prog == null) return;

            _dispatcher.BeginInvoke(() =>
            {
                _prog.IsIndeterminate = isProgress;
                _prog.IsVisible = true;
                _prog.Text = message;
            });

            if (clearAfterMilliSeconds.HasValue)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(clearAfterMilliSeconds.Value);
                    ClearStatus();
                });
            }
        }

        public void ClearStatus()
        {
            if (_prog == null) return;

            _dispatcher.BeginInvoke(() =>
            {
                _prog.IsIndeterminate = false;
                _prog.IsVisible = false;
                _prog.Text = string.Empty;
            });
        }
    }
}
