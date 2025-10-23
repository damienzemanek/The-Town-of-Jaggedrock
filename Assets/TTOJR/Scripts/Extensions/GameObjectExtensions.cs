using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject SetActiveThen(this GameObject gameObject, bool val)
    {
        gameObject.SetActive(val);
        return gameObject;
    }
}
