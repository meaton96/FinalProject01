using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class MagicianBehaviour : EnemyBehaviour
{
    private Vector2 vectorToPlayer = new();
    [SerializeField] float projectileSpeed;
    private Animator magicianAnimator;
    float timer = 5f;

    private const string DEATH_TRIGGER = "Death";
    private const string ATTACK_TRIGGER = "attack";
    private const string THROW_TRIGGER = "throw";
    private const string HURT_TRIGGER = "Hurt";
    private const string SPEED_TRIGGER = "Speed";


    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            magicianAnimator.SetTrigger(THROW_TRIGGER);
            timer = 5f;
        }
    }
    protected override void Start() {
        magicianAnimator = GetComponent<Animator>();
        base.Start();
    }
    public override void AttackPlayer() {
        GameObject ball = Instantiate(GameObject.FindWithTag("GameControl").GetComponent<GameController>().MagicianBallPreFab, transform.position, Quaternion.identity);
        Projectile projScript = ball.GetComponent<Projectile>();
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        projScript.SetDirection((transform.position - playerPos).normalized * -1);
        projScript.SetSpeed(projectileSpeed);
        projScript.SetCollisionIgnores();
    }
    private void Wander() {
        //wander around randomly in small area
    }
    private void AggroPlayer() {
        //notice player after *some range* and follow them until player is out of *some range* 
    }
    private void ShootAtPlayer() {
        //play the shooting atnimation if the player is close enough
    }
    private bool IsInRangeOfPlayer() {
        return false;
    }
}
