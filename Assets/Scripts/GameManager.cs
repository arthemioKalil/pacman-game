using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float StartupTime;

    public float LifeLostTimer;

    private enum GameState
    {
        Starting,
        Playing,
        LifeLost,
        GameOver,
        Victory
    }
    private GameState _gameState;
    private int _victoryCount;

    private GhostAI[] _allGhosts;
    private CharacterMotor _pacmanMotor;

    private GhostHouse _ghostHouse;

    private float _lifeLostTimer;
    private bool _isGameOver;

    public event Action OnGameStarted;
    public event Action OnVictory;
    public event Action OnGameOver;

    private void Start()
    {
        var allColletibles = FindObjectsOfType<Collectible>();

        _victoryCount = 0;
        foreach (var collectible in allColletibles)
        {
            if (collectible.IsVictoryCondition)
            {
                _victoryCount++;
                collectible.OnCollected += Collectible_OnCollected;
            }
        }

        var pacman = GameObject.FindWithTag("Player");
        _pacmanMotor = pacman.GetComponent<CharacterMotor>();
        _allGhosts = FindObjectsOfType<GhostAI>();
        StopAllCharacters();

        _ghostHouse = FindObjectOfType<GhostHouse>();
        _ghostHouse.enabled = false;

        pacman.GetComponent<Life>().OnLifeRemoved += Pacman_OnLifeRemoved; ;

        _gameState = GameState.Starting;
    }

    private void Pacman_OnLifeRemoved(int remainingLives)
    {
        StopAllCharacters();

        _lifeLostTimer = LifeLostTimer;
        _gameState = GameState.LifeLost;

        _isGameOver = remainingLives <= 0;
    }

    private void Collectible_OnCollected(int _, Collectible collectible)
    {
        //_ significa que é um parametro descarte
        _victoryCount--;

        if (_victoryCount <= 0)
        {
            Debug.Log("Victory");
            _gameState = GameState.Victory;

            StopAllCharacters();
            OnVictory?.Invoke();

        }
        //como foi feito uma inscrição desse collectible lá em cima, quando é removido, é bom fazer a desubscrição tambem
        collectible.OnCollected -= Collectible_OnCollected;
    }
    private void StartAllCharacters()
    {
        _pacmanMotor.enabled = true;

        foreach (var ghost in _allGhosts)
        {
            ghost.StartMoving();
        }
    }
    private void StopAllCharacters()
    {
        _pacmanMotor.enabled = false;

        foreach (var ghost in _allGhosts)
        {
            ghost.StopMoving();
        }
    }
    private void ResetAllCharacters()
    {
        _pacmanMotor.ResetPosition();

        foreach (var ghost in _allGhosts)
        {
            ghost.Reset();
        }

        StartAllCharacters();
    }

    void Update()
    {
        switch (_gameState)
        {
            case GameState.LifeLost:
                _lifeLostTimer -= Time.deltaTime;

                if (_lifeLostTimer <= 0)
                {
                    if (_isGameOver)
                    {
                        _gameState = GameState.GameOver;
                        OnGameOver?.Invoke();
                    }
                    else
                    {
                        ResetAllCharacters();
                        _gameState = GameState.Playing;
                    }
                }
                break;

            case GameState.Starting:
                StartupTime -= Time.deltaTime;

                if (StartupTime <= 0)
                {
                    _gameState = GameState.Playing;
                    StartAllCharacters();
                    _ghostHouse.enabled = true;
                }
                OnGameStarted?.Invoke();
                break;

            case GameState.GameOver:
            case GameState.Victory:
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }
}
