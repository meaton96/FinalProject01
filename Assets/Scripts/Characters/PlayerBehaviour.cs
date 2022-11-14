using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// remove this namespace bracketing


public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private bool facingRight;                          //if the player is facing right or left
    [SerializeField] private float rollSpeed;                           //the speed the player moves when rolling
    private float rollSpeedCounter;                                     //float to count the duration of the player roll
    [SerializeField] private float rollSpeedReduction;                  //how fast the roll speed decreases over time
    private Vector3 rollDir;                                            //vector diretion for the player roll
    private List<Item> backpack;                                        //list of items in the player's backpack

    public enum Damage { Half, Full }                                   //enum to represent a full heart damage or half heart damage event
    //string constants for player animations
    private const string END_GAME_SCENE = "End Game";                   
    private const string ANIM_ATTACK_TAG = "Attack";
    private const string ANIM_SHOOT_TAG = "RangedAttack";
    private const string ANIM_ROLL_TAG = "Roll";
    private const string ANIM_DEATH_TAG = "Death";
    private const string ANIM_CELEBRATE_TAG = "Celebrate";
    private const string ANIM_SPEED_TAG = "Speed";

    //enum representing if the player is rolling or not
    private enum State { Normal, Roll }
    private State state;

    public int health_current = 10;
    public int health_max = 10;
    public InterfaceScript interfaceScript;         //reference to the User Interface to update it
    public float speed;                             //player movement speed
    public float playerKnockbackOnEnemyCollision;   //player knockback amount when colliding with an enemy (unused)
    public int numArrowsFired;                      //number of arrows fired when shooting bow (unused)
    public int melee_damage;                        //amount of damage that the player does when using melee weapon (unused)
    public int ranged_damage;                       //amount of damage the player does when firing a ranged weapon (unused)
    public GameObject arrowGameObject;              //reference to arrow prefab
    private int numCoins { get; set; }              //numnber of coins the player has
    Rigidbody2D rb;
    Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        backpack = new();
        animator = GetComponent<Animator>();
        state = State.Normal;

    }

    // Update is called once per frame
    void Update() {
        //if in normal state then handle movement and animation
        //also check for player death
        switch (state) {
            case State.Normal:
                HandleMovement();
                Animate();
                if (health_current <= 0)
                    animator.SetTrigger(ANIM_DEATH_TAG);
                break;
            case State.Roll:        //if rolling just handle the roll
                HandleRoll();
                break;
        }

    }
    //handle player movement by accepting keyboard input
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

        //flip the sprite when changing directions
        if (moveX < 0 && facingRight)
            FlipSprite();
        if (moveX > 0 && !facingRight)
            FlipSprite();

        //set position and velocity
        rb.position += movement;
        rb.velocity = movement;
        //tell the animator to play walking animation 
        if (movement.magnitude > 0)
            animator.SetFloat(ANIM_SPEED_TAG, 1);
        else
            animator.SetFloat(ANIM_SPEED_TAG, 0);
    }
    //start the player roll
    //set the state and set rollDir vector 
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
        rollSpeedCounter = rollSpeed;   //reset the rollspeed counter to the roll speed
    }
    //update player position and adjust the roll counter over time to tell when its time to stop the roll animation
    private void HandleRoll() {
        if (facingRight)
            transform.position += rollSpeed * Time.deltaTime * rollDir;
        else
            transform.position -= rollSpeed * Time.deltaTime * rollDir;
        rollSpeedCounter -= rollSpeedCounter * Time.deltaTime * rollSpeedReduction;

        //roll is over reset state to normal ending the roll
        if (rollSpeedCounter <= 1)
            state = State.Normal;
    }
    //set animation tags based on player input, rolling or attacking
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
    //Not Yet Implemented shotting arrow method
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
                interfaceScript.UpdateCoinText(numCoins);
            }
            else
                backpack.Add(collision.gameObject.GetComponent<Item>().PickUp());   //might be null??
            
        }
    }

}
