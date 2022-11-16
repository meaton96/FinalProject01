using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected GameObject preFab;       //item model
    [SerializeField] private float weight;
    [SerializeField] private float cost;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Instantiate the item at the given location to "drop" from an enemy or chest 
    public virtual Item Drop(Vector2 dropLocation, Vector2 dropDirection) {
        GameObject item = Instantiate(preFab, dropLocation, Quaternion.identity);
        item.GetComponent<Rigidbody2D>().AddForce(dropDirection - dropLocation);
        gameObject.SetActive(true);

        GameObject[] enemies = GameObject.FindWithTag("GameControl").GetComponent<GameController>().GetEnemies();

        for (int x = 0; x < enemies.Length; x++) {
            if (enemies[x] != null)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemies[x].GetComponent<Collider2D>());
        }
        return this;
    }
    //Player picks up the item so destroy it and return
    public virtual Item PickUp() {
        Destroy(gameObject);
        return this;
    }
    
}
