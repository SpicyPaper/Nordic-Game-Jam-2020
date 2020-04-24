using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterMovement : MonoBehaviour
{
    private AIPath aiPath;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        aiPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", aiPath.desiredVelocity.x);
        animator.SetFloat("Vertical", aiPath.desiredVelocity.y);
        animator.SetFloat("Speed", aiPath.desiredVelocity.sqrMagnitude);
    }
    
}
