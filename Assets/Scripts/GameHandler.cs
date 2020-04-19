using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public delegate void PauseResumeGameEvent(bool isPaused);
    public static event PauseResumeGameEvent OnPauseResumeGameEvent;

    public static GameHandler instance;

    public RectTransform ChestHealthRect;

    public enum GameState
    {
        PLAY,
        PAUSE
    }

    private GameState currentGameState;

    public float ChestHealth = 100f;

    private float currentChestHealth;

    public float CurrentChestHealth { get => currentChestHealth; set { currentChestHealth = value; ChestHealthRect.localScale = new Vector3(CurrentChestHealth / ChestHealth, 1, 1); } }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.PLAY;
        CurrentChestHealth = ChestHealth;
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

    public void DamageChest(float damage)
    {
        CurrentChestHealth = Mathf.Max(0, CurrentChestHealth - damage);

        if (CurrentChestHealth <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // TODO: GAME END
    }

}
