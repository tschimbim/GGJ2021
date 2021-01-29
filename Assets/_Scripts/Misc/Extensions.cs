using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions0
{
    /// <summary>
    /// Returns true if the distance between to the other vector is less than the given distance
    /// </summary>
    public static bool DistanceCheck(this Vector3 a, Vector3 b, float distance) => (b - a).sqrMagnitude < distance * distance;
}
