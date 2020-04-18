using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public delegate void PauseResumeGameEvent(bool isPaused);
    public static event PauseResumeGameEvent OnPauseResumeGameEvent;

    public enum GameState
    {
        PLAY,
        PAUSE
    }

    private GameState currentGameState;

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeOrPauseGame();
        }
    }

    private void ResumeOrPauseGame()
    {
        if (currentGameState == GameState.PLAY)
        {
            currentGameState = GameState.PAUSE;
        }
        else if (currentGameState == GameState.PAUSE)
        {
            currentGameState = GameState.PLAY;
        }

        OnPauseResumeGameEvent(currentGameState == GameState.PAUSE);
    }

}
