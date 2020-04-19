using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public delegate void PauseResumeGameEvent(bool isPaused);
    public static event PauseResumeGameEvent OnPauseResumeGameEvent;

    [SerializeField] private Canvas inGameCanvas;

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

    private void OnEnable()
    {
        InGameMenuManager.OnResumeGameEvent += ResumeOrPauseGame;
    }

    private void OnDisable()
    {
        InGameMenuManager.OnResumeGameEvent -= ResumeOrPauseGame;
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

            inGameCanvas.gameObject.SetActive(true);
        }
        else if (currentGameState == GameState.PAUSE)
        {
            currentGameState = GameState.PLAY;

            inGameCanvas.gameObject.SetActive(false);
        }

        OnPauseResumeGameEvent(currentGameState == GameState.PAUSE);
    }
}
