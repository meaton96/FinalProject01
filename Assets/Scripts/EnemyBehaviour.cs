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
    private List<Item> drops;
    [SerializeField] private int numItemsDropped;


    [SerializeField] protected float Health;
    //public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        MakeDrops();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        
    }
    protected void SetCollisionIgnores() {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
            GameObject.FindWithTag("Item").GetComponent<Collider2D>());
    }
    
    protected void MakeDrops() {
        drops = new List<Item>();
        //placeholder 
        for (int x = 0; x < numItemsDropped; x++) {
            if (Random.Range(0, 1) > .5f) {
                drops.Add(gameObject.AddComponent<CoinBehaviour>());
            }
            else
                drops.Add(gameObject.AddComponent<HeartBehaviour>());
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
        foreach(Item i in drops) {
            i.Drop(transform.position).gameObject.SetActive(true);
            //Instantiate(i.GetPreFab(), transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
