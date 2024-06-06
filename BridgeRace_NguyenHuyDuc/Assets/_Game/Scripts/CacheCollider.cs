using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheCollider<T>
{
    private static Dictionary<Collider, T> dict = new Dictionary<Collider, T>();


    public static T GetCollider(Collider collider)
    {
        if (!Contains(collider))
        {
            dict.Add(collider, collider.GetComponent<T>());
        }

        return dict[collider];
    }

    public static bool Contains(Collider collider)
    {
        if (dict.ContainsKey(collider))
        {
            return true;
        }
        return false;
    }
}
