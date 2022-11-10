using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject CoinPreFab;
    public GameObject HeartItemPreFab;
    public GameObject MagicianBallPreFab;
    [SerializeField] float cameraMovementSpeed;
    [SerializeField] GameObject[] grassObjects = new GameObject[5];
    [SerializeField] GameObject[] largeRockObjects = new GameObject[2];
    [SerializeField] Vector2 backgroundStartingLocation;
    [SerializeField] int backgroundCol, backgroundRow;
    private const float GRASS_SIZE = .76f;
    [SerializeField] GameObject playerObject;
    private const float LARGE_ROCK_SIZE = 1.47f;
    private Vector2 borderStartLocation;
    private int numBorderRocksX, numBorderRocksY;

    //public float camMoveX, camMoveY;

    // Start is called before the first frame update
    void Start() {
        CreateBackground();
        CreateOuterBarrier();
    }

    // Update is called once per frame
    void Update() {
    }
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
    private void CreateOuterBarrier() {
        borderStartLocation = new Vector2(backgroundStartingLocation.x + 2.9f, backgroundStartingLocation.y - 1.6f);
        numBorderRocksX = (int)((backgroundCol * GRASS_SIZE) / LARGE_ROCK_SIZE) - 4;
        numBorderRocksY = (int)((backgroundRow * GRASS_SIZE) / LARGE_ROCK_SIZE) - 2;
        for (int x = 0; x < numBorderRocksX; x++) {
            for (int y = 0; y < numBorderRocksY; y++) {
                if (x == 0 || x == numBorderRocksX - 1 || y == 0 || y == numBorderRocksY - 1) {
                    Instantiate(largeRockObjects[Random.Range(0, 1)], new Vector2(borderStartLocation.x + x * LARGE_ROCK_SIZE, borderStartLocation.y - y * LARGE_ROCK_SIZE), Quaternion.identity);
                }
            }
        }
    }
    
}
