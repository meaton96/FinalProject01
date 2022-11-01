using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// remove this namespace bracketing


public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private bool facingRight;

    float horizontalValue;
    float verticalValue;

    public float speed;

    Rigidbody2D rb;

    // reference var for our Animator Component
    Animator animator;

    private void Start() {
        // gets reference to Rigidbody2D on same GameObject
        rb = GetComponent<Rigidbody2D>();

        // get reference to Animator on the same GameObject
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        CheckAxes();
        Animate();
        FlipSprite();

    }

    void FixedUpdate() {
        // call physics-related methods like setvelocity in FixedUpdate

        // Use either Set Velocity or ForceMove. Not both. They are different movement ideas

        SetVelocity();

        //ForceMove();
    }
    
    void Animate() {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger("Attack");
        }
        
    }

    void CheckAxes() {
        horizontalValue = Input.GetAxis("Horizontal") * speed;
        verticalValue = Input.GetAxis("Vertical") * speed;
    }

    void FlipSprite() {
        if (horizontalValue < 0 && facingRight == true) {
            transform.Rotate(0, 180, 0);
            facingRight = false;
        }
        else if (horizontalValue > 0 && facingRight == false) {
            transform.Rotate(0, 180, 0);
            facingRight = true;
        }
    }

    void SetVelocity() {
        // assigns value to our rigidbody's velocity
        rb.velocity = new Vector2(horizontalValue, verticalValue);
        animator.SetFloat("Speed", Mathf.Abs(horizontalValue));
    }
}
