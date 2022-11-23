using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Talent : MonoBehaviour, IComparable<Talent>
{

    

    //  new string name;
    string description;                                             
    int id;
  //  int cost;
    private TextMeshProUGUI nameText, descriptionText, costText;    
    private SpriteRenderer sr;
    

    

    private bool HasBeenPurchased { get; set; }

    public int CompareTo(Talent other) {
        return id.CompareTo(other.id);
    }

    //set the text for the button and 
    public void Init(string name, string desc, int cost, int id) {
      //  this.name = name;
        this.id = id;
        description = desc;

        nameText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        costText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();    

        nameText.SetText(name);
        descriptionText.SetText(description);
        costText.SetText(cost + "");

    }
    public void SetSprite(Sprite sprite) {
        sr.sprite = sprite;
    }
    public void Purchase() {
        HasBeenPurchased = true;
    }
    
    public static bool operator <(Talent a, Talent b) {
        return a.CompareTo(b) < 0;
    }
    public static bool operator >(Talent a, Talent b) {
        return a.CompareTo(b) < 0;
    }

}
