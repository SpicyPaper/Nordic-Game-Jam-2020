using System;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    private enum RessourceType
    {
        WOOD,
        STONE,
        CRYSTAL
    }

    private enum RessourceProducerType
    {
        TREE,
        BUSH,
        LITTLE_BUSH,
        STONE,
        LITTLE_STONE,
        CRYSTAL,
        LITTLE_CRYSTAL
    }

    public float movementSpeed = 1f;
    public ParticleSystem selectedElementParticleSystemModel;

    private IsometricCharacterRenderer isoRenderer;
    private Rigidbody2D rbody;
    private bool isAbleToCollect;
    private bool isRessourceCurrentlyCollected;
    private RessourceProducerType currentProducerType;
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

    public float PlayerDamage = 5f;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();

        selectedElementParticleSystem = Instantiate(selectedElementParticleSystemModel);
        selectedElementParticleSystem.Stop();

        woodQuantity = 0;
        stoneQuantity = 0;
        crystalQuantity = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (RessourceProducerContains(collision.tag))
        {
            isAbleToCollect = true;

            currentProducerType = GetRessourceProducerType(collision.tag);
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
        if (RessourceProducerContains(collision.tag))
        {
            selectedElementParticleSystem.Stop();
            isAbleToCollect = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootFireball(isoRenderer.GetDirection());
        }

        if (isAbleToCollect)
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

                float scalePerStep = 1f / GetQuantityRessource(currentProducerType);

                float scale = Mathf.Lerp(0f, scalePerStep, Time.deltaTime);
                currentElapsedScaleStep += scale;

                currentProducer.GetComponentInChildren<Renderer>().transform.localScale -= Vector3.one * scale;

                if (elapsedTimeCollectingRessource >= NEEDED_TIME_COLLECT_ONE_RESSOURCE_IN_S)
                {
                    if (currentProducer.GetComponentInChildren<Renderer>().transform.localScale.x > scalePerStep / 2)
                    {
                        elapsedTimeCollectingRessource -= NEEDED_TIME_COLLECT_ONE_RESSOURCE_IN_S;
                        currentElapsedScaleStep = 0;
                        CollectOneRessource(currentProducerType);
                    }
                }

                if (currentProducer.GetComponentInChildren<Renderer>().transform.localScale.x <= 0)
                {
                    selectedElementParticleSystem.Stop();
                    selectedElementParticleSystem.transform.parent = transform;

                    isRessourceCurrentlyCollected = false;
                    resetScaleAllowed = false;
                    isAbleToCollect = false;

                    CollectOneRessource(currentProducerType);
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
        var fireball = Instantiate(FireballPrefab, transform.position, Quaternion.identity).GetComponent<Fireball>();
        fireball.SetDirection(direction);
        fireball.SetDamage(PlayerDamage);
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

        if (inputVector.x != 0 ||
            inputVector.y != 0)
        {
            isRessourceCurrentlyCollected = false;
        }
    }

    private RessourceType GetRessourceType(RessourceProducerType type)
    {
        switch (type)
        {
            case RessourceProducerType.TREE:
                return RessourceType.WOOD;
        }

        return RessourceType.WOOD;
    }

    private RessourceProducerType GetRessourceProducerType(string tag)
    {
        switch (tag)
        {
            case "Tree":
                return RessourceProducerType.TREE;
            case "Bush":
                return RessourceProducerType.BUSH;
            case "Little_Bush":
                return RessourceProducerType.LITTLE_BUSH;
            case "Stone":
                return RessourceProducerType.STONE;
            case "Little_Stone":
                return RessourceProducerType.LITTLE_STONE;
            case "Crystal":
                return RessourceProducerType.CRYSTAL;
            case "Little_Crystal":
                return RessourceProducerType.LITTLE_CRYSTAL;
        }

        return RessourceProducerType.TREE;
    }

    private string GetRessourceProducerString(RessourceProducerType type)
    {
        switch (type)
        {
            case RessourceProducerType.TREE:
                return "Tree";
            case RessourceProducerType.BUSH:
                return "Bush";
            case RessourceProducerType.LITTLE_BUSH:
                return "Little_Bush";
            case RessourceProducerType.STONE:
                return "Stone";
            case RessourceProducerType.LITTLE_STONE:
                return "Little_Stone";
            case RessourceProducerType.CRYSTAL:
                return "Crystal";
            case RessourceProducerType.LITTLE_CRYSTAL:
                return "Little_Crystal";
        }

        return "Tree";
    }

    private bool RessourceProducerContains(string value)
    {
        if (Enum.IsDefined(typeof(RessourceProducerType), value.ToUpper()))
            return true;

        return false;
    }

    private int GetQuantityRessource(RessourceProducerType type)
    {
        switch (type)
        {
            case RessourceProducerType.TREE:
                return 5;
            case RessourceProducerType.BUSH:
                return 3;
            case RessourceProducerType.LITTLE_BUSH:
                return 1;
            case RessourceProducerType.STONE:
                return 5;
            case RessourceProducerType.LITTLE_STONE:
                return 3;
            case RessourceProducerType.CRYSTAL:
                return 5;
            case RessourceProducerType.LITTLE_CRYSTAL:
                return 3;
        }

        return 0;
    }

    private void CollectAllRessource(RessourceProducerType type)
    {
        CollectRessource(type, GetQuantityRessource(type));
    }

    private void CollectOneRessource(RessourceProducerType type)
    {
        CollectRessource(type, 1);
    }

    private void CollectRessource(RessourceProducerType type, int quantity)
    {
        AddRessource(GetRessourceType(type), quantity);

        Debug.Log(quantity + " more " + GetRessourceProducerString(type) + " collected");
    }

    private void AddRessource(RessourceType type, int quantity)
    {
        switch (type)
        {
            case RessourceType.WOOD:
                woodQuantity += quantity;
                break;
            case RessourceType.STONE:
                stoneQuantity += quantity;
                break;
            case RessourceType.CRYSTAL:
                crystalQuantity += quantity;
                break;
            default:
                break;
        }
    }
}
