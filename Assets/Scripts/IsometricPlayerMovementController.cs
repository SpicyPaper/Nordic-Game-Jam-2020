using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public ParticleSystem selectedElementParticleSystemModel;

    private IsometricCharacterRenderer isoRenderer;
    private Rigidbody2D rbody;
    private bool isAbleToCollect;
    private bool isRessourceCurrentlyCollected;
    private ResourcesManager.ResourceQuantityType currentResourceQuantityType;
    private GameObject currentProducer;
    private ParticleSystem selectedElementParticleSystem;

    private int woodQuantity;
    private int stoneQuantity;
    private int crystalQuantity;

    private float elapsedTimeCollectingRessource;
    private static float NEEDED_TIME_COLLECT_ONE_RESSOURCE_IN_S = 1;

    private float currentElapsedScaleStep;

    private bool resetScaleAllowed;

    public GameObject FireballPrefab;

    public float PlayerDamage = 20f;

    public float AttackSpeed = 1 / 2f;

    private bool canAttack = true;

    [SerializeField] private TextMeshProUGUI txtWoodQuantity;
    [SerializeField] private TextMeshProUGUI txtStoneQuantity;
    [SerializeField] private TextMeshProUGUI txtCrystalQuantity;

    public int WoodQuantity { get => woodQuantity; set { woodQuantity = value; txtWoodQuantity.text = value.ToString(); } }
    public int StoneQuantity { get => stoneQuantity; set { stoneQuantity = value; txtStoneQuantity.text = value.ToString(); } }
    public int CrystalQuantity { get => crystalQuantity; set { crystalQuantity = value; txtCrystalQuantity.text = value.ToString(); } }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();

        selectedElementParticleSystem = Instantiate(selectedElementParticleSystemModel);
        selectedElementParticleSystem.Stop();

        WoodQuantity = 0;
        StoneQuantity = 0;
        CrystalQuantity = 0;
    }

    private void OnEnable()
    {
        DayNightManager.OnStartNightEvent += StartNight;
    }

    private void OnDisable()
    {
        DayNightManager.OnStartNightEvent -= StartNight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.transform.parent.tag;
        if (ResourcesManager.RessourceQuantityContains(tag))
        {
            isAbleToCollect = true;

            currentResourceQuantityType = ResourcesManager.GetRessourceQuantityType(tag);
            currentProducer = collision.transform.parent.gameObject;

            selectedElementParticleSystem.time = 0;
            selectedElementParticleSystem.Play();
            selectedElementParticleSystem.transform.parent = currentProducer.transform;
            selectedElementParticleSystem.transform.localPosition = Vector3.zero + Vector3.up * 0.1f;
            selectedElementParticleSystem.transform.localScale = Vector3.one;

            Debug.Log("Press E to collect ressource");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.transform.parent.tag;
        if (ResourcesManager.RessourceQuantityContains(tag))
        {
            selectedElementParticleSystem.Stop();
            isAbleToCollect = false;
            isRessourceCurrentlyCollected = false;
        }
    }

    public Vector2 GetPlayerFront()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

        playerPosition += isoRenderer.GetDirection() * 0.5f;
        return playerPosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ShootFireball(isoRenderer.GetDirection());
        }

        if (isAbleToCollect && currentProducer != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isRessourceCurrentlyCollected)
                {
                    elapsedTimeCollectingRessource = 0;
                    currentElapsedScaleStep = 0;
                }

                isRessourceCurrentlyCollected = true;
            }

            if (isRessourceCurrentlyCollected)
            {
                resetScaleAllowed = true;
                elapsedTimeCollectingRessource += Time.deltaTime;

                float scalePerStep = 1f / ResourcesManager.GetQuantityRessource(currentResourceQuantityType);

                float scale = Mathf.Lerp(0f, scalePerStep, Time.deltaTime);
                currentElapsedScaleStep += scale;

                Renderer[] renderer = currentProducer.GetComponentsInChildren<Renderer>();
                foreach (var item in renderer)
                {
                    item.transform.localScale -= Vector3.one * scale;
                }

                if (elapsedTimeCollectingRessource >= NEEDED_TIME_COLLECT_ONE_RESSOURCE_IN_S)
                {
                    if (currentProducer.GetComponentInChildren<Renderer>().transform.localScale.x > scalePerStep / 2)
                    {
                        elapsedTimeCollectingRessource -= NEEDED_TIME_COLLECT_ONE_RESSOURCE_IN_S;
                        currentElapsedScaleStep = 0;
                        CollectOneRessource(currentResourceQuantityType);
                    }
                }

                if (currentProducer.GetComponentInChildren<Renderer>().transform.localScale.x <= 0)
                {
                    selectedElementParticleSystem.Stop();
                    selectedElementParticleSystem.transform.parent = transform;

                    isRessourceCurrentlyCollected = false;
                    resetScaleAllowed = false;
                    isAbleToCollect = false;
                    isRessourceCurrentlyCollected = false;

                    CollectOneRessource(currentResourceQuantityType);
                    Destroy(currentProducer);
                }
            }
        }

        if (resetScaleAllowed)
        {
            if (!isRessourceCurrentlyCollected)
            {
                resetScaleAllowed = false;
                isRessourceCurrentlyCollected = false;
                currentProducer.GetComponentInChildren<Renderer>().transform.localScale += Vector3.one * currentElapsedScaleStep;
            }
        }
        
    }

    private void ShootFireball(Vector2 direction)
    {
        if (canAttack)
        {
            var fireball = Instantiate(FireballPrefab, transform.position, Quaternion.identity).GetComponent<Fireball>();
            fireball.SetDirection(direction);
            fireball.SetDamage(PlayerDamage);
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(AttackSpeed);
        canAttack = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
    }

    private void StartNight()
    {
        selectedElementParticleSystem.Stop();
        selectedElementParticleSystem.transform.parent = transform;
    }

    private void CollectAllRessource(ResourcesManager.ResourceQuantityType type)
    {
        CollectRessource(type, ResourcesManager.GetQuantityRessource(type));
    }

    private void CollectOneRessource(ResourcesManager.ResourceQuantityType type)
    {
        CollectRessource(type, 1);
    }

    private void CollectRessource(ResourcesManager.ResourceQuantityType type, int quantity)
    {
        AddRessource(ResourcesManager.GetRessourceType(type), quantity);

        Debug.Log(quantity + " more " + ResourcesManager.GetRessourceString(type) + " collected");
    }

    private void AddRessource(ResourcesManager.ResourceType type, int quantity)
    {
        switch (type)
        {
            case ResourcesManager.ResourceType.WOOD:
                WoodQuantity += quantity;
                break;
            case ResourcesManager.ResourceType.STONE:
                StoneQuantity += quantity;
                break;
            case ResourcesManager.ResourceType.CRYSTAL:
                CrystalQuantity += quantity;
                break;
            default:
                break;
        }
    }
}
