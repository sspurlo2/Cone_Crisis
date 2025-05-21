using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

public void PlayGame()
    {
        GameManager.Instance.playerMoney = 100;
        GameManager.Instance.isTutorial = false;
        SceneManager.LoadScene("Game_scene");
    }

}
