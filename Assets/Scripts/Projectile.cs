using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private float time = 1;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y > 20 || transform.position.y < -20)
            Destroy(gameObject);
            
    }
    private void FixedUpdate() {
        // Vector3 lerpPosition = Vector3.Lerp(transform.position, direction, speed * Time.deltaTime);
        // transform.position = lerpPosition;

        transform.Translate(direction.normalized * (speed * Time.deltaTime));
        if (timer > time) {
            timer = 0;
            LogInfo();
        }
        else
            timer += Time.deltaTime;
    }
    private void LogInfo() {
        Debug.Log($"Pos: ({transform.position.x},{transform.position.y})");
        //Debug.Log($"Velocity");
    }
    private void OnCollisionEnter2D(Collision2D collision) {
      /*  Destroy(gameObject);
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerBehaviour>().DamagePlayerHealth(damage);
        }*/
    }
    public void SetDirection(Vector2 dir) { direction = dir; }
    public void SetSpeed(float speed) { this.speed = speed; }
    //tell the projectile to ignore all enemies and items
    public void SetCollisionIgnores() {
        GameObject[] itemObject = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] enemyObject = GameObject.FindGameObjectsWithTag("Enemy");
        if (itemObject != null) {
            for (int x = 0; x < itemObject.Length; x++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                itemObject[x].GetComponentInChildren<Collider2D>());
            }
        }
        if (enemyObject != null) {
            for (int x = 0; x < enemyObject.Length; x++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                enemyObject[x].GetComponentInChildren<Collider2D>());
            }
        }

    }
    public int Damage { set { damage = value; } } 
}
