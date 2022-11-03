using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// remove this namespace bracketing


public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private bool facingRight;

    public enum Damage { Half, Full }

    float horizontalValue;
    float verticalValue;
    public int health_current = 10;
    public int health_max = 10;
    public InterfaceScript interfaceScript;

    public float speed;
    public float playerKnockbackOnEnemyCollision;
    Rigidbody2D rb;

    // reference var for our Animator Component
    Animator animator;

    private void Start() {
        // gets reference to Rigidbody2D on same GameObject
        rb = GetComponent<Rigidbody2D>();

        // get reference to Animator on the same GameObject
        animator = GetComponent<Animator>();

    }
    private

    // Update is called once per frame
    void Update() {
        CheckAxes();
        Animate();
        FlipSprite();
        if (health_current <= 0)
            animator.SetTrigger("Death");

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
    private Vector2 GetVectorToEnemy(GameObject enemy) {
        return (enemy.GetComponent<Rigidbody2D>().position - rb.position).normalized;
    }
    public void PlayerDeath() {
        Destroy(gameObject);
    }
    public void DamagePlayerHealth(Damage dam) {
        if (dam == Damage.Half) {
            interfaceScript.RemoveHeartHalf();
        }
        else
            interfaceScript.RemoveFullHeart();
        health_current--;
    }

    
}
