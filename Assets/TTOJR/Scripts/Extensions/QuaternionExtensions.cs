using UnityEngine;

public static class QuaternionExtensions
{
    public static Quaternion WithEuler(this Quaternion quaternion, float? x = null, float? y = null, float? z = null)
    {
        var euler = quaternion.eulerAngles;
        return Quaternion.Euler(x ?? euler.x, y ?? euler.y, z ?? euler.z);
    }
}
