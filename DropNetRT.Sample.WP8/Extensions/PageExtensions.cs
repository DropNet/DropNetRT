using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropNetRT.Sample.WP8.Extensions
{
    public static class PageExtensions
    {
        public static ProgressIndicator GetProgressIndicator(this PhoneApplicationPage page)
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator == null)
            {
                progressIndicator = new ProgressIndicator();
                SystemTray.SetProgressIndicator(page, progressIndicator);
            }
            return progressIndicator;
        }
    }
}
