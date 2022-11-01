using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceScript : MonoBehaviour
{
    public PlayerBehaviour player;
    public GameObject heartFullPrefab, heartHalfPrefab, heartEmptyPrefab;

    private double maxHearts, curHearts;
    public Vector2 heartStartLoc;
    // Start is called before the first frame update
    void Start()
    {
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts;
        InstantiateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InstantiateHearts() {
        for (int x = 0; x < maxHearts; x++) {
            Instantiate(heartFullPrefab,
                new Vector3(heartStartLoc.x + (x * .2f), heartStartLoc.y, 0),
                Quaternion.identity);
        }
    }
    
}
