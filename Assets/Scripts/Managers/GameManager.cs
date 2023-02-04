using System;
using System.Collections.Generic;
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

    private List<IMovableCharacter> _allMovableCharacters;

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

        _allMovableCharacters = new List<IMovableCharacter>();

        var pacman = GameObject.FindWithTag("Player");
        _allMovableCharacters.Add(pacman.GetComponent<PacmanInput>());
        var _allGhosts = FindObjectsOfType<GhostAI>();
        _allMovableCharacters.AddRange(_allGhosts);
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
        //_ significa que � um parametro descarte
        _victoryCount--;

        if (_victoryCount <= 0)
        {
            Debug.Log("Victory");
            _gameState = GameState.Victory;

            StopAllCharacters();
            OnVictory?.Invoke();

        }
        //como foi feito uma inscri��o desse collectible l� em cima, quando � removido, � bom fazer a desubscri��o tambem
        collectible.OnCollected -= Collectible_OnCollected;
    }
    private void StartAllCharacters()
    {
        foreach (var character in _allMovableCharacters)
        {
            character.StartMoving();
        }
    }
    private void StopAllCharacters()
    {
        foreach (var character in _allMovableCharacters)
        {
            character.StopMoving();
        }
    }
    private void ResetAllCharactersPositions()
    {
        foreach (var character in _allMovableCharacters)
        {
            character.ResetPosition();
        }
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
                        ResetAllCharactersPositions();
                        StartAllCharacters();
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
