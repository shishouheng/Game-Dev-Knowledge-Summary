using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTemplate<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                int number = FindObjectsOfType<T>().Length;
                if (number > 1)
                {
                    Debug.LogError("instance count>1");
                }
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}