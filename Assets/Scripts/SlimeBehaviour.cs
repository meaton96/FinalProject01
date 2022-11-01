using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    Animator animator;
    GameObject player;
    [SerializeField] float movementSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = GetVectorToPlayer();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
    }

    private Vector2 GetVectorToPlayer() {
        return (player.GetComponent<Rigidbody2D>().position - rb.position).normalized * movementSpeed;
    }
}
