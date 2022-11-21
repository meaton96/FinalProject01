using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class GameController : MonoBehaviour {
    public GameObject CoinPreFab;                                               //coin object for drawing on UI
    public GameObject HeartItemPreFab;                                          //heart object for drawing on UI
    public GameObject MagicianBallPreFab;                                       //magician projectile 
    [SerializeField] GameObject[] enemyPreFabList = new GameObject[2];          //list of all enemy prefabs in order to randomly choose which type of enemy to spawn
    [SerializeField] GameObject[] sceneryPreFabList = new GameObject[9];        //list of all scenery prefabs to choose from
    [SerializeField] GameObject[] grassObjects = new GameObject[5];             //grass objects for drawing background
    [SerializeField] GameObject[] largeRockObjects = new GameObject[2];         //large rocks for creating the border 
    [SerializeField] Vector2 backgroundStartingLocation;                        //vector location for where to start drawing the background
    [SerializeField] int backgroundCol, backgroundRow;                          //number of columns and rows to draw for the background
    private const float GRASS_SIZE = .76f;                                      //the size in pixels of each grass square
    [SerializeField] GameObject playerObject;                                   //the player
    private const float LARGE_ROCK_SIZE = 1.47f;                                //the size in pixels of the large rocks
    private Vector2 borderStartLocation;                                        //vector to start drawing the border rocks
    private int numBorderRocksX, numBorderRocksY;                               //number of border rocks for each direction
    private List<GameObject> currentEnemies;                                        //array of enemies currently spawned in the level
    private const int NUMBER_OF_ENEMIES_TO_SPAWN = 20;
    private const int NUMBER_OF_SCENERY_TO_SPAWN = 30;
    [SerializeField] private GameObject[] chests = new GameObject[7];
    [SerializeField] private float enemySpawnDistance = 1.5f;
    private float spawnTimer = 0;
    [SerializeField] private const float SPAWN_TIME = 10;
    [SerializeField] private int CHEST_NUM_ITEMS_DROPPED = 5;


    // Start is called before the first frame update
    void Start() {
        Physics2D.IgnoreLayerCollision(6, 7);       //ignore all collisions between enemies and dropped items
        // CreateBackground();
        //SpawnEnemyWave(5);
        // CreateOuterBarrier();
        // SpawnEntities(sceneryPreFabList, NUMBER_OF_SCENERY_TO_SPAWN);
        //  SpawnEnemies();
        currentEnemies = new();
        InitChests();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update() {
        if (currentEnemies == null || (AllAreNull(currentEnemies) && NoSpawnersLeft())) {
            Debug.Log("Round over");
            //SceneManager.LoadScene(2);
            //Debug.Log("Load new scene");
           // SpawnEnemies();
        }
        else {
            //spawn an enemy from each spawner every SPAWN_TIME seconds
            if (playerObject != null) {
                if (spawnTimer >= SPAWN_TIME) {
                    spawnTimer = 0;
                    SpawnEnemies();
                }
                else
                    spawnTimer += Time.deltaTime;

                playerObject.GetComponent<PlayerBehaviour>().interfaceScript.
                    UpdateEnemiesRemaining(NumEnemiesLeft(currentEnemies));     //update the number of enemies if the player isnt dead
            }
            }
        }
    private bool NoSpawnersLeft() {
        for (int x = 0; x < chests.Length; x++) {
            if (chests[x].GetComponent<ChestBehaviour>().CanSpawnEnemy())
                return false;
        }
        return true;
    }
    private void InitChests() {
        for (int x = 0; x < chests.Length; x++) {
            chests[x].GetComponent<ChestBehaviour>().Init(CHEST_NUM_ITEMS_DROPPED);
        }

    }
    private void SpawnEnemies() {
        /*currentEnemies = new GameObject[NUMBER_OF_ENEMIES_TO_SPAWN];
        List<GameObject> enemies = SpawnEntities(enemyPreFabList, NUMBER_OF_ENEMIES_TO_SPAWN);
        for (int x = 0; x < enemies.Count; x++) {
            currentEnemies[x] = enemies[x];
        }*/
        for (int x = 0; x < chests.Length; x++) {
            if (chests[x].GetComponent<ChestBehaviour>().CanSpawnEnemy()) {
                float theta = Random.Range(0, 2 * Mathf.PI);
                int index = Random.Range(0, enemyPreFabList.Length);
                Vector2 spawnLocation = new(
                    Mathf.Cos(theta) * enemySpawnDistance + chests[x].transform.position.x,
                    Mathf.Sin(theta) * enemySpawnDistance + chests[x].transform.position.y);
                currentEnemies.Add(Instantiate(enemyPreFabList[index], spawnLocation, Quaternion.identity));
            }
        }

    }
    //returns true if each of the objects in the array are null
    private bool AllAreNull<T>(List<T> enemies) {
        for (int x = 0; x < enemies.Count; x++) {
            if (enemies[x] != null) {
                return false;
            }
        }
        return true;
    }
    //returns the amount of enemies left in the enemy array
    private int NumEnemiesLeft(List<GameObject> enemies) {
        int numEnemies = 0;
        for (int x = 0; x < enemies.Count; x++) {
            if (enemies[x] != null)
                numEnemies++;
        }
        return numEnemies;
    }
    public List<GameObject> GetEnemies() { return currentEnemies; }

    /// <summary>
    /// Spawns a number of entities on the level
    /// </summary>
    /// <param name="entityPrefabArray">the list of prefabs to chose from to spawn an entity</param>
    /// <param name="numberToSpawn">the number of entities to spawn</param>
    /// <returns>A list of all spawned game object entities</returns>
    private List<GameObject> SpawnEntities(GameObject[] entityPrefabArray, int numberToSpawn) {

        float leftConstraint, topConstraint, rightConstraint, bottomConstraint;  //constraints for where to spawn the entities 
        /* leftConstraint = borderStartLocation.x + LARGE_ROCK_SIZE * 2 / 3;
         topConstraint = borderStartLocation.y - LARGE_ROCK_SIZE * 2 / 3;
         rightConstraint = leftConstraint + LARGE_ROCK_SIZE * numBorderRocksX - LARGE_ROCK_SIZE * 2.5f;
         bottomConstraint = topConstraint - LARGE_ROCK_SIZE * numBorderRocksY + LARGE_ROCK_SIZE * 2.5f;
        */
        leftConstraint = -11.5f;
        rightConstraint = 11.5f;
        topConstraint = 6.5f;
        bottomConstraint = -6.5f;

        List<GameObject> entitiesSpawnedList = new();   //create list to store spawned entities 
        for (int i = 0; i < numberToSpawn; i++) {
            int index = Random.Range(0, entityPrefabArray.Length);
            Vector2 location;

            do {
                //create a new random location
                location = new Vector2(Random.Range(leftConstraint, rightConstraint),
                                    Random.Range(bottomConstraint, topConstraint));
                //Instantiate an enemy and store it in the enemies array
                entitiesSpawnedList.Add(Instantiate(entityPrefabArray[index], location, Quaternion.identity));

            } while (SpawnLocationNotEmpty(location)); //keep testing locations until there is an empty area
        }
        return entitiesSpawnedList;
    }
    /// <summary>
    /// Test to see if the spawn location is empty or not
    /// </summary>
    /// <param name="location">the vector2 location to test</param>
    /// <returns>true if the area around the location vector is empty false otherwise</returns>
    private bool SpawnLocationNotEmpty(Vector2 location) {
        return Physics2D.Raycast(location, new Vector2(.1f, 0), 0.1f);

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
        //2.9 and 1.6 + the width of the rock prevents the camera from viewing the edge of the grass squares 
        borderStartLocation = new Vector2(backgroundStartingLocation.x + 2.9f, backgroundStartingLocation.y - 1.6f);
        numBorderRocksX = (int)(backgroundCol * GRASS_SIZE / LARGE_ROCK_SIZE) - 4;  //4 is about the horizontal width of the camera from the center
        numBorderRocksY = (int)(backgroundRow * GRASS_SIZE / LARGE_ROCK_SIZE) - 2;  //2 is about the vertical height of the camera
        for (int x = 0; x < numBorderRocksX; x++) {
            for (int y = 0; y < numBorderRocksY; y++) {
                if (x == 0 || x == numBorderRocksX - 1 || y == 0 || y == numBorderRocksY - 1) {
                    Instantiate(largeRockObjects[Random.Range(0, 1)],   //choose one of the 2 rock options
                        new Vector2(borderStartLocation.x + x * LARGE_ROCK_SIZE, borderStartLocation.y - y * LARGE_ROCK_SIZE),
                        Quaternion.identity);
                }
            }
        }
    }

}
