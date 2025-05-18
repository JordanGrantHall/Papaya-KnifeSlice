using KnifeSlicer.GameManagement;
using PapayaPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapayaLifeCycleGame : MonoBehaviour, IPapayaGame
{

    public GameManager _gameManager;

    public PapayaLifeCycleGame(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void CountdownStarted()
    {
        _gameManager.CountdownStarted();
    }

    public void GoalAchieved()
    {
        _gameManager.GoalAchieved();
    }

    public void LoadGame(IPapayaGameConfigurations gameConfig)
    {
        _gameManager.LoadGame(gameConfig);
    }

    public void PauseGame()
    {
        _gameManager.PauseGame();
    }

    public void ResumeGame()
    {
        _gameManager.ResumeGame();
    }

    public void SetGameVolume(float volume)
    {
        _gameManager.SetGameVolume(volume);
    }

    public void SetHaptics(bool playHaptics)
    {
        _gameManager.SetHaptics(playHaptics);
    }

    public void SetMusicVolume(float volume)
    {
        _gameManager.SetMusicVolume(volume);
    }

    public void StartGame()
    {
        _gameManager.StartGame();
    }

    public void TerminateGame()
    {
        _gameManager.TerminateGame();
    }

    public void UnloadGame()
    {
        _gameManager.UnloadGame();
    }
}
