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
       // Vector3 lerpPosition = Vector3.Lerp(transform.position, direction, speed * Time.deltaTime);
       // transform.position = lerpPosition;

        transform.Translate(direction.normalized * (speed * Time.deltaTime));
    }
    public void SetDirection(Vector2 dir) { direction = dir; }
    public void SetSpeed(float speed) { this.speed = speed; }
    public void SetCollisionIgnores() {
        GameObject[] itemObject = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] enemyObject = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] borderObject = GameObject.FindGameObjectsWithTag("BorderRocks");
        if (borderObject != null) {
            for (int x = 0; x < itemObject.Length; x++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                itemObject[x].GetComponentInChildren<Collider2D>());
            }
        }
        if (borderObject != null) {
            for (int x = 0; x < enemyObject.Length; x++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                enemyObject[x].GetComponentInChildren<Collider2D>());
            }
        }
        if (borderObject != null) {
            for (int x = 0; x < borderObject.Length; x++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),
                borderObject[x].GetComponentInChildren<Collider2D>());
            }
        }

    }
}
