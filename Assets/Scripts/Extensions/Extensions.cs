using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T GetRandomItem<T>(this IEnumerable<T> enumerable, int maxIndex = 0)
    {
        var myEnumerable = enumerable as T[] ?? enumerable.ToArray();
        return myEnumerable[Random.Range(0, maxIndex != 0 ? maxIndex : myEnumerable.Length)];
    }
}