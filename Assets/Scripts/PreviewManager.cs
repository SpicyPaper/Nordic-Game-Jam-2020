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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CraftAvoider")
        {
            collidersInCollision.Remove(collision);
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
