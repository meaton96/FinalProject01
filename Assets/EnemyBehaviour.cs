using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    GameObject player;
    public float KnockBackAmount;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            rb.AddForce(-1 * KnockBackAmount * GetVectorToPlayer());
        }
    }
    protected Vector2 GetVectorToPlayer() {
        return (player.GetComponent<Rigidbody2D>().position - rb.position).normalized;
    }
}
