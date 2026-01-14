using UnityEngine;

public class AnimalsTouchingSystem : MonoBehaviour
{
    private bool touchingOtherAnimal;

    private AnimalScript thisAnimal;
    private AnimalScript otherAnimal;

    void Awake()
    {
        thisAnimal = GetComponentInParent<AnimalScript>();
    }

    void Start()
    {
        touchingOtherAnimal = true;
    }

    void Update()
    {

        if(touchingOtherAnimal == true)
        {
            switch(thisAnimal.speciesName)
            {
                case "Mamooth": 
                    switch(otherAnimal.speciesName)
                    {
                        case "Tiger": otherAnimal.health -= 1 * Time.deltaTime; break;
                        case "Hippo": break;
                        case "Turtle": break;
                        case "Fish": break;
                    }
                break;

                case "Tiger": 
                    

                    switch(otherAnimal.speciesName)
                    {
                        case "Mamooth": Debug.Log("WIE BITTE"); break;
                        case "Hippo": otherAnimal.health -= 1 * Time.deltaTime; break;
                        case "Turtle": otherAnimal.health -= 1 * Time.deltaTime; break;
                        case "Fish": otherAnimal.health -= 1 * Time.deltaTime; break;
                    }
                break;

                case "Hippo": 
                    switch(otherAnimal.speciesName)
                    {
                        case "Tiger": break;
                        case "Mamooth": break;
                        case "Turtle": break;
                        case "Fish": otherAnimal.health -= 1 * Time.deltaTime; break;
                    }
                break;

                case "Turtle": 
                    switch(otherAnimal.speciesName)
                    {
                        case "Tiger": break;
                        case "Hippo": break;
                        case "Mamooth": break;
                        case "Fish": otherAnimal.health -= 1 * Time.deltaTime; break;
                    }
                break;

                case "Fish": 
                    switch(otherAnimal.speciesName)
                    {
                        case "Tiger": break;
                        case "Hippo": break;
                        case "Turtle": break;
                        case "Mamooth": break;
                    }
                break;

                


            }
        }

        if(thisAnimal.health <= 0)
        {
            Debug.Log("Tier gestorben");
            Destroy(thisAnimal.gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        AnimalScript a = other.GetComponentInParent<AnimalScript>();

        if(other.CompareTag("AnimalBody"))
        {
            if (a != null && thisAnimal != a)
            {
                otherAnimal = a;
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        AnimalScript a = other.GetComponentInParent<AnimalScript>();

        if(other.CompareTag("AnimalBody"))
        {
            if (a == otherAnimal && thisAnimal != otherAnimal)
            {
                otherAnimal = null;
            }
        }
    }
}
