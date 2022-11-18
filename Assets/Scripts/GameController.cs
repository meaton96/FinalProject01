using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject CoinPreFab;                                               //coin object for drawing on UI
    public GameObject HeartItemPreFab;                                          //heart object for drawing on UI
    public GameObject MagicianBallPreFab;                                       //magician projectile 
    [SerializeField] GameObject[] preFabList = new GameObject[2];               //list of all enemy prefabs in order to randomly choose which type of enemy to spawn
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
    private const int NUMBER_OF_ENEMIES_TO_SPAWN = 5;

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
            SpawnEnemyWave(NUMBER_OF_ENEMIES_TO_SPAWN);
        }
        else {
            if (playerObject != null)
                playerObject.GetComponent<PlayerBehaviour>().interfaceScript.
                    UpdateEnemiesRemaining(NumEnemiesLeft(currentEnemies));     //update the number of enemies if the player isnt dead
        }
    }
    //returns true if each of the objects in the array are null
    private bool AllAreNull<T>(T[] enemies) {
        for (int x = 0; x < enemies.Length; x++) {
            if (enemies[x] != null) {
                return false;
            }
        }
        return true;
    }
    //returns the amount of enemies left in the enemy array
    private int NumEnemiesLeft(GameObject[] enemies) {
        int numEnemies = 0;
        for (int x = 0; x < enemies.Length; x++) {
            if (enemies[x] != null)
                numEnemies++;
        }
        return numEnemies;
    }
    public GameObject[] GetEnemies() { return currentEnemies; }

    /// <summary>
    /// Spawns a wave of enemies
    /// </summary>
    /// <param name="numEnemies">the number of enemies to spawn on that wave</param>
    private void SpawnEnemyWave(int numEnemies) {

        float leftConstraint, topConstraint, rightConstraint, bottomConstraint;                             //constraints for where to spawn the enemies 
        leftConstraint = borderStartLocation.x + LARGE_ROCK_SIZE * 2 / 3;
        topConstraint = borderStartLocation.y - LARGE_ROCK_SIZE * 2 / 3;
        rightConstraint = leftConstraint + LARGE_ROCK_SIZE * numBorderRocksX - LARGE_ROCK_SIZE * 2.5f;
        bottomConstraint = topConstraint - LARGE_ROCK_SIZE * numBorderRocksY + LARGE_ROCK_SIZE * 2.5f;



        currentEnemies = new GameObject[numEnemies];                            //create a new array to store the spawned enemies 
        for (int i = 0; i < numEnemies; i++) {
            int index = Random.Range(0, preFabList.Length);

            //Instantiate an enemy and store it in the enemies array
            currentEnemies[i] = Instantiate(preFabList[index],
                new Vector3(Random.Range(leftConstraint, rightConstraint),
                            Random.Range(bottomConstraint, topConstraint), 0f),
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
