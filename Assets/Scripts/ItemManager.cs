using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject preview;

    [Header("Turret")]
    [SerializeField] TextMeshProUGUI txtTurretWood;
    [SerializeField] TextMeshProUGUI txtTurretRock;
    [SerializeField] TextMeshProUGUI txtTurretCrystal;

    private int turretWoodPrice;

    public int TurretWoodPrice
    {
        get { return turretWoodPrice; }
        set { turretWoodPrice = value;
            txtTurretWood.text = turretWoodPrice.ToString();
        }
    }


    private int turretRockPrice;

    public int TurretRockPrice
    {
        get { return turretRockPrice; }
        set
        {
            turretRockPrice = value;
            txtTurretRock.text = turretRockPrice.ToString();
        }
    }


    private int turretCrystalPrice;

    public int TurretCrystalPrice
    {
        get { return turretCrystalPrice; }
        set
        {
            turretCrystalPrice = value;
            txtTurretCrystal.text = turretCrystalPrice.ToString();
        }
    }

    [Header("Wall")]
    [SerializeField] TextMeshProUGUI txtWallWood;
    [SerializeField] TextMeshProUGUI txtWallRock;
    [SerializeField] TextMeshProUGUI txtWallCrystal;
    private int wallWoodPrice;

    public int WallWoodPrice
    {
        get { return wallWoodPrice; }
        set
        {
            wallWoodPrice = value;
            txtWallWood.text = wallWoodPrice.ToString();
        }
    }


    private int wallRockPrice;

    public int WallRockPrice
    {
        get { return wallRockPrice; }
        set
        {
            wallRockPrice = value;
            txtWallRock.text = wallRockPrice.ToString();
        }
    }


    private int wallCrystalPrice;

    public int WallCrystalPrice
    {
        get { return wallCrystalPrice; }
        set
        {
            wallCrystalPrice = value;
            txtWallCrystal.text = wallCrystalPrice.ToString();
        }
    }

    [Header("Trap")]
    [SerializeField] TextMeshProUGUI txtTrapWood;
    [SerializeField] TextMeshProUGUI txtTrapRock;
    [SerializeField] TextMeshProUGUI txtTrapCrystal;

    private int trapWoodPrice;

    public int TrapWoodPrice
    {
        get { return trapWoodPrice; }
        set
        {
            trapWoodPrice = value;
            txtTrapWood.text = trapWoodPrice.ToString();
        }
    }


    private int trapRockPrice;

    public int TrapRockPrice
    {
        get { return trapRockPrice; }
        set
        {
            trapRockPrice = value;
            txtTrapRock.text = trapRockPrice.ToString();
        }
    }


    private int trapCrystalPrice;

    public int TrapCrystalPrice
    {
        get { return trapCrystalPrice; }
        set
        {
            trapCrystalPrice = value;
            txtTrapCrystal.text = trapCrystalPrice.ToString();
        }
    }

    [SerializeField] private GameObject itemHolder;
    private bool isInItemMod;


    // Start is called before the first frame update
    void Start()
    {
        TurretWoodPrice = 10;
        TurretRockPrice = 15;
        TurretCrystalPrice = 5;

        TurretWoodPrice = 10;
        TurretRockPrice = 15;
        TurretCrystalPrice = 5;

        TurretWoodPrice = 10;
        TurretRockPrice = 15;
        TurretCrystalPrice = 5;

        itemHolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EnableDisableItemMode();
        }
        if (isInItemMod)
        {
            Vector3 position = grid.GetCellCenterWorld(grid.WorldToCell(player.GetComponent<IsometricPlayerMovementController>().GetPlayerFront()));
            preview.transform.position = position;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Instantiate(turretPrefab, position, Quaternion.identity);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Instantiate(wallPrefab, position, Quaternion.identity);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }
        }

         

    }

    void EnableDisableItemMode()
    {
        isInItemMod = !isInItemMod;
        itemHolder.SetActive(isInItemMod);
        preview.SetActive(isInItemMod);
    }


    
}
