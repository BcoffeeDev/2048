using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGames
{
    public static class Extension
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}
