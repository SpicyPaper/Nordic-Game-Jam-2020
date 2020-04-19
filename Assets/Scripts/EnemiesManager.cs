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
    /// <summary>
    /// Item1 = interval in s between each spawning wave
    /// Item2 = number of enemies spawning each wave
    /// </summary>
    private List<Tuple<float, int>> levels;
    List<Transform> availableSpawners;
    private float elapsedTimeInterval;

    private void Awake()
    {
        CurrentLevel = -1;

        levels = new List<Tuple<float, int>>()
        {
            new Tuple<float, int>(20, 5),
            new Tuple<float, int>(10, 5),
            new Tuple<float, int>(5, 10),
            new Tuple<float, int>(5, 15),
            new Tuple<float, int>(1, 30)
        };

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

            if (elapsedTimeInterval >= levels[CurrentLevel].Item1)
            {
                elapsedTimeInterval -= levels[CurrentLevel].Item1;

                for (int i = 0; i < levels[CurrentLevel].Item2; i++)
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
        elapsedTimeInterval = levels[CurrentLevel].Item1;
    }

    private void StartDay()
    {
        spawningEnabled = false;
        CurrentLevel++;
    }
}
