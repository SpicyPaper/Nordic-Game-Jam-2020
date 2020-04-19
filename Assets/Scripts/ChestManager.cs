using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Transform chestSpawnersParent;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        List<Transform> availableSpawners = new List<Transform>();
        for (int i = 0; i < chestSpawnersParent.childCount; i++)
        {
            availableSpawners.Add(chestSpawnersParent.GetChild(i));
        }

        int rand = Random.Range(0, availableSpawners.Count - 1);

        Transform spawner = availableSpawners[rand];

        chest.transform.localPosition = spawner.localPosition;
        chest.transform.localScale = Vector3.one;

        player.transform.localPosition = chest.transform.localPosition + Vector3.right * 0.5f;
    }
}
