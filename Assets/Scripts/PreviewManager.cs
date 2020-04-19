using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public static bool IsCraftingAllowed
    {
        get
        {
            return numberOfCollision == 0;
        }
    }

    private static int numberOfCollision = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CraftAvoider")
        {
            print("+ " + numberOfCollision);
            numberOfCollision++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CraftAvoider")
        {
            print("- " + numberOfCollision);
            numberOfCollision--;
        }
    }
}
