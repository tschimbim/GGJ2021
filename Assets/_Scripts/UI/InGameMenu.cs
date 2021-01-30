using UnityEngine;

public class InGameMenu : SingletonBase<InGameMenu>
{
    public GameObject myPreGameUI = default;
    public GameObject myInGameUI = default;
    public GameObject myPostGameUI = default;

    private void Start()
    {
        ShowUI((GameState)(-1));
    }

    public void ShowUI(GameState gameState)
    {
        myPreGameUI.SetActive(gameState == GameState.PreGame);
        myInGameUI.SetActive(gameState == GameState.InGame);
        myPostGameUI.SetActive(gameState == GameState.PostGame);
    }
}
