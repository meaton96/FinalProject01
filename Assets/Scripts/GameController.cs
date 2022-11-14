using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject CoinPreFab;                                               //coin object for drawing on UI
    public GameObject HeartItemPreFab;                                          //heart object for drawing on UI
    public GameObject MagicianBallPreFab;                                       //magician projectile 
    [SerializeField] GameObject[] preFabList = new GameObject[2];
    [SerializeField] GameObject[] grassObjects = new GameObject[5];             //grass objects for drawing background
    [SerializeField] GameObject[] largeRockObjects = new GameObject[2];         //large rocks for creating the border 
    [SerializeField] Vector2 backgroundStartingLocation;                        //vector location for where to start drawing the background
    [SerializeField] int backgroundCol, backgroundRow;                          //number of columns and rows to draw for the background
    private const float GRASS_SIZE = .76f;                                      //the size in pixels of each grass square
    [SerializeField] GameObject playerObject;                                   //the player
    private const float LARGE_ROCK_SIZE = 1.47f;                                //the size in pixels of the large rocks
    private Vector2 borderStartLocation;                                        //vector to start drawing the border rocks
    private int numBorderRocksX, numBorderRocksY;                               //number of border rocks for each direction
    private GameObject[] currentEnemies;                                        //array of enemies currently spawned in the level

    // Start is called before the first frame update
    void Start() {
        CreateBackground();
        //SpawnEnemyWave(5);
        CreateOuterBarrier();
    }

    // Update is called once per frame
    void Update() {
        if (currentEnemies == null || AllAreNull(currentEnemies)) {
            //SceneManager.LoadScene(2);
            //Debug.Log("Load new scene");
            SpawnEnemyWave(15);
        }
        else {
            playerObject.GetComponent<PlayerBehaviour>().interfaceScript.UpdateEnemiesRemaining(NumEnemiesLeft(currentEnemies));
        }
    }
    private bool AllAreNull(GameObject[] enemies) {
        for (int x = 0; x < enemies.Length; x++) {
            if (enemies[x] != null) {
                return false;
            }
        }
        return true;
    }
    private int NumEnemiesLeft(GameObject[] enemies) {
        int numEnemies = 0;
        if (AllAreNull(enemies))
            return 0;
        for (int x = 0; x < enemies.Length; x++) {
            if (enemies[x] != null)
                numEnemies++;
        }
        return numEnemies;
    }

    private void SpawnEnemyWave(int numEnemies) {
        //constraints not right *********************************************************
        float leftConstraint, topConstraint, rightConstraint, bottomConstraint;
        leftConstraint = backgroundStartingLocation.x + LARGE_ROCK_SIZE +.5f;   
        topConstraint = backgroundStartingLocation.y - LARGE_ROCK_SIZE - .5f;
        rightConstraint = leftConstraint + LARGE_ROCK_SIZE * (numBorderRocksX);
        bottomConstraint = topConstraint - LARGE_ROCK_SIZE * (numBorderRocksY);

        currentEnemies = new GameObject[numEnemies];
        for (int i = 0; i < numEnemies; i++) {
            int index = Random.Range(0, preFabList.Length);

            //Debug.Log($"({Random.Range(leftConstraint, rightConstraint)},{Random.Range(bottomConstraint, topConstraint)})");
            currentEnemies[i] = Instantiate(preFabList[index],
                new Vector3(Random.Range(leftConstraint, rightConstraint),
                            Random.Range(bottomConstraint,topConstraint), 0f),
                            Quaternion.identity);
            
        }
    }
    //creates a grass background by tiling the randomly chosen grass squares
    private void CreateBackground() {
        for (int x = 0; x < backgroundCol; x++) {
            for (int y = 0; y < backgroundRow; y++) {
                Instantiate(grassObjects[Random.Range(0, grassObjects.Length)],
                    new Vector2(backgroundStartingLocation.x + (x * GRASS_SIZE),
                    backgroundStartingLocation.y - (y * GRASS_SIZE)),
                    Quaternion.identity);
            }
        }
    }
    //creates a square border of large rocks so the player cannot leave the game world
    private void CreateOuterBarrier() {
        borderStartLocation = new Vector2(backgroundStartingLocation.x + 2.9f, backgroundStartingLocation.y - 1.6f);
        numBorderRocksX = (int)(backgroundCol * GRASS_SIZE / LARGE_ROCK_SIZE) - 4;
        numBorderRocksY = (int)(backgroundRow * GRASS_SIZE / LARGE_ROCK_SIZE) - 2;
        for (int x = 0; x < numBorderRocksX; x++) {
            for (int y = 0; y < numBorderRocksY; y++) {
                if (x == 0 || x == numBorderRocksX - 1 || y == 0 || y == numBorderRocksY - 1) {
                    Instantiate(largeRockObjects[Random.Range(0, 1)], new Vector2(borderStartLocation.x + x * LARGE_ROCK_SIZE, borderStartLocation.y - y * LARGE_ROCK_SIZE), Quaternion.identity);
                }
            }
        }
    }

}
