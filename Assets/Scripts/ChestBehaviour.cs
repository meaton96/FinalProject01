using Pathfinding.Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour {
    private Queue<Item> drops;
    [SerializeField] private Sprite openSprite, closeSprite;
    private bool hasBeenOpened = false;
    public enum State {
        closed, open
    }
    private State state;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision) {
        if (state == State.closed)
            Open();
        else
            Close();
    }
    private void Close() {
        state = State.closed;
        GetComponent<SpriteRenderer>().sprite = closeSprite;
    }
    public void Init(int numItems) {
        state = State.closed;
        drops = MakeDrops(numItems);
    }
    public bool CanSpawnEnemy() {
        return !hasBeenOpened;
    }
    //create and populate the stack of drops
    //randomly assigns a coin or heart currently
    protected Queue<Item> MakeDrops(int numItemsDropped) {
        Queue<Item> drops = new();
        //placeholder 
        for (int x = 0; x < numItemsDropped; x++) {
            if (Random.Range(0, 1f) > .5f) {
                drops.Enqueue(gameObject.AddComponent<CoinBehaviour>());
            }
            else
                drops.Enqueue(gameObject.AddComponent<HeartBehaviour>());
        }
        return drops;
    }
    public void Open() {
        hasBeenOpened = true;
        state = State.open;
        GetComponent<SpriteRenderer>().sprite = openSprite;
        while (drops.TryDequeue(out Item i)) {
            float theta = Random.Range(0, 2 * Mathf.PI);
            Vector2 dropDir = new(transform.position.x + 2 * Mathf.Cos(theta),
                transform.position.y + 2 * Mathf.Sin(theta));
            i.Drop(transform.position, dropDir);
        }
    }
}
