using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions0
{
    /// <summary>
    /// Returns true if the distance between to the other vector is less than the given distance
    /// </summary>
    public static bool DistanceCheck(this Vector3 a, Vector3 b, float distance) => (b - a).sqrMagnitude < distance * distance;

    /// <summary>
    /// Returns a random element from this list using <see cref="Random.Range(int, int)"/>
    /// </summary>
    public static T RandomElement<T>(this List<T> list) => list[Random.Range(0, list.Count)];
    /// <summary>
    /// Returns a random element from this list using <see cref="Random.Range(int, int)"/>
    /// </summary>
    public static T RandomElement<T>(this T[] array) => array[Random.Range(0, array.Length)];

    public static Vector2 XZ(this Vector3 pos) => new Vector2(pos.x, pos.z);
    public static Vector3 To3D(this Vector2 pos) => new Vector3(pos.x, 0.0f, pos.y);
}
