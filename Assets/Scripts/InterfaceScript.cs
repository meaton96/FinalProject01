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

    public PlayerBehaviour player;                                              //player pointer
    public GameObject heartFullPrefab, heartHalfPrefab, heartEmptyPrefab;       //prefab objects for drawing the health hearts
    public float heartDistance;                                                 //distance between heart objects on UI
    public GameObject[] hearts;                                                 //array of currently drawn heart objects
    private int maxHearts, curHeart;                                            //the maximum number of heart and current heart index
    public Vector2 heartStartLoc;                                               //place on the screen to begin drawing the hearts
    [SerializeField] private TextMeshProUGUI coinText;                          //coin text object to display number of coins
    [SerializeField] private TextMeshProUGUI enemyRemainingText;                //text to display number of enemies left
    [SerializeField] private TextMeshProUGUI spawnersReaminingText;

    // Start is called before the first frame update
    void Start() {
        AdjustMaximumHearts();
    }

    // Update is called once per frame
    void Update() {
        
    }
    public void UpdateEnemiesRemaining(int numEnemies) {
        enemyRemainingText.SetText("Remaining " + numEnemies);
    }
    //create, instantiate, and fill the heart array with full hearts, also attack it to the main camera 
    private void InstantiateHearts() {
        for (int x = 0; x < maxHearts; x++) {
            //fill the heart array with full heart objects
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
        maxHearts = player.health_max / 2;
        curHeart = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
    }
    public void AddHalfHeart() {
        //if hearts are all full do nothing
        if (curHeart == maxHearts - 1 && hearts[curHeart].CompareTag(HEART_FULL_TAG))
            return;
        //grab reference to current heart
        GameObject currentHeart = hearts[curHeart];
        //if it is an empty heart make it a half full heart
        if (currentHeart.CompareTag(HEART_EMPTY_TAG)) {
            hearts[curHeart] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[curHeart].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        }
        else {
            //otherwise make it a full heart
            hearts[curHeart] = Instantiate(heartFullPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[curHeart].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
            //if current heart counter isnt at the end of the hearts array then increment
            if (curHeart != maxHearts - 1)
                curHeart++;
        }
        Destroy(currentHeart);  //destroy the previous heart
    }
    public void RemoveHalfHeart() {
        //grab reference to the current heart
        GameObject currentHeart = hearts[curHeart];
        if (currentHeart.CompareTag(HEART_FULL_TAG)) {
            //replace it with a half heart if it was full
            hearts[curHeart] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[curHeart].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        }
        else {
            //otherwise replace it with an empty heart
            hearts[curHeart] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[curHeart].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
            if (curHeart != 0)
                curHeart--;
        }
        Destroy(currentHeart);
    }
    //update coin text by pulling amount of coins from player
    public void UpdateCoinText(int numCoins) {
        coinText.text = numCoins.ToString();
    }
    public void UpdateSpawnerText(int numSpawners) {
        spawnersReaminingText.text = "Spawners: " + numSpawners;
    }


}
