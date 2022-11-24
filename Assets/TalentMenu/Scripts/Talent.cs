using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Talent : MonoBehaviour, IComparable<Talent> {



    //  new string name;
    string description;
    int id;
    int cost;
    int effectId;
    private TextMeshProUGUI nameText, descriptionText, costText;
    private SpriteRenderer sr;

    [SerializeField] GameObject purchasedMarkPreFab;
    TextMeshProUGUI playerMoney;


    private bool HasBeenPurchased { get; set; }

    public int CompareTo(Talent other) {
        return id.CompareTo(other.id);
    }

    private void OnMouseDown() {
        if (!HasBeenPurchased) {
            int money = int.Parse(playerMoney.text);
            if (money >= cost)
                Purchase(money);
        }

    }

    //set the text for the button and 
    public void Init(string name, string desc, int cost, int id, int effectId, bool hasBeenPurchased) {
        
        //  this.name = name;
        this.id = id;
        description = desc;
        this.cost = cost;
        this.effectId = effectId;

        playerMoney = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();

        nameText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        costText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();

        nameText.SetText(name);
        descriptionText.SetText(description);
        costText.SetText(cost + "");

        if (hasBeenPurchased) {
         //   Purchase(int.Parse(playerMoney.text));

        }
    }
    public void SetSprite(Sprite sprite) {
        sr.sprite = sprite;
    }
    public void Purchase(int playerCurrentMoney) {
        HasBeenPurchased = true;
        Instantiate(purchasedMarkPreFab, transform.position, Quaternion.identity).transform.parent = transform;
        playerMoney.text = playerCurrentMoney - cost + "";
        PlayerBehaviour playerBehaviour = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
        playerBehaviour.RemoveCoins(cost);
        playerBehaviour.ApplyTalent(effectId, id);
    }
    
    public static bool operator <(Talent a, Talent b) {
        return a.CompareTo(b) < 0;
    }
    public static bool operator >(Talent a, Talent b) {
        return a.CompareTo(b) < 0;
    }
    

}
