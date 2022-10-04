using UnityEngine;

namespace Utils
{
    public static class GameObjectUtils
    {
        public static T CreateObject<T>(string prefabPath)
        {
            return Object.Instantiate(Resources.Load<GameObject>(prefabPath)).GetComponent<T>();
        }
    }
}