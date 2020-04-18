using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holder for the health bar
/// Can call SetSize to change the percentage of the bar
/// </summary>
public class HealthBar : MonoBehaviour
{
    private Transform bar;


    // Start is called before the first frame update
    void Awake()
    {
        bar = transform.Find("Bar");
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(Math.Max(0, sizeNormalized), 1f);
    }
    
}
