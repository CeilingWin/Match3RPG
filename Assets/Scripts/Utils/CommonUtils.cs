using System.Collections.Generic;

namespace Utils
{
    public static class CommonUtils
    {
        public static V GetValue<K,V>(Dictionary<K,V> dic, K key, V defaultValue)
        {
            if (dic.ContainsKey(key)) return dic[key];
            dic.Add(key, defaultValue);
            return defaultValue;
        }
    }
}