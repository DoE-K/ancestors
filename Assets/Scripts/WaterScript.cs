//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaterScript : MonoBehaviour
{
    public GameObject nameTextObject;
    private TMP_Text nameText;

    public float thirst = 20f;

    void Start()
    {
        nameText = nameTextObject.GetComponent<TMP_Text>();
        nameText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            nameText.text = "water";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            nameText.text = "";
        }
    }
}
