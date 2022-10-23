using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class CustomAttributes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/

[AttributeUsage(AttributeTargets.Struct)]
public class SaveComponent : Attribute
{
    
}


[AttributeUsage(AttributeTargets.Struct)]
public class EntityReference : Attribute
{
    
}