using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
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
    private bool isDay;

    [HideInInspector] public static DayNightManager instance;

    private void Awake()
    {
        instance = this;
        defaultMoonPosition = moonLight.transform.position;
        defaultGlobalLightIntensity = globalLight.intensity;

        StartDay();
    }

    public void StartDay()
    {
        currentTime = 0;
        moonLight.enabled = false;
        globalLight.intensity = defaultGlobalLightIntensity;
        isDay = true;
    }

    public void StartNight()
    {
        isDay = false;
        currentTime = 0;
        moonLight.transform.position = defaultMoonPosition;
        moonLight.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        print(currentTime);
        if (isDay && currentTime >= DayDuration * DarkStart)
        {
            globalLight.intensity = Mathf.Lerp(defaultGlobalLightIntensity, NightIntensity, (currentTime - DayDuration * DarkStart) / (DayDuration * DarkStart));
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
}
