using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceScript : MonoBehaviour {

    //strting constants for drawing hearts 
    private const string HEART_FULL_TAG = "Interface_HeartFull";
    private const string HEART_HALF_TAG = "Interface_HeartHalf";
    private const string HEART_EMPTY_TAG = "Interface_HeartEmpty";
    public delegate void AdjustHeart();

    public PlayerBehaviour player;
    public GameObject heartFullPrefab, heartHalfPrefab, heartEmptyPrefab;
    public float heartDistance;
    public GameObject[] hearts;
    private double maxHearts, curHearts;
    public Vector2 heartStartLoc;
    [SerializeField] private TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start() {
        //set max hearts at the beginning of the game based on player health
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts - 1;  //set heart counter to the end of the hearts array
        hearts = new GameObject[(int)maxHearts];        //create array of hearts and call instantiate method
        InstantiateHearts();
        
    }

    // Update is called once per frame
    void Update() {

    }
    //create, instantiate, and fill the heart array with full hearts, also attack it to the main camera 
    private void InstantiateHearts() {
        for (int x = 0; x < maxHearts; x++) {
            hearts[x] = Instantiate(heartFullPrefab,
                new Vector3(heartStartLoc.x + (x * heartDistance), heartStartLoc.y, 0),
                Quaternion.identity);
            hearts[x].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        }
    }
    //Adjust heart delegate 
    public void AdjustHearts(AdjustHeart adjustHeart) {
        adjustHeart();
    }
    //adjust the hearts a number of times
    public void AdjustHeartMultipleTimes(AdjustHeart adjustHeart, int numTimes) {
        for (int x = 0; x < numTimes; x++)
            adjustHeart();
    }
    //change the max maximum number of hearts and reinstantiate them all
    public void AdjustMaximumHearts() {
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
    }
    //add a half heart if possible
    public void AddHalfHeart() {
        GameObject currentHeart = hearts[(int)curHearts];
        if (curHearts >= hearts.Length - 1 && currentHeart.CompareTag(HEART_FULL_TAG))
            return;
        //replace half heart with full heart
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartFullPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts++;
        }
        else {
            //replace empty heart with half heart
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
        }
        //destroy the old heart
        Destroy(currentHeart);
    }
    //remove half a heart 
    public void RemoveHeartHalf() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        //replace half heart with empty heart
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
        }
        else {
            //replace full heart with half heart
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);

        }
        Destroy(currentHeart);
    }
    //remove a full heart, slightly more efficient than removing the half heart twice
    public void RemoveFullHeart() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        //empty the current half heart then replace the next heart in the line with a half heart
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
            Destroy(currentHeart);
            currentHeart = hearts[(int)curHearts];
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
        }
        //remove the current full heart and replace it with an empty heart
        else {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
            curHearts--;
        }

    }
    //update coin text by pulling amount of coins from player
    public void UpdateCoinText(int numCoins) {
        coinText.text = numCoins.ToString();
    }


}
