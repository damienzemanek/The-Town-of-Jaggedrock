using UnityEngine;

public static class DebugExtensions 
{
    static string Colorize(string text, string colorHex) => $"<color={colorHex}>{text}</color>";
    static string Bold(string text) => $"<b>{text}</b>";


    public static void Log(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.Log($"{Colorize("[<null>]", "#AAAAAA")}: {msg}");
        else if (obj is Object unityObj)
            Debug.Log($"{Colorize($"[SCRIPT: {Bold(unityObj.GetType().Name)}]", "#00FF00")} " +
                      $"{Colorize($"[G.O.: {Bold(unityObj.name)}]", "#FFA500")}: {msg}");
        else
            Debug.Log($"{Colorize($"[SCRIPT: {Bold(obj.GetType().Name)}]", "#00FF00")}: {msg}");
    }
    public static void Warn(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogWarning($"{Colorize("[<null>]", "#AAAAAA")}: {msg}");
        else if (obj is Object unityObj)
            Debug.LogWarning($"{Colorize($"[SCRIPT: {Bold(unityObj.GetType().Name)}]", "#00FF00")} " +
                             $"{Colorize($"[G.O.: {Bold(unityObj.name)}]", "#FFA500")}: {msg}");
        else
            Debug.LogWarning($"{Colorize($"[SCRIPT: {Bold(obj.GetType().Name)}]", "#00FF00")}: {msg}");
    }
    public static void Error(this object obj, string msg = "")
    {
        if (obj == null)
            Debug.LogError($"{Colorize("[<null>]", "#AAAAAA")}: {msg}");
        else if (obj is Object unityObj)
            Debug.LogError($"{Colorize($"[SCRIPT: {Bold(unityObj.GetType().Name)}]", "#00FF00")} " +
                         $"{Colorize($"[G.O.: {Bold(unityObj.name)}]", "#FFA500")}: {msg}");
        else
            Debug.LogError($"{Colorize($"[SCRIPT: {Bold(obj.GetType().Name)}]", "#00FF00")}: {msg}");
    }
}
