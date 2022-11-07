using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// remove this namespace bracketing


public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private bool facingRight;
    [SerializeField] private float rollSpeed;
    private float rollSpeedCounter;
    [SerializeField] private float rollSpeedReduction;
    private Vector3 rollDir;
    private List<Item> backpack;

    public enum Damage { Half, Full }
    private const string END_GAME_SCENE = "End Game";
    private const string ANIM_ATTACK_TAG = "Attack";
    private const string ANIM_SHOOT_TAG = "RangedAttack";
    private const string ANIM_ROLL_TAG = "Roll";
    private const string ANIM_DEATH_TAG = "Death";
    private const string ANIM_CELEBRATE_TAG = "Celebrate";
    private const string ANIM_SPEED_TAG = "Speed";

    private enum State {
        Normal,
        Roll
    }
    private State state;

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
    private int numCoins { get; set; }
    Rigidbody2D rb;

    // reference var for our Animator Component
    Animator animator;

    private void Start() {
        // gets reference to Rigidbody2D on same GameObject
        rb = GetComponent<Rigidbody2D>();
        backpack = new();

        // get reference to Animator on the same GameObject
        animator = GetComponent<Animator>();
        state = State.Normal;

    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case State.Normal:
                //CheckAxes();
                HandleMovement();
                Animate();
                //FlipSprite();
                if (health_current <= 0)
                    animator.SetTrigger(ANIM_DEATH_TAG);
                break;
            case State.Roll:
                HandleRoll();
                break;
        }

    }
    public void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)) {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = 1f;
        }
        Vector2 movement = speed * Time.deltaTime * new Vector2(moveX, moveY);

        if (moveX < 0 && facingRight)
            FlipSprite();
        if (moveX > 0 && !facingRight)
            FlipSprite();

        rb.position += movement;
        rb.velocity = movement;
        if (movement.magnitude > 0)
            animator.SetFloat(ANIM_SPEED_TAG, 1);
        else
            animator.SetFloat(ANIM_SPEED_TAG, 0);
    }
    private bool TryMove(Vector3 moveDir, float speed) {
        Vector3 move = moveDir;
        bool canMove = CanMove(moveDir, speed);
        if (!canMove) {
            move = new Vector3(moveDir.x, 0f).normalized;
            canMove = move.x != 0f && CanMove(move, speed);
            if (!canMove) {
                move = new Vector3(0f, moveDir.y).normalized;
                canMove = moveDir.y != 0f && CanMove(move, speed);
            }
        }
        if (canMove) {
            transform.position += move * speed;
            return true;
        }
        else
            return false;
    }
    private bool CanMove(Vector3 moveDir, float distance) {
        return Physics2D.Raycast(transform.position, moveDir, distance).collider == null;
    }
    private void StartRoll() {
        state = State.Roll;
        if (rb.velocity.magnitude < .05f) {
            rollDir = Vector2.right;
        }
        else {
            if (facingRight)
                rollDir = rb.velocity.normalized;
            else
                rollDir = rb.velocity.normalized * -1;
        }
        // rollDir = (Input.mousePosition - transform.position).normalized;
        //Debug.Log("(" + rollDir.x + "," + rollDir.y + ")");
        rollSpeedCounter = rollSpeed;
    }
    private void HandleRoll() {
        if (facingRight)
            transform.position += rollSpeed * Time.deltaTime * rollDir;
        else
            transform.position -= rollSpeed * Time.deltaTime * rollDir;
        rollSpeedCounter -= rollSpeedCounter * Time.deltaTime * rollSpeedReduction;

        if (rollSpeedCounter <= 1)
            state = State.Normal;
    }

    void FixedUpdate() {
        // call physics-related methods like setvelocity in FixedUpdate

        // Use either Set Velocity or ForceMove. Not both. They are different movement ideas

        //SetVelocity();

        //ForceMove();
    }


    void Animate() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger(ANIM_ATTACK_TAG);
        }
        //else if (Input.GetMouseButtonDown(1)) {
            //animator.SetTrigger(ANIM_SHOOT_TAG);
        //}
        else if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger(ANIM_ROLL_TAG);
            StartRoll();
        }

    }

    private void ShootArrow() {
        Vector3 playerPos = transform.position;
        Vector3 mousePos = Input.mousePosition;

        Stack<GameObject> arrows = new();
        for (int x = 0; x < numArrowsFired; x++) {
            //figure out arrows
        }
        //figure out arrow direction =/
        Quaternion q = new Quaternion();
        Vector3 a = Vector3.Cross(playerPos, mousePos);
        q.Set(a.x, a.y, a.z,
            Mathf.Sqrt(
                (Mathf.Pow(playerPos.magnitude, 2) *
                 Mathf.Pow(Input.mousePosition.magnitude, 2))) +
                 Vector3.Dot(playerPos, mousePos));

        arrows.Push(Instantiate(arrowGameObject, playerPos, q));
        arrows.Pop().gameObject.GetComponent<ArrowBehaviour>().SetDirection((playerPos - mousePos).normalized, 0);

    }

    void FlipSprite() {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Item")) {
            if (collision.gameObject.GetComponent<HeartBehaviour>() != null) {
                int healthAmount = (int)collision.gameObject.GetComponent<HeartBehaviour>().HealAmount;
                health_current += healthAmount;
                collision.gameObject.GetComponent<HeartBehaviour>().PickUp();
                if (health_current > health_max)
                    health_current = health_max;
                interfaceScript.AdjustHeartMultipleTimes(interfaceScript.AddHalfHeart, healthAmount);
                
            }
            else if (collision.gameObject.GetComponent<CoinBehaviour>() != null) {
                numCoins += ((CoinBehaviour) collision.gameObject.GetComponent<CoinBehaviour>().PickUp()).Value;
            }
            else
                backpack.Add(collision.gameObject.GetComponent<Item>().PickUp());   //might be null??
            
        }
    }

}
