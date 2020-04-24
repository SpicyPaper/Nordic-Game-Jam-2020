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
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private GameObject preview;

    [Header("Turret")]
    [SerializeField] TextMeshProUGUI txtTurretWood;
    [SerializeField] TextMeshProUGUI txtTurretRock;
    [SerializeField] TextMeshProUGUI txtTurretCrystal;

    private Vector3 initPos;

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

    public static bool IsInItemMod;


    // Start is called before the first frame update
    void Start()
    {
        TurretWoodPrice = 15;
        TurretRockPrice = 15;
        TurretCrystalPrice = 5;

        WallWoodPrice = 10;
        WallRockPrice = 10;
        WallCrystalPrice = 0;

        TrapWoodPrice = 10;
        TrapRockPrice = 10;
        TrapCrystalPrice = 10;

        initPos = transform.localPosition;
        IsInItemMod = true;
        EnableDisableItemMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EnableDisableItemMode();
        }

        if (IsInItemMod)
        {
            Vector3 position = grid.GetCellCenterWorld(grid.WorldToCell(player.GetComponent<IsometricPlayerMovementController>().GetPlayerFront()));
            preview.transform.position = position;

            if (PreviewManager.IsCraftingAllowed)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SpawnIfMoney(TurretWoodPrice, TurretRockPrice, TurretCrystalPrice, position, turretPrefab);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SpawnIfMoney(WallWoodPrice, WallRockPrice, WallCrystalPrice, position, wallPrefab);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    SpawnIfMoney(TrapWoodPrice, TrapRockPrice, TrapCrystalPrice, position, trapPrefab);
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (PreviewManager.CurrentCollidingItem != null)
                {
                    Destroy(PreviewManager.CurrentCollidingItem);
                }
            }
        }
    }

    private void SpawnIfMoney(int woodPrice, int rockPrice, int crystalPrice, Vector3 position, GameObject prefab)
    {
        IsometricPlayerMovementController p = player.GetComponent<IsometricPlayerMovementController>();
        if (p.WoodQuantity >= woodPrice && p.StoneQuantity >= rockPrice && p.CrystalQuantity >= crystalPrice)
        {
            Instantiate(prefab, position, Quaternion.identity);
            p.WoodQuantity -= woodPrice;
            p.StoneQuantity -= rockPrice;
            p.CrystalQuantity -= crystalPrice;
        }
    }

    void EnableDisableItemMode()
    {
        IsInItemMod = !IsInItemMod;
        if (IsInItemMod)
        {
            transform.localPosition = initPos;
        }
        else
        {
            transform.localPosition += Vector3.down * 180;
        }
        preview.SetActive(IsInItemMod);
    }
}
