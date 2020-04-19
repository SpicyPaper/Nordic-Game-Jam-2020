using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public static bool IsCraftingAllowed
    {
        get
        {
            return collidersInCollision.Count == 0;
        }
    }

    private static List<Collider2D> collidersInCollision;

    public static GameObject CurrentCollidingItem = null;

    private void Awake()
    {
        collidersInCollision = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CraftAvoider")
        {
            collidersInCollision.Add(collision);
        }

        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "Turret" || collision.transform.parent.tag == "Trap")
            {
                print("OK");
                CurrentCollidingItem = collision.transform.parent.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CraftAvoider")
        {
            collidersInCollision.Remove(collision);
        }

        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "Turret" || collision.transform.parent.tag == "Trap")
            {
                print("OKD");
                CurrentCollidingItem = null;
            }
        }
    }

    private void Update()
    {
        for (int i = collidersInCollision.Count - 1; i >= 0; i--)
        {
            if (collidersInCollision[i] == null)
            {
                collidersInCollision.RemoveAt(i);
            }
        }
    }
}
