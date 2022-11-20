using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
using UnityEngine;

public class MagicianBehaviour : EnemyBehaviour
{
    [SerializeField] float projectileSpeed;
    private Animator magicianAnimator;
    private const float AttackDelayTime = 1f;
    [SerializeField] private float AttackDelayCounter = 0;
    private int projectileDamage;

    private const string DEATH_TRIGGER = "Death";
    private const string ATTACK_TRIGGER = "attack";
    private const string THROW_TRIGGER = "throw";
    private const string HURT_TRIGGER = "Hurt";
    private const string SPEED_TRIGGER = "Speed";


    protected override void Start() {
        movementSpeed = .6f;
        attackRange = 1.5f;
        aggroRange = 2f;
        projectileDamage = 2;
        magicianAnimator = GetComponent<Animator>();
        base.Start();
    }
    public override void AttackPlayer() {
        if (GetVectorToPlayer().magnitude > attackRange)
            state = State.Aggroed;
        if (AttackDelayCounter >= 0) {
            AttackDelayCounter -= Time.deltaTime;
        }
        else {
            magicianAnimator.SetTrigger(THROW_TRIGGER);
            AttackDelayCounter = AttackDelayTime;
        }
        
    }
    public void Shoot() {
        GameObject ball = Instantiate(GameObject.FindWithTag("GameControl")
            .GetComponent<GameController>().MagicianBallPreFab, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ball.GetComponent<Collider2D>());
        Projectile projScript = ball.GetComponent<Projectile>();
        //projScript.SetCollisionIgnores();
        //Debug.Log(projScript.gameObject.transform.position);    
        //Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        projScript.SetDirection(GetVectorToPlayer().normalized);
        projScript.SetSpeed(projectileSpeed);
        
        projScript.Damage = projectileDamage;
    }

}
