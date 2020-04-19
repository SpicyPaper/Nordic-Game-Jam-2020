using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private Transform enemieSpawnersParent;
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private GameObject enemieModel;
    [SerializeField] private GameObject chest;

    private bool spawningEnabled;
    public static int CurrentLevel;
    List<Transform> availableSpawners;
    private float elapsedTimeInterval;

    private float currentInterval = 30;
    private float currentNumberOfEnemies = 5;

    private float maxInterval = 1;
    private float maxNumberOfEnemies = 30;

    private float intervalDivisor = 1.5f;
    private float numberOfEnemiesMult = 1.5f;

    private void Awake()
    {
        CurrentLevel = -1;

        availableSpawners = new List<Transform>();
        for (int i = 0; i < enemieSpawnersParent.childCount; i++)
        {
            availableSpawners.Add(enemieSpawnersParent.GetChild(i));
        }
    }

    private void OnEnable()
    {
        DayNightManager.OnStartDayEvent += StartDay;
        DayNightManager.OnStartNightEvent += StartNight;
    }

    private void OnDisable()
    {
        DayNightManager.OnStartDayEvent -= StartDay;
        DayNightManager.OnStartNightEvent -= StartNight;
    }

    private void Update()
    {
        if (spawningEnabled)
        {
            elapsedTimeInterval += Time.deltaTime;

            if (elapsedTimeInterval >= currentInterval)
            {
                elapsedTimeInterval -= currentInterval;

                for (int i = 0; i < currentNumberOfEnemies; i++)
                {
                    int rand = Random.Range(0, availableSpawners.Count - 1);

                    Transform spawner = availableSpawners[rand];

                    GameObject enemie = Instantiate(enemieModel);
                    enemie.transform.parent = enemiesParent;
                    enemie.transform.localPosition = spawner.localPosition;
                    enemie.transform.localScale = Vector3.one;

                    enemie.GetComponent<AIDestinationSetter>().target = chest.transform;
                }
            }
        }
    }

    private void StartNight()
    {
        spawningEnabled = true;
        elapsedTimeInterval = currentInterval;
    }

    private void StartDay()
    {
        spawningEnabled = false;
        CurrentLevel++;
        currentInterval /= intervalDivisor;
        currentNumberOfEnemies *= numberOfEnemiesMult;

        print(currentInterval + " " + currentNumberOfEnemies);
    }
}
