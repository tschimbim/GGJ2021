using UnityEngine;

public class InGameMenu : SingletonBase<InGameMenu>
{
    public GameObject myPreGameUI = default;
    public GameObject myInGameUI = default;
    public GameObject myPostGameUI = default;

    public void ShowUI(GameState gameState)
    {
        myPreGameUI.SetActive(gameState == GameState.PreGame);
        myInGameUI.SetActive(gameState == GameState.InGame);
        myPostGameUI.SetActive(gameState == GameState.PostGame);
    }
}
