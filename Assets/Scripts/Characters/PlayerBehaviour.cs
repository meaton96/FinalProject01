using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// remove this namespace bracketing


public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private bool facingRight;

    public enum Damage { Half, Full }
    private const string END_GAME_SCENE = "End Game";
    private const string ANIM_ATTACK_TAG = "Attack";
    private const string ANIM_SHOOT_TAG = "RangedAttack";
    private const string ANIM_ROLL_TAG = "Roll";
    private const string ANIM_DEATH_TAG = "Death";
    private const string ANIM_CELEBRATE_TAG = "Celebrate";
    private const string ANIM_SPEED_TAG = "Speed";

    float horizontalValue;
    float verticalValue;
    public int health_current = 10;
    public int health_max = 10;
    public InterfaceScript interfaceScript;
    public float speed;
    public float playerKnockbackOnEnemyCollision;
    public int numArrowsFired;
    public int melee_damage;
    public int ranged_damage;
    public GameObject arrowGameObject;
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
            animator.SetTrigger(ANIM_DEATH_TAG);

    }

    void FixedUpdate() {
        // call physics-related methods like setvelocity in FixedUpdate

        // Use either Set Velocity or ForceMove. Not both. They are different movement ideas

        SetVelocity();

        //ForceMove();
    }


    void Animate() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger(ANIM_ATTACK_TAG);
        }
        else if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger(ANIM_SHOOT_TAG);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger(ANIM_ROLL_TAG);
        }

    }
    public void RollSpeedUp() {
        speed *= 2;
    }
    public void RollSpeedDown() {
        speed /= 2;
    }
    private void ShootArrow() {
        float angle = 0;


        Stack<GameObject> arrows = new();
        for (int x = 0; x < numArrowsFired; x++) {
            //figure out arrows
        }
        arrows.Push(Instantiate(arrowGameObject, transform.position, new Quaternion(1,1,0,0)));
        arrows.Pop().gameObject.GetComponent<ArrowBehaviour>().SetDirection(angle, 0);

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
        animator.SetFloat(ANIM_SPEED_TAG, Mathf.Abs(horizontalValue));
    }
    private Vector2 GetVectorToEnemy(GameObject enemy) {
        return (enemy.GetComponent<Rigidbody2D>().position - rb.position).normalized;
    }
    public void PlayerDeath() {
        Destroy(gameObject);
        SceneManager.LoadScene(END_GAME_SCENE, LoadSceneMode.Single);
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
