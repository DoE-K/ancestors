using UnityEngine;
using UnityEngine.UI;

public class PlayerHungerThirstScript : MonoBehaviour
{
    public Slider hungerSlider;
    public Slider thirstSlider;

    public float hunger = 100f;
    public float thirst = 100f;
    private float hungerDecayRate = 0.028f;
    private float thirstDecayRate = 0.083f;

    void Start()
    {
        hungerSlider.maxValue = 100;
        hungerSlider.value = hunger;
        thirstSlider.maxValue = 100;
        thirstSlider.value = thirst;
    }

    void Update()
    {

        hunger -= hungerDecayRate * Time.deltaTime;
        thirst -= thirstDecayRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        thirst = Mathf.Clamp(thirst, 0f, 100f);

        //hungerSlider.value = hunger;
        //thirstSlider.value = thirst;

        
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            if (rightHandItem != null && rightHandItem.CompareTag("Food"))
            {
                var food = rightHandItem.GetComponent<FoodScript>();
                if (food != null)
                {
                    float nutrition = food.nutrition * rightHandItem.transform.localScale.magnitude;
                    EatFood(nutrition);
                    Destroy(rightHandItem);
                    rightHandItem = null;
                    rightHandItemSave = "";
                }

            }
            else if (leftHandItem != null && leftHandItem.CompareTag("Food"))
            {
                var food = leftHandItem.GetComponent<FoodScript>();
                if (food != null)
                {
                    float nutrition = food.nutrition * leftHandItem.transform.localScale.magnitude;
                    EatFood(nutrition);
                    Destroy(leftHandItem);
                    leftHandItem = null;
                    leftHandItemSave = "";
                }

            }
        }*/

        if (hunger <= 0 || thirst <= 0) Die();
    }

    public void EatFood(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
        hungerSlider.value = hunger;
    }

    public void DrinkWater(float amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0f, 100f);
        thirstSlider.value = thirst;
    }
    
    void Die()
    {
        Debug.Log("Spieler ist gestorben!"); //TO DO
    }
}
