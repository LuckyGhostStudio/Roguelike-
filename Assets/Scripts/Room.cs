using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorUp, doorDown, doorLeft, doorRight;

    public bool roomUp, roomDown, roomLeft, roomRight;         //上下左右是否有房间

    public Text text;

    public int stepToStart;           //到初始房间的格子距离

    public int doorNumber;            //房间门的个数

    void Start()
    {
        //根据是否有房间启用门
        doorUp.SetActive(roomUp);
        doorDown.SetActive(roomDown);
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
    }

    public void UpdateRoom(float xOffset,float yOffset)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + (Mathf.Abs(transform.position.y / yOffset)));

        text.text = stepToStart.ToString();           //显示到文本

        if (roomUp) doorNumber++;
        if (roomDown) doorNumber++;
        if (roomLeft) doorNumber++;
        if (roomRight) doorNumber++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))        
        {
            CameraControl.instance.ChangeTarget(transform);     //切换目标点
        }
    }
}
