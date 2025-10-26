using UnityEngine;


namespace Extensions
{
    public static class ComponentExtensions
    {
        public static T TryGet<T>(this Object obj) where T : Component
        {
            string thisType = typeof(T).Name;

            switch (obj)
            {
                case Component comp: return TryOnComponent(comp);
                case GameObject go: return TryOnGameObject(go);
                default: return (T)CannotTryGet(obj);
            }

            T TryOnComponent(Component comp)
            {
                if (comp.TryGetComponent<T>(out T found)) return found;
                obj.Error($"Failed to TryGet {thisType} on {found}");
                return null;
            }

            T TryOnGameObject(GameObject go)
            {
                if (go.TryGetComponent<T>(out T found)) return found;
                go.Error($"Failed to TryGet{thisType} on {found}");

                return null;
            }

            object CannotTryGet(object obj)
            {
                obj.Error($"Failed to Tryget {thisType} on {obj}");
                return null;
            }

        }

        public static bool TryGet<T>(this Object obj, out T result) where T : Component
        {
            result = null;
            string thisType = typeof(T).Name;

            switch (obj)
            {
                case Component comp: return TryOnComponent(comp, out result);
                case GameObject go: return TryOnGameObject(go, out result);
                default: return (T)CannotTryGet(obj, thisType);
            }

            bool TryOnComponent(Component comp, out T found)
            {
                if (comp.TryGetComponent<T>(out found)) return true;
                comp.Error($"Failed to TryGet {thisType} on {comp.name}");
                return false;
            }

            bool TryOnGameObject(GameObject go, out T found)
            {
                if (go.TryGetComponent<T>(out found)) return true;
                go.Error($"Failed to TryGet {thisType} on {go.name}");
                return false;
            }

            object CannotTryGet(object badObj, string type)
            {
                badObj.Error($"Failed to TryGet {type} on {badObj}");
                return null;
            }
        }



    }

}