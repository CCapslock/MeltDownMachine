using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mans
{
    public sealed class LoadResources
    {
        private static Dictionary<string, object> _data = new Dictionary<string, object>();

        public static void Init()
        {
            _data.Clear();
        }


        private static void Add<T>(string key, T value) where T : Object
        {
            _data.Add(key, value);
        }

        public static T GetValue<T>(string key) where T : Object
        {
            if (!_data.ContainsKey(key))
            {
                var keyLoad=Load<T>(key);
                Add(key, keyLoad);
            }
            return _data[key] as T;
        }

        private static T Load<T>(string key) where T: Object
        {
            var path = key;
            var resLoad= Resources.Load<T>(Path.ChangeExtension(path, null));
            if (resLoad==null) Debug.LogWarning($"Resource don't load: {path}");
            return resLoad;
        }

    }
}