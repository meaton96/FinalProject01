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
    public float KnockBackAmount;
    private Stack<Item> drops;                           
    [SerializeField] private int numItemsDropped;
    [SerializeField] private float itemSpawnRange;

    [SerializeField] protected float Health;
    //public GameObject enemy;
    // Start is called before the first frame update
    protected void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        GetAnimator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
        MakeDrops();
        SetCollisionIgnores();

    }
    protected void SetCollisionIgnores() {
        GameObject itemObject = GameObject.FindWithTag("Item");
        if (itemObject != null)
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                GameObject.FindWithTag("Item").GetComponent<Collider2D>());
    }
    
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
        foreach (Item item in drops) {
            //item.gameObject.SetActive(false);
        }

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
        if (player != null)
            return (player.GetComponent<Rigidbody2D>().position - rb.position);
        else return Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player_Projectile")) {
            Health--;
        }
    }
    public void Damage(float damage) {
        Health -= damage;
        if (Health <= 0)
            Die();

    }
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
