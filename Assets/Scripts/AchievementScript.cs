using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementScript : MonoBehaviour
{
    public GameObject pointPopup;
    public TextMeshProUGUI pointPopupText;


    private PlayerScript player;

    private bool BallRunMemorialFound;

    public static bool stoneFound;
    public static bool branchFound;
    public static bool silverFound;
    public static bool goldFound;
    public static bool fireFound;
    public static bool cordageFound;
    public static bool hammerstoneFound;
    public static bool obsidianFound;
    public static bool obsidianbladeFound;
    public static bool obsidiansplinterFound;
    public static bool plantfiberFound;
    public static bool stickFound;
    public static bool stonebladeFound;
    public static bool stonesplinterFound;
    public static bool woodpieceFound;
    public static bool urblackberryFound;
    public static bool urblueberryFound;
    public static bool urfigFound;
    public static bool urmangoFound;
    public static bool urdateFound;
    public static bool uravocadoFound;

    public static bool boneFound;
    public static bool boneshardFound;
    public static bool needleFound;
    public static bool hideFound;
    public static bool driedhideFound;
    public static bool preparedhideFound;
    public static bool fabricFound;
    
    public static bool plankFound;
    public static bool raftFound;
    public static bool boatFound;
    public static bool shipFound;

    // Start is called before the first frame update
    void Start()
    {
        BallRunMemorialFound = false;
        
        stoneFound = false;
        branchFound = false;
        silverFound = false;
        goldFound = false;
        fireFound = false;
        cordageFound = false;
        hammerstoneFound = false;
        obsidianFound = false;
        obsidianbladeFound = false;
        obsidiansplinterFound = false;
        plantfiberFound = false;
        stickFound = false;
        stonebladeFound = false;
        stonesplinterFound = false;
        woodpieceFound = false;
        urblackberryFound = false;
        urblueberryFound = false;
        urfigFound = false;
        urmangoFound = false;
        urdateFound = false;
        uravocadoFound = false;

        boneFound = false;
        boneshardFound = false;
        needleFound = false;
        hideFound = false;
        driedhideFound = false;
        preparedhideFound = false;
        fabricFound = false;

        plankFound = false;
        raftFound = false;
        boatFound = false;
        shipFound = false;

        player = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ItemFound();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Visited(other);

    }

    /*private void ItemFound()
    {
        if(player.rightHandItemSave == "Stone" && stoneFound == false)
        {
            AddPoints(10);
            stoneFound = true;  
        }
                
    }*/

    private void ItemFound()
    {
        switch (player.rightHandItemSave)
        {
            case "Stone":
                if (!stoneFound)
                {
                    AddPoints(10);
                    stoneFound = true;
                }
                break;

            case "Branch":
                if (!branchFound)
                {
                    AddPoints(10);
                    branchFound = true;
                }
                break;

            case "Silver":
                if (!silverFound)
                {
                    AddPoints(10);
                    silverFound = true;
                }
                break;

            case "Gold":
                if (!goldFound)
                {
                    AddPoints(10);
                    goldFound = true;
                }
                break;

            case "Fire":
                if (!fireFound)
                {
                    AddPoints(10);
                    fireFound = true;
                }
                break;

            case "Cordage":
                if (!cordageFound)
                {
                    AddPoints(10);
                    cordageFound = true;
                }
                break;

            case "Hammerstone":
                if (!hammerstoneFound)
                {
                    AddPoints(10);
                    hammerstoneFound = true;
                }
                break;

            case "Obsidian":
                if (!obsidianFound)
                {
                    AddPoints(10);
                    obsidianFound = true;
                }
                break;

            case "Obsidianblade":
                if (!obsidianbladeFound)
                {
                    AddPoints(10);
                    obsidianbladeFound = true;
                }
                break;

            case "Obsidiansplinter":
                if (!obsidiansplinterFound)
                {
                    AddPoints(10);
                    obsidiansplinterFound = true;
                }
                break;

            case "Plantfiber":
                if (!plantfiberFound)
                {
                    AddPoints(10);
                    plantfiberFound = true;
                }
                break;

            case "Stick":
                if (!stickFound)
                {
                    AddPoints(10);
                    stickFound = true;
                }
                break;

            case "Stoneblade":
                if (!stonebladeFound)
                {
                    AddPoints(10);
                    stonebladeFound = true;
                }
                break;

            case "Stonesplinter":
                if (!stonesplinterFound)
                {
                    AddPoints(10);
                    stonesplinterFound = true;
                }
                break;

            case "Woodpiece":
                if (!woodpieceFound)
                {
                    AddPoints(10);
                    woodpieceFound = true;
                }
                break;

            case "UrBlackberry":
                if (!urblackberryFound)
                {
                    AddPoints(5);
                    urblackberryFound = true;
                }
                break;

            case "UrBlueberry":
                if (!urblueberryFound)
                {
                    AddPoints(5);
                    urblueberryFound = true;
                }
                break;

            case "UrFig":
                if (!urfigFound)
                {
                    AddPoints(5);
                    urfigFound = true;
                }
                break;

            case "UrMango":
                if (!urmangoFound)
                {
                    AddPoints(5);
                    urmangoFound = true;
                }
                break;

            case "UrDate":
                if (!urdateFound)
                {
                    AddPoints(5);
                    urdateFound = true;
                }
                break;

            case "UrAvocado":
                if (!uravocadoFound)
                {
                    AddPoints(5);
                    uravocadoFound = true;
                }
                break;

            case "Bone":
                if (!boneFound)
                {
                    AddPoints(10);
                    boneFound = true;
                }
                break;

            case "BoneShard":
                if (!boneshardFound)
                {
                    AddPoints(10);
                    boneshardFound = true;
                }
                break;

            case "Needle":
                if (!needleFound)
                {
                    AddPoints(10);
                    needleFound = true;
                }
                break;

            case "Hide":
                if (!hideFound)
                {
                    AddPoints(10);
                    hideFound = true;
                }
                break;

            case "DriedHide":
                if (!driedhideFound)
                {
                    AddPoints(10);
                    driedhideFound = true;
                }
                break;

            case "PreparedHide":
                if (!preparedhideFound)
                {
                    AddPoints(10);
                    preparedhideFound = true;
                }
                break;

            case "Fabric":
                if (!fabricFound)
                {
                    AddPoints(10);
                    fabricFound = true;
                }
                break;

            case "Raftblueprint":
                if (!raftFound)
                {
                    AddPoints(10);
                    raftFound = true;
                }
                break;

            case "Boatblueprint":
                if (!boatFound)
                {
                    AddPoints(10);
                    boatFound = true;
                }
                break;

            case "Shipblueprint":
                if (!shipFound)
                {
                    AddPoints(10);
                    shipFound = true;
                }
                break;
        }
    }


    private void Visited(Collider2D other)
    {
        if (other.CompareTag("BallRunMemorial") && BallRunMemorialFound == false)
        {
            AddPoints(100);
            BallRunMemorialFound = true;
        }
    }

    private void AddPoints(int points)
    {
        GlobalScore.AddScore(points);
        pointPopupText.text = "+" + points;
        pointPopup.SetActive(true);
    }
}
