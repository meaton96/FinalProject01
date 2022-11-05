using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float speed;
    public int pierceNunber;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            pierceNunber--;
        }
        if (pierceNunber <= 0)
            Destroy(gameObject);


    }
    public void SetDirection(Vector2 dir, int pierceNum) {
        pierceNunber = pierceNum;

        gameObject.GetComponent<Rigidbody2D>().velocity = dir * speed;

    }
}
