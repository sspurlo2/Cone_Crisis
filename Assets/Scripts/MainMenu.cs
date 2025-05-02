using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
                SceneManager.LoadScene("Game_scene"); // or whatever your real scene is called
        GameManager.Instance.playerMoney = 50f; //starter money
    }
}
