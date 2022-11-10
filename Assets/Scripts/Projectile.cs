using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
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
        Vector3 lerpPosition = Vector3.Lerp(transform.position, direction, speed);
        transform.position = lerpPosition;
    }
    public void SetDirection(Vector2 dir) { direction = dir; }
    public void SetSpeed(float speed) { this.speed = speed; }
    public void SetCollisionIgnores() {
        GameObject itemObject = GameObject.FindWithTag("Item");
        GameObject enemyObject = GameObject.FindWithTag("Enemy");
        if (itemObject != null) {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                itemObject.GetComponent<Collider2D>());
        }
        if (enemyObject != null) {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                enemyObject.GetComponent<Collider2D>());
        }

    }
}
