using UnityEngine;

public static class DebugExtensions 
{
    public static void Log(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.Log($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.Log($"[{unityObj.GetType()}] [{unityObj.name}]: {msg}");
        else 
            Debug.Log($"[{obj.GetType().Name}]: {msg}");
    }

    public static void Warn(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogWarning($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.LogWarning($"[{unityObj.GetType()}] [{unityObj.name}]: {msg}");
        else
            Debug.LogWarning($"[{obj.GetType().Name}]: {msg}");
    }

    public static void Error(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogError($"[<null>]: {msg}");
        else if (obj is Object unityObj)
            Debug.LogError($"[{unityObj.GetType()}] [{unityObj.name}]: {msg}");
        else
            Debug.LogError($"[{obj.GetType().Name}]: {msg}");
    }
}
