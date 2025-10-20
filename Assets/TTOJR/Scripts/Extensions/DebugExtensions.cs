using UnityEngine;

public static class DebugExtensions 
{
    public static void Log(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.Log($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.Log(message: $"[SCRIPT: {unityObj.GetType()}] [G.O.: {unityObj.name}]: {msg}");
        else 
            Debug.Log($"[SCRIPT: {obj.GetType().Name}]: {msg}");
    }

    public static void Warn(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogWarning($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.LogWarning(message: $"[SCRIPT: {unityObj.GetType()}] [G.O.: {unityObj.name}]: {msg}");
        else
            Debug.LogWarning($"SCRIPT: [{obj.GetType().Name}]: {msg}");
    }

    public static void Error(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogError($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.LogError(message: $"[SCRIPT: {unityObj.GetType()}] [G.O.: {unityObj.name}]: {msg}");
        else
            Debug.LogError($"[SCRIPT: {obj.GetType().Name}]: {msg}");
    }
}
