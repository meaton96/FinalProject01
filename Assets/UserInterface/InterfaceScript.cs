using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceScript : MonoBehaviour {

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
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
        
    }

    // Update is called once per frame
    void Update() {

    }
    private void InstantiateHearts() {
        for (int x = 0; x < maxHearts; x++) {
            hearts[x] = Instantiate(heartFullPrefab,
                new Vector3(heartStartLoc.x + (x * heartDistance), heartStartLoc.y, 0),
                Quaternion.identity);
        }
    }
    public void AdjustHearts(AdjustHeart adjustHeart) {
        adjustHeart();
    }
    public void AdjustHeartMultipleTimes(AdjustHeart adjustHeart, int numTimes) {
        for (int x = 0; x < numTimes; x++)
            adjustHeart();
    }
    public void AdjustMaximumHearts() {
        maxHearts = player.health_max / 2.0;
        curHearts = maxHearts - 1;
        hearts = new GameObject[(int)maxHearts];
        InstantiateHearts();
    }
    public void AddHalfHeart() {
        GameObject currentHeart = hearts[(int)curHearts];
        if (curHearts >= hearts.Length - 1 && currentHeart.CompareTag(HEART_FULL_TAG))
            return;

        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartFullPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts++;
        }
        else {
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
        }
        Destroy(currentHeart);
    }
    public void RemoveHeartHalf() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
        }
        else {
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);

        }
        Destroy(currentHeart);
    }
    public void RemoveFullHeart() {
        if (curHearts < 0)
            return;
        GameObject currentHeart = hearts[(int)curHearts];
        if (currentHeart.CompareTag(HEART_HALF_TAG)) {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            curHearts--;
            Destroy(currentHeart);
            currentHeart = hearts[(int)curHearts];
            hearts[(int)curHearts] = Instantiate(heartHalfPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
        }
        else {
            hearts[(int)curHearts] = Instantiate(heartEmptyPrefab, currentHeart.transform.position, Quaternion.identity);
            Destroy(currentHeart);
            curHearts--;
        }

    }
    public void UpdateCoinText(int numCoins) {
        coinText.text = numCoins.ToString();
    }


}
