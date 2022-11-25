using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceScript : MonoBehaviour {

    public PlayerBehaviour player;                                              //player pointer
    [SerializeField] GameObject UIHeartPreFab;                                  //prefab for the ui heart object
    public float heartDistance;                                                 //distance between heart objects on UI
    public GameObject[] hearts;                                                 //array of currently drawn heart objects
    private int maxPlayerHealth;                                                //the maximum number of heart and current heart index
    public Vector2 heartStartLoc;                                               //place on the screen to begin drawing the hearts
    [SerializeField] private TextMeshProUGUI coinText;                          //coin text object to display number of coins
    [SerializeField] private TextMeshProUGUI enemyRemainingText;                //text to display number of enemies left
    [SerializeField] private TextMeshProUGUI spawnersReaminingText;             //text to display unopened chests in the level

    private int currentPlayerHealth;                                            //

    private UIHeartBehaviour[] uiHearts;
    // Start is called before the first frame update
    void Start() {
        InitHearts();
        currentPlayerHealth = player.health_current;
    }

    void InitHearts() {
        uiHearts = new UIHeartBehaviour[15];
        for (int x = 0; x < uiHearts.Length; x++) {
            uiHearts[x] = Instantiate(UIHeartPreFab, new Vector2(
                heartStartLoc.x + x * heartDistance, heartStartLoc.y), 
                Quaternion.identity).GetComponent<UIHeartBehaviour>();
            uiHearts[x].gameObject.transform.parent = transform;
        }
    }
    public void UpdateEnemiesRemaining(int numEnemies) {
        enemyRemainingText.SetText("Remaining " + numEnemies);
    }

    private void Update() {
        if (currentPlayerHealth != player.health_current || maxPlayerHealth != player.health_max) {
            currentPlayerHealth = player.health_current;
            maxPlayerHealth = player.health_max;
            UpdateHearts();
        }
    }
    void UpdateHearts() {

        for (int x = 0; x < player.health_max / 2; x++) {
            if (x < currentPlayerHealth / 2) {
                uiHearts[x].SetActiveSprite(UIHeartBehaviour.FULL_INDEX);
            }
            else
                uiHearts[x].SetActiveSprite(UIHeartBehaviour.EMPTY_INDEX);
        }
        if (currentPlayerHealth % 2 != 0) {
            uiHearts[currentPlayerHealth / 2].SetActiveSprite(UIHeartBehaviour.HALF_INDEX);
        }
    }
    
    //update coin text by pulling amount of coins from player
    public void UpdateCoinText(int numCoins) {
        coinText.text = numCoins.ToString();
    }
    public void UpdateSpawnerText(int numSpawners) {
        spawnersReaminingText.text = "Spawners: " + numSpawners;
    }


}
