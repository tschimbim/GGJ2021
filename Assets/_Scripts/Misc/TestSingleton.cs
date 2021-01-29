using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : SingletonBase<TestSingleton>
{
    private void Start()
    {
        Debug.LogFormat("Singletone rules! {0}", TestSingleton.instance.name);
    }
}
