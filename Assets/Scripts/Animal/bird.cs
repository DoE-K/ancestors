using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : AnimalScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public override void Act()
    {
        // z. B. "Gras fressen" oder stehen bleiben
    }
}
