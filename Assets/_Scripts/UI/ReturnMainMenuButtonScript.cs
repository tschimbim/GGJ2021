using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenuButtonScript : MonoBehaviour
{
   public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
