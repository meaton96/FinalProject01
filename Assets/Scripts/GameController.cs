using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public GameObject CoinPreFab;
    public GameObject HeartItemPreFab;
    [SerializeField] float cameraMovementSpeed;
    [SerializeField] GameObject[] grassObjects = new GameObject[5];
    [SerializeField] Vector2 backgroundStartingLocation;
    [SerializeField] int backgroundCol, backgroundRow;
    private const float grassSquareSize = .76f;
    [SerializeField] GameObject playerObject;
    //public float camMoveX, camMoveY;

    // Start is called before the first frame update
    void Start() {
        CreateBackground();
    }

    // Update is called once per frame
    void Update() {
    }
    private void CreateBackground() {
        for (int x = 0; x < backgroundCol; x++) {
            for (int y = 0; y < backgroundRow; y++) {
                Instantiate(grassObjects[Random.Range(0, grassObjects.Length)],
                    new Vector2(backgroundStartingLocation.x + (x * grassSquareSize),
                    backgroundStartingLocation.y - (y * grassSquareSize)),
                    Quaternion.identity);
            }
        }
    }
    
}
