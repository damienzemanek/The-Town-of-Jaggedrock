using UnityEngine;

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
            if(go.TryGetComponent<T>(out T found))  return found;
            go.Error($"Failed to TryGet{thisType} on {found}");

            return null;
        }

        object CannotTryGet(object obj)
        {
            obj.Error($"Failed to Tryget {thisType} on {obj}");
            return null;
        }

    }



}
