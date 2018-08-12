using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AwesomeGithub.Extension
{
    public static class ExtensionMethodsCollection
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> collectionToAdd)
        {
            foreach(var element in collectionToAdd)
            {
                collection.Add(element);
            }
        }
    }
}
