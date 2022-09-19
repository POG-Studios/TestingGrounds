using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataObj 
{
    

    public List<dataComponent> saveDat;

    public SaveDataObj()
    {
        saveDat = new List<dataComponent>();
    }
}
