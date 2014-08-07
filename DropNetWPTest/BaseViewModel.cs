namespace DropNetRTWP
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Windows.System.Threading;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly ProgressBar progressBar;
        private readonly CoreDispatcher coreDispatcher;

        public BaseViewModel(ProgressBar prog, CoreDispatcher dispatcher)
        {
            progressBar = prog;
            coreDispatcher = dispatcher;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected async void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                    () => PropertyChanged(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
        public void SetStatus(string message, bool isProgress, int? clearAfterMilliSeconds)
        {
            var completed = false;
            var delayValue = clearAfterMilliSeconds.HasValue ? clearAfterMilliSeconds.Value : 1000;
            var delay = TimeSpan.FromMilliseconds(delayValue);

            var delayTimer = ThreadPoolTimer.CreateTimer(
            async source =>
            {
                if (coreDispatcher != null)
                {
                    await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        progressBar.IsIndeterminate = isProgress;
                        progressBar.Visibility = Visibility.Visible;
                    });
                }
                completed = true;
            }, delay,
                async source =>
                {
                    await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                        async () =>
                    {
                        if (completed)
                        {
                            await ClearStatus();
                        }
                    });
                }
            
            );
        }

        public async Task ClearStatus()
        {
            if (progressBar == null) return;

            await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = Visibility.Collapsed;
            });
        }
    }
}
