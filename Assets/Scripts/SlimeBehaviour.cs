using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float movementSpeed;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = GetVectorToPlayer();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
    }

    
}
