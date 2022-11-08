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
    public float camMoveX, camMoveY;

    // Start is called before the first frame update
    void Start() {
        CreateBackground();
    }

    // Update is called once per frame
    void Update() {
        AdjustCamera();
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
    private void AdjustCamera() {

        /*  float xAxisValue = Input.GetAxisRaw("Horizontal");
          float zAxisValue = Input.GetAxisRaw("Vertical");
          if (Camera.current != null) {
              Camera.current.transform.Translate(new Vector3(xAxisValue / 200f, zAxisValue / 200f, 0f));
          }
          */
        if (Camera.current != null) {

            Vector3 dif = Camera.current.transform.position - playerObject.transform.position;
            while (dif.x > 1.5f || dif.y > 1.5f) {
                dif = Camera.current.transform.position - playerObject.transform.position;
                Camera.current.transform.Translate(dif.x * cameraMovementSpeed * Time.deltaTime,
                    dif.y * cameraMovementSpeed * Time.deltaTime,
                    -10f);
            }
        }
        
    }
}
