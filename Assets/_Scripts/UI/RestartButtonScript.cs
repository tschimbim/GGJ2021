using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonScript : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.instance.StartGame();
    }
}
