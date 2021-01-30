using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerScript : MonoBehaviour
{
    public Text myText = default;

    // Update is called once per frame
    void Update()
    {
        myText.text = string.Format(TimeSpan.FromSeconds(GameManager.instance.ingameTimeLeft).ToString(@"mm\:ss"));
    }
}
