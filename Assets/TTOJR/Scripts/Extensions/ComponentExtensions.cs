using UnityEngine;

public static class ComponentExtensions 
{
    public static T TryGet<T>(this Component component) where T : Component
    {
        if(component.TryGetComponent<T>(out var _component)) return _component;

        component.Error($"failed to TryGet Component {component.GetType()}");
        return null;
    }
}
