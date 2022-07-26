using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContral : MonoBehaviour
{
    public GameObject Map;
    bool isMap;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMap = !isMap;
            Map.SetActive(isMap);
        }
    }
}
