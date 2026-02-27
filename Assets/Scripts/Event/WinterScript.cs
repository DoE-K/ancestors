using UnityEngine;
using System;

public class WinterScript : MonoBehaviour
{
    public GameObject snow;

    void Start()
    {
        UpdateSnowState();
    }

    void UpdateSnowState()
    {
        DateTime now = DateTime.Now;

        bool isWinter = (now.Month == 11) || (now.Month == 12) || (now.Month == 1) || (now.Month == 2);

        if (isWinter == true)
        {
            snow.SetActive(true);
        }
        else
        {
            snow.SetActive(false);
        }
    }
}
