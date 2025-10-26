using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject SetActiveThen(this GameObject gameObject, bool val)
        {
            gameObject.SetActive(val);
            return gameObject;
        }
    }

}