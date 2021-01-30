using System;
using UnityEngine;
using UnityEngine.UI;

public class PreGameTimerScript : MonoBehaviour
{
    public Text myText = default;

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        myText.text = Mathf.CeilToInt(GameManager.instance.pregameCooldown).ToString();
    }
}
