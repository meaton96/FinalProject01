using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceScript : MonoBehaviour {

    private const string HEART_FULL_TAG = "Interface_HeartFull";
    private const string HEART_HALF_TAG = "Interface_HeartHalf";
    private const string HEART_EMPTY_TAG = "Interface_HeartEmpty";

    public PlayerBehaviour player;
    public GameObject heartFullPrefab, heartHalfPrefab, heartEmptyPrefab;
    public float heartDistance;
    public GameObject[] hearts;
    private double maxHearts, curHearts;
    public Vector2 heartStartLoc;
    // Start is called before the first frame update
    void Start() {
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
    }

    // Update is called once per frame
    void Update() {

    }
    private void InstantiateHearts() {
        for (int x = 0; x < maxHearts; x++) {
            hearts[x] = Instantiate(heartFullPrefab,
                new Vector3(heartStartLoc.x + (x * heartDistance), heartStartLoc.y, 0),
                Quaternion.identity);
        }
    }
    public void RemoveHeartHalf() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
        }
        else {
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);

        }
        Destroy(currentHeart);
    }
    public void RemoveFullHeart() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
            Destroy(currentHeart);
            currentHeart = hearts[(int)curHearts];
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
        }
        else {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
            curHearts--;
        }

    }


}
