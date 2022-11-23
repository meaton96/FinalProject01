using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TalentTreeBehaviour : MonoBehaviour {
    [SerializeField] private Sprite[] talentButtonSprites = new Sprite[16];
    private const int HEART_SPRITE_INDEX = 15;
    [SerializeField] private GameObject talentObjectPreFab;
    private const string TALENT_FILE_PATH = "Assets\\talents.json";
    public static Vector2 ROOT_NODE_LOCATION = new(0f, -4f);

    [SerializeField] private GameObject leftBracket, rightBracket;

    public const float BRACKET_OFFSET_Y = 1.2f;
    public const float BRACKET_OFFSET_X = 1.6f;

    public const float OFFSET_Y = 2.4f;
    public const float OFFSET_X = 3;

    public const float START_X = 0;
    public const float START_Y = -4;

    BinaryTree talentTree;
    // Start is called before the first frame update
    void Start() {

        string json = "";
        using (StreamReader sr = File.OpenText(TALENT_FILE_PATH)) {
            while (!sr.EndOfStream) {
                json += sr.ReadLine();
            }

            json = json[1..];
            json.Trim('}');
            string[] talents = json.Split("}");
            for (int x = 0; x < talents.Length; x++) {
                //Debug.Log(talents[x]);
                ParseJsonLine(talents[x]);
            }
        }

        talentTree.ActivateAllNodes(talentTree.Root);
        talentTree.SetNodeTransforms(talentTree.Root, OFFSET_X, OFFSET_Y, BRACKET_OFFSET_X,
            BRACKET_OFFSET_Y, leftBracket, rightBracket);

        //Debug.Log(talentTree.Root.Data.ToString());
    }

    // Update is called once per frame
    void Update() {

    }
    void HandleMouseClick() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.collider == null) {

            }
        }
    }
    //takes a line of json data and turns it into a talent node
    void ParseJsonLine(string json) {
        if (json.IndexOf('{') == -1)
            return;

        //Debug.Log(json);

        int cost;
        string name, description;
        int id;

        name = json.Substring(2, json.IndexOf('{') - 5);
        name = name.Replace('\"', '\u2009');


        json = json.Remove(0, name.Length + 5);

        string[] jsonLines = json.Split(",");
        string[] variables = new string[jsonLines.Length];
        //Debug.Log(json);

        for (int x = 0; x < jsonLines.Length; x++) {
            variables[x] = jsonLines[x][jsonLines[x].IndexOf(':')..].Trim('\"');
            variables[x] = variables[x][3..];
            // Debug.Log(variables[x]);
        }
        // name = variables[0];
        description = variables[0];
        id = int.Parse(variables[1]);
        cost = int.Parse(variables[2][..variables[2].IndexOf('\"')]);       //trim off the last quotation because trim didnt work???

        GameObject temp = Instantiate(talentObjectPreFab, ROOT_NODE_LOCATION, Quaternion.identity);
        temp.GetComponent<Talent>().Init(name, description, cost, id);
        temp.SetActive(false);
        temp.transform.parent = transform;


        // Debug.Log(name + "\n" + description + "\n" + id + "\n" + cost);

        // Debug.Log(temp.GetComponent<Talent>().name);
        if (name.Contains("Heart")) {
            temp.GetComponent<Talent>().SetSprite(talentButtonSprites[HEART_SPRITE_INDEX]);
        }
        else
            temp.GetComponent<Talent>().SetSprite(talentButtonSprites[Random.Range(0, HEART_SPRITE_INDEX - 1)]);

        if (talentTree == null) {
            talentTree = new BinaryTree(temp);
            //temp.transform.position = new Vector2(Talent.START_X, Talent.START_Y);
        }
        else {
            talentTree.Add(temp, talentTree.Root);
            /* Node<GameObject> node = talentTree.Find(talentTree.Root, temp);
             //null reference
             //this is horrible
             //using a tree for this is the worst thing ever
             if (temp.GetComponent<Talent>() < node.Parent.Data.GetComponent<Talent>()) 
                 temp.transform.position = new Vector2(node.Parent.Data.transform.position.x - Talent.OFFSET_X,
                     node.Parent.Data.transform.position.y + Talent.OFFSET_Y);
             else
                 temp.transform.position = new Vector2(node.Parent.Data.transform.position.x + Talent.OFFSET_X,
                     node.Parent.Data.transform.position.y + Talent.OFFSET_Y);
            */

        }

    }




}
