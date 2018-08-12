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

        public static void AddRange<K, T>(this Dictionary<K, T> collection, IEnumerable<Tuple<K, T>> collectionToAdd)
        {
            foreach(var tuple in collectionToAdd)
            {
                if(!collection.ContainsKey(tuple.Item1))
                {
                    collection.Add(tuple.Item1, tuple.Item2); 
                }
            }
        }
    }
}
