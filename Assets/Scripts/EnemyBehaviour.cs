using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private Stack<Item> drops;                      //stack for item drops when enemy dies
    private int numItemsDropped;                    //number of items the enemy drops on death
    private float itemSpawnRange;                   //distance for spawning items in a circle around the enemy location
    private float wanderDirectionChangeCounter;     //counter to time duration of wander direction
    private const int DAMAGE_ON_COLLISION = 1;      //amount of damage inflected on the player when running into them baseline
                                                    //1 = 1/2 a heart
    [SerializeField] private float wanderSpeedMultiplier;       //how slower/faster the enemy moves while wandering vs aggroed
    [SerializeField] private float wanderDirectionChangeTime;   //the time set during the wandering movement before changing directions
                                                                //in seconds
    [SerializeField] protected float movementSpeed;             //the base movement speed of the enemy
    [SerializeField] protected float attackRange;               //the range the enemy must get to before attacking the player
    [SerializeField] protected float aggroRange;                //the range the enmy must get to before starting to move toward the player
    [SerializeField] protected float Health;
    public enum State {         //state for storing the current enemy actions
        Attacking,              //actively attempting to attack the player
        Aggroed,                //the player is in range to move towards
        Dormant,                //do nothing
        Wandering               //move around in a random direction and change very X seconds
    }
    protected State state;


    // Start is called before the first frame update
    protected virtual void Start() {
        Init();

    }
    public void Init() {
        wanderSpeedMultiplier = .5f;                    //default wandering speed is 1/2 regular movement speed
        wanderDirectionChangeTime = 10f;                //set change direction time to 10 seconds
        numItemsDropped = Random.Range(1, 5);           //random number of drops 1-5 to be replaced by different number per enemy 
        itemSpawnRange = .2f;                           //radius of item spawn circle
        wanderDirectionChangeCounter = 0;
        Rb = GetComponent<Rigidbody2D>();
        GetAnimator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
        state = State.Wandering;                        //set default state
        
        MakeDrops();
     //   SetCollisionIgnores();
    }
    //ignore collisions with items
    protected void SetCollisionIgnores() {
        GameObject[] itemObject = GameObject.FindGameObjectsWithTag("Item");
        //if (GetComponent<Collider2D>() != null && itemObject != null  && itemObject.GetComponent<Collider2D>() != null)

      //  Debug.Log(itemObject.Length);
      //  Debug.Log(itemObject[0].GetComponent<Collider2D>() == null);
      //  Debug.Log(GetComponent<Collider2D>() == null);

        for (int x = 0; x < itemObject.Length; x++) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
                itemObject[x].GetComponent<Collider2D>());
        }
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
    protected virtual void Update() {
        switch (state) {
            case State.Wandering:
                Wander();
                break;
            case State.Dormant:
                break;
            case State.Attacking:
                AttackPlayer();
                break;
            case State.Aggroed:
                MoveToPlayer();
                break;
        }
    }
    private void MoveToPlayer() {
        //just sets the velocity maybe add an attack animation later
        SetVelocity(movementSpeed, GetVectorToPlayer());
        //play attack animation or wander animation if the player moved closer or father away
        if (GetVectorToPlayer().magnitude <= attackRange)
            state = State.Attacking;
        if (GetVectorToPlayer().magnitude >= aggroRange) {
            wanderDirectionChangeCounter = 0;
            state = State.Wandering;
        }

    }
    //change direction randomly every X seconds, swap to aggroed if in range
    private void Wander() {
        if (wanderDirectionChangeCounter <= 1) {
            float angle = Random.Range(0, 2 * Mathf.PI);
            SetVelocity(movementSpeed * wanderSpeedMultiplier,
                new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            wanderDirectionChangeCounter = wanderDirectionChangeTime;
        }
        else
            wanderDirectionChangeCounter -= Time.deltaTime;

        if (GetVectorToPlayer().magnitude <= aggroRange)
            state = State.Aggroed;
    }
    //implemented per enemy type
    public virtual void AttackPlayer() {}
    //sets velocity to move towards player at the passed in value of movement speed
    protected void SetVelocity(float movementSpeed, Vector2 direction) {
        if (rb != null) {
            rb.velocity = direction.normalized * movementSpeed;
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
        }
    }
    //returns a Vector2 that represents the vector from this enemy to the player
    protected Vector2 GetVectorToPlayer() {
        if (player != null && rb != null)
            return player.GetComponent<Rigidbody2D>().position - rb.position;
        else return Vector2.zero;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //collided with player
            player.GetComponent<PlayerBehaviour>().DamagePlayerHealth(DAMAGE_ON_COLLISION);
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
            Vector2 dropDir = new(transform.position.x + itemSpawnRange * Mathf.Cos(theta),
                transform.position.y + itemSpawnRange * Mathf.Sin(theta));
            i.Drop(transform.position, dropDir);
        }
        Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Damage(collision.GetComponent<WeaponHitBoxScript>().damageValue);
    }

}
