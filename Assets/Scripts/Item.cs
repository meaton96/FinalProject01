using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected GameObject preFab;
    [SerializeField] private float weight;
    [SerializeField] private float cost;
    [SerializeField] private float width;
    [SerializeField] private float height;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual Item Drop(Vector2 dropLocation) {
        Instantiate(preFab, dropLocation, Quaternion.identity);
        return this;
    }
    public virtual Item PickUp() {
        Destroy(gameObject);
        return this;
    }
    public GameObject GetPreFab() { return preFab; }
}
