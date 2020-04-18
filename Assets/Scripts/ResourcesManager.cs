using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourcesManager : MonoBehaviour
{
    public enum ResourceType
    {
        WOOD,
        STONE,
        CRYSTAL
    }
    public enum ResourceQuantityType
    {
        BIG_WOOD,
        MEDIUM_WOOD,
        SMALL_WOOD,
        BIG_STONE,
        MEDIUM_STONE,
        SMALL_STONE,
        BIG_CRYSTAL,
        MEDIUM_CRYSTAL,
        SMALL_CRYSTAL
    }

    public enum ResourceProducerType
    {
        PLAIN_BARREL,
        PLAIN_TREE,
        PLAIN_TREE_STUMP,
        PLAIN_BUSH,
        PLAIN_LITTLE_BUSH,
        MOUNTAIN_CRYSTAL,
        MOUNTAIN_LITTLE_CRYTAL,
        MOUNTAIN_STONE,
        MOUTAIN_LITTLE_STONE,
        DESERT_TREE,
        DESERT_STONE,
        DESERT_LITTLE_STONE,
        DESERT_BONES_1,
        DESERT_BONES_2,
        DESERT_BONES_3,
        SNOW_STONE,
        SNOW_LITTLE_STONE,
        SNOW_STONE_SNOW_ON,
        SNOW_LITTLE_STONE_SNOW_ON,
        SNOW_STONE_LAYERED,
        SNOW_TREE_1,
        SNOW_TREE_2,
        SNOW_TREE_3
    }

    [SerializeField] private Transform resourceSpawnersParent;
    [SerializeField] private Transform resourcePlainSpawnersParent;
    [SerializeField] private Transform resourceDesertSpawnersParent;
    [SerializeField] private Transform resourceMountainSpawnersParent;
    [SerializeField] private Transform resourceSnowSpawnersParent;

    [Header("Plain")]
    [SerializeField] private GameObject barelModel;
    [SerializeField] private GameObject treeModel;
    [SerializeField] private GameObject treeStumpModel;
    [SerializeField] private GameObject bushModel;
    [SerializeField] private GameObject littleBushModel;

    [Header("Mountain")]
    [SerializeField] private GameObject crystalModel;
    [SerializeField] private GameObject littleCrystalModel;
    [SerializeField] private GameObject mountainStoneModel;
    [SerializeField] private GameObject littleMountainStoneModel;

    [Header("Desert")]
    [SerializeField] private GameObject desertTree;
    [SerializeField] private GameObject desertLittleStone;
    [SerializeField] private GameObject desertStone;
    [SerializeField] private GameObject bones1;
    [SerializeField] private GameObject bones2;
    [SerializeField] private GameObject bones3;

    [Header("Snow")]
    [SerializeField] private GameObject snowStoneModel;
    [SerializeField] private GameObject littleSnowStoneModel;
    [SerializeField] private GameObject snowStoneSnowOnModel;
    [SerializeField] private GameObject littleSnowStoneSnowOnModel;
    [SerializeField] private GameObject littleSnowStoneSnowOnLayeredModel;
    [SerializeField] private GameObject snowTree1Model;
    [SerializeField] private GameObject snowTree2Model;
    [SerializeField] private GameObject snowTree3Model;

    private int numberOfPlainResourcesPerDay = 60;
    private int numberOfDesertResourcesPerDay = 20;
    private int numberOfSnowResourcesPerDay = 40;
    private int numberOfMountainResourcesPerDay = 20;
    private float spawnMaxChancePlain;
    private float spawnMaxChanceDesert;
    private float spawnMaxChanceMountain;
    private float spawnMaxChanceSnow;
    private Dictionary<ResourceProducerType, Tuple<float, float, float>> resourcePlainSpawnChance;
    private Dictionary<ResourceProducerType, Tuple<float, float, float>> resourceSnowSpawnChance;
    private Dictionary<ResourceProducerType, Tuple<float, float, float>> resourceMountainSpawnChance;
    private Dictionary<ResourceProducerType, Tuple<float, float, float>> resourceDesertSpawnChance;

    private void Awake()
    {
        resourcePlainSpawnChance = new Dictionary<ResourceProducerType, Tuple<float, float, float>>();
        resourcePlainSpawnChance.Add(ResourceProducerType.PLAIN_BARREL, new Tuple<float, float, float>(20, 0, 0));
        resourcePlainSpawnChance.Add(ResourceProducerType.PLAIN_BUSH, new Tuple<float, float, float>(50, 0, 0));
        resourcePlainSpawnChance.Add(ResourceProducerType.PLAIN_LITTLE_BUSH, new Tuple<float, float, float>(50, 0, 0));
        resourcePlainSpawnChance.Add(ResourceProducerType.PLAIN_TREE, new Tuple<float, float, float>(100, 0, 0));
        resourcePlainSpawnChance.Add(ResourceProducerType.PLAIN_TREE_STUMP, new Tuple<float, float, float>(20, 0, 0));

        resourceSnowSpawnChance = new Dictionary<ResourceProducerType, Tuple<float, float, float>>();
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_LITTLE_STONE, new Tuple<float, float, float>(50, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_LITTLE_STONE_SNOW_ON, new Tuple<float, float, float>(50, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_STONE, new Tuple<float, float, float>(20, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_STONE_LAYERED, new Tuple<float, float, float>(10, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_STONE_SNOW_ON, new Tuple<float, float, float>(20, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_TREE_1, new Tuple<float, float, float>(10, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_TREE_2, new Tuple<float, float, float>(10, 0, 0));
        resourceSnowSpawnChance.Add(ResourceProducerType.SNOW_TREE_3, new Tuple<float, float, float>(10, 0, 0));

        resourceMountainSpawnChance = new Dictionary<ResourceProducerType, Tuple<float, float, float>>();
        resourceMountainSpawnChance.Add(ResourceProducerType.MOUNTAIN_CRYSTAL, new Tuple<float, float, float>(10, 0, 0));
        resourceMountainSpawnChance.Add(ResourceProducerType.MOUNTAIN_LITTLE_CRYTAL, new Tuple<float, float, float>(20, 0, 0));
        resourceMountainSpawnChance.Add(ResourceProducerType.MOUNTAIN_STONE, new Tuple<float, float, float>(50, 0, 0));
        resourceMountainSpawnChance.Add(ResourceProducerType.MOUTAIN_LITTLE_STONE, new Tuple<float, float, float>(50, 0, 0));

        resourceDesertSpawnChance = new Dictionary<ResourceProducerType, Tuple<float, float, float>>();
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_BONES_1, new Tuple<float, float, float>(5, 0, 0));
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_BONES_2, new Tuple<float, float, float>(5, 0, 0));
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_BONES_3, new Tuple<float, float, float>(5, 0, 0));
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_LITTLE_STONE, new Tuple<float, float, float>(50, 0, 0));
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_STONE, new Tuple<float, float, float>(20, 0, 0));
        resourceDesertSpawnChance.Add(ResourceProducerType.DESERT_TREE, new Tuple<float, float, float>(5, 0, 0));

        float minValue = 0;
        spawnMaxChancePlain = 0;
        foreach (var key in resourcePlainSpawnChance.Keys.ToList())
        {
            Tuple<float, float, float> value = resourcePlainSpawnChance[key];

            spawnMaxChancePlain += value.Item1;
            resourcePlainSpawnChance[key] = new Tuple<float, float, float>(value.Item1, minValue, spawnMaxChancePlain);
            minValue = spawnMaxChancePlain;
        }

        minValue = 0;
        spawnMaxChanceSnow = 0;
        foreach (var key in resourceSnowSpawnChance.Keys.ToList())
        {
            Tuple<float, float, float> value = resourceSnowSpawnChance[key];

            spawnMaxChanceSnow += value.Item1;
            resourceSnowSpawnChance[key] = new Tuple<float, float, float>(value.Item1, minValue, spawnMaxChanceSnow);
            minValue = spawnMaxChanceSnow;
        }

        minValue = 0;
        spawnMaxChanceMountain = 0;
        foreach (var key in resourceMountainSpawnChance.Keys.ToList())
        {
            Tuple<float, float, float> value = resourceMountainSpawnChance[key];

            spawnMaxChanceMountain += value.Item1;
            resourceMountainSpawnChance[key] = new Tuple<float, float, float>(value.Item1, minValue, spawnMaxChanceMountain);
            minValue = spawnMaxChanceMountain;
        }

        minValue = 0;
        spawnMaxChanceDesert = 0;
        foreach (var key in resourceDesertSpawnChance.Keys.ToList())
        {
            Tuple<float, float, float> value = resourceDesertSpawnChance[key];

            spawnMaxChanceDesert += value.Item1;
            resourceDesertSpawnChance[key] = new Tuple<float, float, float>(value.Item1, minValue, spawnMaxChanceDesert);
            minValue = spawnMaxChanceDesert;
        }
    }

    private void OnEnable()
    {
        DayNightManager.OnStartDayEvent += StartDay;
    }

    private void OnDisable()
    {
        DayNightManager.OnStartDayEvent -= StartDay;
    }

    private void StartDay()
    {
        ResetResources();
        SpawnResources(resourcePlainSpawnChance, resourcePlainSpawnersParent, numberOfPlainResourcesPerDay, spawnMaxChancePlain);
        SpawnResources(resourceSnowSpawnChance, resourceSnowSpawnersParent, numberOfSnowResourcesPerDay, spawnMaxChanceSnow);
        SpawnResources(resourceMountainSpawnChance, resourceMountainSpawnersParent, numberOfMountainResourcesPerDay, spawnMaxChanceMountain);
        SpawnResources(resourceDesertSpawnChance, resourceDesertSpawnersParent, numberOfDesertResourcesPerDay, spawnMaxChanceDesert);
    }

    private void ResetResources()
    {
        for (int i = 0; i < resourceSpawnersParent.childCount; i++)
        {
            Transform child = resourceSpawnersParent.GetChild(i);

            for (int j = 0; j < child.childCount; j++)
            {
                Transform childOfChild = child.GetChild(j);

                for (int k = 0; k < childOfChild.childCount; k++)
                {
                    Destroy(childOfChild.GetChild(k).gameObject);
                }
            }
        }
    }

    private void SpawnResources(Dictionary<ResourceProducerType, Tuple<float, float, float>> ressourcesSpawnChance, Transform parent, int numberOfResourcePerDay, float spawnMaxChance)
    {
        List<Transform> availableSpawner = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            availableSpawner.Add(parent.GetChild(i));
        }

        for (int i = 0; i < numberOfResourcePerDay; i++)
        {
            int randSpawner = Random.Range(0, availableSpawner.Count);
            float randRessource = Random.Range(0, spawnMaxChance - 1);

            Transform currentSpawner = availableSpawner[randSpawner];
            GameObject currentRessource = null;

            availableSpawner.Remove(currentSpawner);

            foreach (var item in ressourcesSpawnChance)
            {
                if (randRessource >= item.Value.Item2 && randRessource < item.Value.Item3)
                {
                    currentRessource = Instantiate(GetResourceGameObject(item.Key));
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

    private GameObject GetResourceGameObject(ResourceProducerType type)
    {
        switch (type)
        {
            case ResourceProducerType.PLAIN_BARREL:
                return barelModel;
            case ResourceProducerType.PLAIN_TREE:
                return treeModel;
            case ResourceProducerType.PLAIN_TREE_STUMP:
                return treeStumpModel;
            case ResourceProducerType.PLAIN_BUSH:
                return bushModel;
            case ResourceProducerType.PLAIN_LITTLE_BUSH:
                return littleBushModel;
            case ResourceProducerType.MOUNTAIN_CRYSTAL:
                return crystalModel;
            case ResourceProducerType.MOUNTAIN_LITTLE_CRYTAL:
                return littleCrystalModel;
            case ResourceProducerType.MOUNTAIN_STONE:
                return mountainStoneModel;
            case ResourceProducerType.MOUTAIN_LITTLE_STONE:
                return littleMountainStoneModel;
            case ResourceProducerType.DESERT_TREE:
                return desertTree;
            case ResourceProducerType.DESERT_STONE:
                return desertStone;
            case ResourceProducerType.DESERT_LITTLE_STONE:
                return desertLittleStone;
            case ResourceProducerType.DESERT_BONES_1:
                return bones1;
            case ResourceProducerType.DESERT_BONES_2:
                return bones2;
            case ResourceProducerType.DESERT_BONES_3:
                return bones3;
            case ResourceProducerType.SNOW_STONE:
                return snowStoneModel;
            case ResourceProducerType.SNOW_LITTLE_STONE:
                return littleSnowStoneModel;
            case ResourceProducerType.SNOW_STONE_SNOW_ON:
                return snowStoneSnowOnModel;
            case ResourceProducerType.SNOW_LITTLE_STONE_SNOW_ON:
                return littleSnowStoneSnowOnModel;
            case ResourceProducerType.SNOW_STONE_LAYERED:
                return littleSnowStoneSnowOnLayeredModel;
            case ResourceProducerType.SNOW_TREE_1:
                return snowTree1Model;
            case ResourceProducerType.SNOW_TREE_2:
                return snowTree2Model;
            case ResourceProducerType.SNOW_TREE_3:
                return snowTree3Model;
        }

        return null;
    }

    public static ResourceType GetRessourceType(ResourceQuantityType type)
    {
        switch (type)
        {
            case ResourceQuantityType.BIG_WOOD:
            case ResourceQuantityType.MEDIUM_WOOD:
            case ResourceQuantityType.SMALL_WOOD:
                return ResourceType.WOOD;
            case ResourceQuantityType.BIG_STONE:
            case ResourceQuantityType.MEDIUM_STONE:
            case ResourceQuantityType.SMALL_STONE:
                return ResourceType.STONE;
            case ResourceQuantityType.BIG_CRYSTAL:
            case ResourceQuantityType.MEDIUM_CRYSTAL:
            case ResourceQuantityType.SMALL_CRYSTAL:
                return ResourceType.CRYSTAL;
        }

        return ResourceType.WOOD;
    }

    public static ResourceQuantityType GetRessourceQuantityType(string tag)
    {
        switch (tag)
        {
            case "Big_Wood":
                return ResourceQuantityType.BIG_WOOD;
            case "Medium_Wood":
                return ResourceQuantityType.MEDIUM_WOOD;
            case "Small_Wood":
                return ResourceQuantityType.SMALL_WOOD;
            case "Big_Stone":
                return ResourceQuantityType.BIG_STONE;
            case "Medium_Stone":
                return ResourceQuantityType.MEDIUM_STONE;
            case "Small_Stone":
                return ResourceQuantityType.SMALL_STONE;
            case "Big_Crystal":
                return ResourceQuantityType.BIG_CRYSTAL;
            case "Medium_Crystal":
                return ResourceQuantityType.MEDIUM_CRYSTAL;
            case "Small_Crystal":
                return ResourceQuantityType.SMALL_CRYSTAL;
        }

        return ResourceQuantityType.BIG_WOOD;
    }

    public static string GetRessourceString(ResourceQuantityType type)
    {
        switch (type)
        {
            case ResourceQuantityType.BIG_WOOD:
            case ResourceQuantityType.MEDIUM_WOOD:
            case ResourceQuantityType.SMALL_WOOD:
                return "Wood";
            case ResourceQuantityType.BIG_STONE:
            case ResourceQuantityType.MEDIUM_STONE:
            case ResourceQuantityType.SMALL_STONE:
                return "Stone";
            case ResourceQuantityType.BIG_CRYSTAL:
            case ResourceQuantityType.MEDIUM_CRYSTAL:
            case ResourceQuantityType.SMALL_CRYSTAL:
                return "Crystal";
        }

        return "";
    }

    public static bool RessourceQuantityContains(string value)
    {
        if (Enum.IsDefined(typeof(ResourceQuantityType), value.ToUpper()))
            return true;

        return false;
    }

    public static int GetQuantityRessource(ResourceQuantityType type)
    {
        switch (type)
        {
            case ResourceQuantityType.BIG_WOOD:
            case ResourceQuantityType.BIG_STONE:
            case ResourceQuantityType.BIG_CRYSTAL:
                return 5;
            case ResourceQuantityType.MEDIUM_WOOD:
            case ResourceQuantityType.MEDIUM_STONE:
            case ResourceQuantityType.MEDIUM_CRYSTAL:
                return 3;
            case ResourceQuantityType.SMALL_WOOD:
            case ResourceQuantityType.SMALL_STONE:
            case ResourceQuantityType.SMALL_CRYSTAL:
                return 2;
        }

        return 0;
    }
}
