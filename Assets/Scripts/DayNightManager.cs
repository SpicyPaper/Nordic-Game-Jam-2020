using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class DayNightManager : MonoBehaviour
{
    public delegate void StartDayEvent();
    public delegate void StartNightEvent();

    public static event StartDayEvent OnStartDayEvent;
    public static event StartNightEvent OnStartNightEvent;

    [Header("D/N Params")]
    public float DayDuration;
    public float NightDuration;

    [Range(0.0f, 1.0f)]
    public float DarkStart;

    [Range(0.0f, 1.0f)]
    public float NightIntensity;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D moonLight;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private Transform endMoonPosition;

    private float currentTime;
    private Vector3 defaultMoonPosition;
    private float defaultGlobalLightIntensity;
    private float defaultPlayerLightIntensity;
    private bool isDay;
    private bool isPaused;

    [HideInInspector] public static DayNightManager instance;

    private void Awake()
    {
        instance = this;
        defaultMoonPosition = moonLight.transform.position;
        defaultGlobalLightIntensity = globalLight.intensity;
        defaultPlayerLightIntensity = playerLight.intensity;
    }

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        currentTime = 0;
        moonLight.enabled = false;
        globalLight.intensity = defaultGlobalLightIntensity;
        playerLight.intensity = defaultPlayerLightIntensity;
        isDay = true;

        OnStartDayEvent();
    }

    public void StartNight()
    {
        isDay = false;
        currentTime = 0;
        moonLight.transform.position = defaultMoonPosition;
        moonLight.enabled = true;

        OnStartNightEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
            currentTime += Time.deltaTime;

        if (isDay && currentTime >= DayDuration * DarkStart)
        {
            globalLight.intensity = Mathf.Lerp(defaultGlobalLightIntensity, NightIntensity, (currentTime - DayDuration * DarkStart) / (DayDuration * DarkStart));
            playerLight.intensity = Mathf.Lerp(defaultPlayerLightIntensity, NightIntensity, (currentTime - DayDuration * DarkStart) / (DayDuration * DarkStart));
            if (currentTime >= DayDuration)
            {
                StartNight();
            }
        }
        else if (!isDay)
        {

            moonLight.transform.position = Vector3.Lerp(defaultMoonPosition, endMoonPosition.position, currentTime / NightDuration);
            if (currentTime >= NightDuration)
            {
                StartDay();
            }
        }
    }

    private void OnEnable()
    {
        GameHandler.OnPauseGameEvent += PauseResumeGame;
    }

    private void PauseResumeGame(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    private void OnDisable()
    {
        GameHandler.OnPauseGameEvent -= PauseResumeGame;
    }
}
