using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;

    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(0).gameObject;  //获得父级的第一个子级

        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Player"))        //进入房间
        {
            mapSprite.SetActive(true);             //显示  
        }
    }
}
