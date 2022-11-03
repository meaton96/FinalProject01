using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    public float KnockBackAmount;
    //public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }
    
    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    protected Animator GetAnimator { get { return animator; } set { animator = value; } }
    protected GameObject Player { get { return player; } set { player = value; } }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void KnockedByPlayer(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            //collided with player
            player.GetComponent<PlayerBehaviour>().DamagePlayerHealth(PlayerBehaviour.Damage.Half);
        }
    }
    public virtual void AttackPlayer() {
        
    }
    protected void SetVelocity(float movementSpeed) {
        rb.velocity = GetVectorToPlayer().normalized * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
    }
    protected Vector2 GetVectorToPlayer() {

        return (player.GetComponent<Rigidbody2D>().position - rb.position);
    }
    
}
