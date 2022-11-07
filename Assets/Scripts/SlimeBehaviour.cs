using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float movementSpeed;
    [SerializeField] float attackRange;
    
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        GetAnimator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
        MakeDrops();
        SetCollisionIgnores();

    }
    void OnCollisionEnter2D(Collision2D collision) {
        KnockedByPlayer(collision);
    }

    // Update is called once per frame
    void Update()
    {
        SetVelocity(movementSpeed);
        if (GetVectorToPlayer().magnitude <= attackRange) {
            GetAnimator.SetTrigger("Attack");
            AttackPlayer();
        }
    }

    
}
