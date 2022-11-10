using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBehaviour : EnemyBehaviour
{
    private Vector2 vectorToPlayer = new();
    [SerializeField] float projectileSpeed;
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AttackPlayer() {
        GameObject ball = Instantiate(GameObject.FindWithTag("GameControl").GetComponent<GameController>().MagicianBallPreFab, transform.position, Quaternion.identity);
        Projectile projScript = ball.GetComponent<Projectile>();
        projScript.SetDirection(GameObject.FindWithTag("Player").transform.position);
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
