using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float playerMoney = 0f;
    public bool isTutorial = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps GameManager between scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }
}
