using UnityEngine;

public class PlayerCaveScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("cave"))
        {
            var cave = other.GetComponent<CaveScript>();

            if(!cave.isExit)
            {
                if(cave.caveName == "cave0")
                {
                    transform.position = new Vector3 (0, -156, 0);
                }

                if(cave.caveName == "cave1")
                {
                    transform.position = new Vector3 (-90, -202, 0);
                }

                if(cave.caveName == "cave2")
                {
                    transform.position = new Vector3 (-185, -202, 0);
                }

                if(cave.caveName == "cave3")
                {
                    transform.position = new Vector3 (-275, -202, 0);
                }
            }
            else
            {
                if(cave.caveName == "cave0")
                {
                    transform.position = new Vector3 (-45, 28, 0);
                }

                if(cave.caveName == "cave1")
                {
                    transform.position = new Vector3 (-200, 73, 0);
                }

                if(cave.caveName == "cave2")
                {
                    transform.position = new Vector3 (-105, -42, 0);
                }

                if(cave.caveName == "cave3")
                {
                    transform.position = new Vector3 (20, 78, 0);
                }
            }
        }

    }
}
