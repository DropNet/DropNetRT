using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropNet2Sample.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            foreach (T item in source)
                result.Add(item);

            return result;
        }
    }
}
