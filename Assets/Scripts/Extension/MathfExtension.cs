using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtension
{
    public static T GetRandom<T>(this IList<T> list)
    {
        if (list == null)
            return default;
        if (list.Count <= 0)
            return default;
        var random = Random.Range(0, list.Count);
        return list[random];
    }
}
