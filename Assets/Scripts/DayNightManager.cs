using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class DayNightManager : MonoBehaviour
{
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

    [Header("Ressources")]
    [SerializeField] private Transform ressourceSpawnersParent;
    [SerializeField] private GameObject treeModel;
    [SerializeField] private GameObject bushModel;
    [SerializeField] private GameObject littleBushModel;
    [SerializeField] private GameObject stoneModel;
    [SerializeField] private GameObject littleStoneModel;
    [SerializeField] private GameObject crystalModel;
    [SerializeField] private GameObject littleCrystalModel;

    private float currentTime;
    private Vector3 defaultMoonPosition;
    private float defaultGlobalLightIntensity;
    private bool isDay;
    private bool isPaused;

    private int numberOfRessourceToSpawnPerDay = 3;
    private float ressourcesSpawnChanceMax;
    private Dictionary<IsometricPlayerMovementController.RessourceProducerType, Tuple<GameObject, float, float, float>> ressourcesSpawnChance;

    [HideInInspector] public static DayNightManager instance;

    private void Awake()
    {
        instance = this;
        defaultMoonPosition = moonLight.transform.position;
        defaultGlobalLightIntensity = globalLight.intensity;

        ressourcesSpawnChance = new Dictionary<IsometricPlayerMovementController.RessourceProducerType, Tuple<GameObject, float, float, float>>();
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.TREE, new Tuple<GameObject, float, float, float>(treeModel, 10, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.BUSH, new Tuple<GameObject, float, float, float>(bushModel, 3, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.LITTLE_BUSH, new Tuple<GameObject, float, float, float>(littleBushModel, 5, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.STONE, new Tuple<GameObject, float, float, float>(stoneModel, 1, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.LITTLE_STONE, new Tuple<GameObject, float, float, float>(littleStoneModel, 2, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.CRYSTAL, new Tuple<GameObject, float, float, float>(crystalModel, 0.5f, 0, 0));
        ressourcesSpawnChance.Add(IsometricPlayerMovementController.RessourceProducerType.LITTLE_CRYSTAL, new Tuple<GameObject, float, float, float>(littleCrystalModel, 1, 0, 0));

        float minValue = 0;
        ressourcesSpawnChanceMax = 0;
        foreach (var key in ressourcesSpawnChance.Keys.ToList())
        {
            Tuple<GameObject, float, float, float> value = ressourcesSpawnChance[key];

            ressourcesSpawnChanceMax += value.Item2;
            ressourcesSpawnChance[key] = new Tuple<GameObject, float, float, float>(value.Item1, value.Item2, minValue, ressourcesSpawnChanceMax);
            minValue = ressourcesSpawnChanceMax;
        }

        StartDay();
    }

    public void StartDay()
    {
        currentTime = 0;
        moonLight.enabled = false;
        globalLight.intensity = defaultGlobalLightIntensity;
        isDay = true;

        ResetRessources();
        SpawnRessources();
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
        if (!isPaused)
            currentTime += Time.deltaTime;

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

    private void OnEnable()
    {
        GameHandler.OnPauseResumeGameEvent += PauseResumeGame;
    }

    private void PauseResumeGame(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    private void OnDisable()
    {
        GameHandler.OnPauseResumeGameEvent -= PauseResumeGame;
    }

    private void ResetRessources()
    {
        for (int i = 0; i < ressourceSpawnersParent.childCount; i++)
        {
            Transform child = ressourceSpawnersParent.GetChild(i);

            for (int j = 0; j < child.childCount; j++)
            {
                Destroy(child.GetChild(j).gameObject);
            }
        }
    }

    private void SpawnRessources()
    {
        List<Transform> availableSpawner = new List<Transform>();
        for (int i = 0; i < ressourceSpawnersParent.childCount; i++)
        {
            availableSpawner.Add(ressourceSpawnersParent.GetChild(i));
        }

        for (int i = 0; i < numberOfRessourceToSpawnPerDay; i++)
        {
            int randSpawner = Random.Range(0, availableSpawner.Count);
            float randRessource = Random.Range(0, ressourcesSpawnChanceMax - 1);

            Transform currentSpawner = availableSpawner[randSpawner];
            GameObject currentRessource = null;

            availableSpawner.Remove(currentSpawner);

            foreach (var item in ressourcesSpawnChance)
            {
                if (randRessource >= item.Value.Item3 && randRessource < item.Value.Item4)
                {
                    currentRessource = Instantiate(item.Value.Item1);
                }
            }

            if (currentRessource != null)
            {
                currentRessource.transform.parent = currentSpawner;
                currentRessource.transform.localPosition = Vector3.zero;
                currentRessource.transform.localScale = Vector3.one;
            }
            else
                print("error occured");
        }
    }
}
