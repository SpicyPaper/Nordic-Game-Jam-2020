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
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;
    private bool isAbleToCollect;
    private bool isRessourceCurrentlyCollected;
    private RessourceProducerType currentProducerType;
    private GameObject currentProducer;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (RessourceProducerContains(collision.tag))
        {
            isAbleToCollect = true;

            currentProducerType = GetRessourceProducerType(collision.tag);
            currentProducer = collision.gameObject;

            Debug.Log("Press E to collect ressource");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (RessourceProducerContains(collision.tag))
        {
            isAbleToCollect = false;
        }
    }

    private void Update()
    {
        if (isAbleToCollect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //TODO : have to be placed to false if the user move
                isRessourceCurrentlyCollected = true;
                CollectRessource(currentProducerType);
                currentProducer.transform.parent.localScale = new Vector3(0.5f, 0.5f);
            }

        }
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

    private void CollectRessource(RessourceProducerType type)
    {
        switch (type)
        {
            case RessourceProducerType.TREE:
                Debug.Log("5 more wood collected");
                break;
            case RessourceProducerType.BUSH:
                Debug.Log("3 more wood collected");
                break;
            case RessourceProducerType.LITTLE_BUSH:
                Debug.Log("1 more wood collected");
                break;
            case RessourceProducerType.STONE:
                Debug.Log("5 more stone collected");
                break;
            case RessourceProducerType.LITTLE_STONE:
                Debug.Log("3 more stone collected");
                break;
            case RessourceProducerType.CRYSTAL:
                Debug.Log("5 more crystal collected");
                break;
            case RessourceProducerType.LITTLE_CRYSTAL:
                Debug.Log("3 more crystal collected");
                break;
        }
    }
}
