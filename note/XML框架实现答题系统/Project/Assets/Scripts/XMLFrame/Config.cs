using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config 
{
    public virtual bool ReadLoad(string xmlText)
    {
        if (string.IsNullOrEmpty(xmlText))
            return false;
        return true;
    }
}
