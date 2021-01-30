using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WinnerTextScript : MonoBehaviour
{
    private Text myText = default;

    private void Start()
    {
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = string.Format("Player with name {0} has won!", GameManager.instance.winningPlayer.name);
    }
}
