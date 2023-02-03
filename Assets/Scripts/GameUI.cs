using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject ReadyMessage;

    public GameObject GameOverMessage;

    public AudioSource AudioSource;
    public AudioClip BeginningMusic;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _gameManager.OnGameStarted += GameManager_OnGameStarted;
        _gameManager.OnGameOver += GameManager_OnGameOver;

        AudioSource.PlayOneShot(BeginningMusic);
    }

    private void GameManager_OnGameStarted()
    {
        ReadyMessage.SetActive(false);
    }
    private void GameManager_OnGameOver()
    {
        GameOverMessage.SetActive(true);
    }

}
