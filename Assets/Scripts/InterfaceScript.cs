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
    private int currentMaxHearts;
    private int playerHealthCurrent;
    public GameObject[] hearts;
    private int maxHearts, curHearts;
    public Vector2 heartStartLoc;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI enemyRemainingText;

    // Start is called before the first frame update
    void Start() {
        AdjustMaximumHearts();
        playerHealthCurrent = player.health_current;
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
        currentMaxHearts = maxHearts;
        maxHearts = player.health_max / 2;
        curHearts = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
    }
    public void AddHalfHeart() {
        if (curHearts >= hearts.Length - 1)
            return;
        GameObject currentHeart = hearts[curHearts];
        if (currentHeart.CompareTag(HEART_EMPTY_TAG)) {
            hearts[curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[(int)curHearts].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        }
        else {
            hearts[curHearts] = Instantiate(heartFullPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[(int)curHearts].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
            if (curHearts < hearts.Length - 1)
                curHearts++;
        }
        Destroy(currentHeart);
    }
    public void RemoveHalfHeart() {
        GameObject currentHeart = hearts[curHearts];
        if (currentHeart.CompareTag(HEART_FULL_TAG)) {
            hearts[curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[(int)curHearts].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        }
        else {
            hearts[curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            hearts[(int)curHearts].transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
            if (curHearts != 0)
                curHearts--;
        }
        Destroy(currentHeart);
    }
    //update coin text by pulling amount of coins from player
    public void UpdateCoinText(int numCoins) {
        coinText.text = numCoins.ToString();
    }


}
