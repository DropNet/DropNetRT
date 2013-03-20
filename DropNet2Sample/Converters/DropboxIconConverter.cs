using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DropNet2Sample.Converters
{
    public class DropboxIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var theme = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible ? "dark" : "light";

            try
            {
                //Overrides
                switch (value.ToString())
                {
                    case "page_white_cplusplus":
                    case "page_white_csharp":
                    case "page_white_actionscript":
                    case "page_white_c":
                    case "page_white_h":
                        value = "page_white_code";
                        break;
                    case "folder_photos":
                        value = "folder_public";
                        break;
                    case "page_white_zip":
                    case "page_white_compressed":
                        value = "page_white_new";
                        break;
                    case "page_white_flash":
                    case "page_white_ruby":
                    case "page_white_php":
                        value = "page_white_visualstudio";
                        break;
                }

                var iconpath = string.Format("/icons/{0}/{1}.png", theme, value.ToString());

                return new BitmapImage(new Uri(iconpath, UriKind.Relative));
            }
            catch (Exception ex)
            {
                //TODO - Default Icon
                if (value != null && value.ToString().Contains("folder"))
                {
                    return new BitmapImage(new Uri(string.Format("/icons/{0}/folder.png", theme), UriKind.Relative));
                }
                else
                {
                    return new BitmapImage(new Uri(string.Format("/icons/{0}/page_white.png", theme), UriKind.Relative));
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
