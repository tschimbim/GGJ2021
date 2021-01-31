using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorScript : MonoBehaviour
{
    public Transform myPlayerTarget = null;
    public Transform myBotTarget = null;

    private void Update()
    {

        GhostScript ghostScript = GetComponent<GhostScript>();
        if (ghostScript == null || !ghostScript.isGhost)
        {
            myPlayerTarget.gameObject.SetActive(false);
            myBotTarget.gameObject.SetActive(false);
            return;
        }

        bool isGameRunning = GameManager.instance.gameState == GameState.InGame;

        myPlayerTarget.gameObject.SetActive(isGameRunning);
        myBotTarget.gameObject.SetActive(isGameRunning);

        if (!isGameRunning)
            return;

        Vector2 pos2D = transform.position.XZ();

        if (ghostScript.target != null)
        {
            Vector2 targetPos2D = ghostScript.target.transform.position.XZ();
            Vector2 targetDir = (targetPos2D - pos2D).normalized;
            myBotTarget.rotation = Quaternion.LookRotation(targetDir.To3D());
        }
        else
            myBotTarget.gameObject.SetActive(false);

        if (ghostScript.otherPlayer != null)
        {
            Vector2 playerPos2D = ghostScript.otherPlayer.transform.position.XZ();
            Vector2 playerDir = (playerPos2D - pos2D).normalized;
            myPlayerTarget.rotation = Quaternion.LookRotation(playerDir.To3D());
        }
        else
            myPlayerTarget.gameObject.SetActive(false);
    }
}
