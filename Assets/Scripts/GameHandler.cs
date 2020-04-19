using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public delegate void PauseGameEvent(bool isPaused);
    public static event PauseGameEvent OnPauseGameEvent;

    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text nightCount;

    public static GameHandler instance;

    public RectTransform ChestHealthRect;

    public enum GameState
    {
        PLAY,
        PAUSE,
        END
    }

    private GameState currentGameState;

    public float ChestHealth = 100f;

    private float currentChestHealth;

    public float CurrentChestHealth { get => currentChestHealth; set { currentChestHealth = value; ChestHealthRect.localScale = new Vector3(CurrentChestHealth / ChestHealth, 1, 1); } }

    private void Awake()
    {
        instance = this;
        endGamePanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.PLAY;
        CurrentChestHealth = ChestHealth;
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

        OnPauseGameEvent(currentGameState == GameState.PAUSE);
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
        currentGameState = GameState.END;

        if (EnemiesManager.CurrentLevel < 2)
        {
            nightCount.text = EnemiesManager.CurrentLevel + " night";
        }
        else
        {
            nightCount.text = EnemiesManager.CurrentLevel + " nights";
        }

        endGamePanel.SetActive(true);
        OnPauseGameEvent(true);
    }

}
