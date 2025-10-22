using System;
using UnityEngine;

public static class ObjectExtensions 
{
    public static void NullCheck(this object script, object input)
    {
        if (input == null) script.Error($"Null Check FAILED. {input} resulted in NULL value");

    }
}
