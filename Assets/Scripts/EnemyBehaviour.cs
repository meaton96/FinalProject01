using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private Stack<Item> drops;  //stack for item drops when enemy dies
    private int numItemsDropped; //number of items the enemy drops on death
    private float itemSpawnRange;   //distance for spawning items in a circle around the enemy location

    [SerializeField] protected float Health;
    //public GameObject enemy;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        numItemsDropped = Random.Range(1, 5);           //random number of drops 1-5 to be replaced by different number per enemy 
        itemSpawnRange = .2f;                           //radius of item spawn circle
        Rb = GetComponent<Rigidbody2D>();
        GetAnimator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
        MakeDrops();
        SetCollisionIgnores();

    }
    //ignore collisions with items
    protected void SetCollisionIgnores() {
        GameObject itemObject = GameObject.FindWithTag("Item");
        if (itemObject != null)
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                GameObject.FindWithTag("Item").GetComponent<Collider2D>());
    }
    //create and populate the stack of drops
    //randomly assigns a coin or heart currently
    protected void MakeDrops() {
        drops = new Stack<Item>();
        //placeholder 
        for (int x = 0; x < numItemsDropped; x++) {
            if (Random.Range(0, 1f) > .5f) {
                drops.Push(gameObject.AddComponent<CoinBehaviour>());
            }
            else
                drops.Push(gameObject.AddComponent<HeartBehaviour>());
        }

    }

    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    protected Animator GetAnimator { get { return animator; } set { animator = value; } }
    protected GameObject Player { get { return player; } set { player = value; } }

    // Update is called once per frame
    void Update()
    {
        
    }
    //called to damage the player when the enemy collides with them
    protected void KnockedByPlayer(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {

            //collided with player
            player.GetComponent<PlayerBehaviour>().DamagePlayerHealth(PlayerBehaviour.Damage.Half);
        }
    }
    public virtual void AttackPlayer() {
        
    }
    //sets velocity to move towards player at the passed in value of movement speed
    protected void SetVelocity(float movementSpeed) {
        rb.velocity = GetVectorToPlayer().normalized * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
    }
    //returns a Vector2 that represents the vector from this enemy to the player
    protected Vector2 GetVectorToPlayer() {
        if (player != null)
            return (player.GetComponent<Rigidbody2D>().position - rb.position);
        else return Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player_Projectile")) {
            Health--;
        }
    }
    //damage the enemy, dies if its health goes below 0
    public void Damage(float damage) {
        Health -= damage;
        if (Health <= 0)
            Die();

    }
    //destroys the game object killing the enemy
    //pops the stack of drops and spawns them 
    private void Die() {
        while (drops.TryPop(out Item i)) {
            float theta = Random.Range(0, 2 * Mathf.PI);
            Vector2 dropPos = new(transform.position.x + itemSpawnRange * Mathf.Cos(theta),
                transform.position.y + itemSpawnRange * Mathf.Sin(theta));
            i.Drop(dropPos);
        }
        Destroy(gameObject);
    }

}
