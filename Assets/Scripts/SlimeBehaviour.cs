using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float movementSpeed;
    [SerializeField] float attackRange;
    

    void OnCollisionEnter2D(Collision2D collision) {
        KnockedByPlayer(collision);
    }

    // Update is called once per frame
    void Update()
    {
        //just sets the velocity maybe add an attack animation later
        SetVelocity(movementSpeed);
        if (GetVectorToPlayer().magnitude <= attackRange) {
            GetAnimator.SetTrigger("Attack");
            AttackPlayer();
        }
    }

    
}
