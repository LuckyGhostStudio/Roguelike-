using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction {up,dowm,left,right};      //枚举类型
    public Direction direction;            //房间方向

    [Header("房间信息")]
    public GameObject roomPrefab;         
    public int roomNumber;                 //房间数量
    public Color startColor, endColor;     //开始和终点房间的颜色
    private GameObject endRoom;            //最后一个房间

    [Header("位置控制")]
    public Transform generatorPoint;       //控制位置的点
    public float xOffset;                  //水平位移
    public float yOffset;                  //竖直位移
    public LayerMask roomLayer;
    public int maxStep;                    //最远房间的距离

    public List<Room> rooms = new List<Room>();   //存放生成的房间

    List<GameObject> farRooms = new List<GameObject>();      //存放最远距离的房间
    List<GameObject> lessFarRooms = new List<GameObject>();  //存放次远距离的房间
    List<GameObject> oneDoorRooms = new List<GameObject>();  //存放只有一个门的房间

    public WallType wallType;

    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            //生成房间并添加到列表
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());

            ChangPointPos();      //改变Point位置
        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;   //设置颜色

        foreach (var room in rooms)       //遍历rooms
        {
            SetUpRoom(room, room.transform.position);  //设置房间
        }
        FindEndRoom();   //找最终房间

        endRoom.GetComponent<SpriteRenderer>().color = endColor;    //设置颜色
    }

    void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }

    public void ChangPointPos()      //改变Point位置
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);    //随机生成方向

            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);  //新房间位置向上移动
                    break;
                case Direction.dowm:
                    generatorPoint.position += new Vector3(0, -yOffset, 0); //新房间位置向下移动
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0); //新房间位置向左移动
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);  //新房间位置向右移动
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer)); //检测是否有重叠房间
    }

    public void SetUpRoom(Room newRoom,Vector3 roomPosition)     //设置房间
    {
        //检测上下左右是否有房间
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset);      //更新房间的距离信息

        //生成对应的墙
        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp) 
                    Instantiate(wallType.singleUP, roomPosition, Quaternion.identity);
                if (newRoom.roomDown) 
                    Instantiate(wallType.singleDown, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft) 
                    Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight) 
                    Instantiate(wallType.singleRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUD, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.doubleUL, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.doubleDR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.doubleDL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.tripleULR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.tripleDLR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft && newRoom.roomDown)
                    Instantiate(wallType.tripleLUD, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.tripleRUD, roomPosition, Quaternion.identity);
                break;
            case 4:
                Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity);
                break;
        }
    }

    public void FindEndRoom()      //找到最终房间
    {
        //找最大的距离
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep) maxStep = rooms[i].stepToStart;
        }
        //获得最大和次大距离的房间
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep) farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1) lessFarRooms.Add(room.gameObject);
        }

        //找到只有一个门的房间
        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1) oneDoorRooms.Add(farRooms[i]);
        }
        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1) oneDoorRooms.Add(lessFarRooms[i]);
        }

        //选择最后的房间
        if (oneDoorRooms.Count != 0)
        {
            endRoom = oneDoorRooms[Random.Range(0, oneDoorRooms.Count)];     //随机选一个单个门的房间
        }
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];             //随机选一个最远距离的房间
        }
    }
}

[System.Serializable]
public class WallType       //墙的类
{
    public GameObject singleUP, singleDown, singleLeft, singleRight,
                      doubleUD, doubleLR, doubleUL, doubleUR, doubleDL, doubleDR,
                      tripleULR, tripleDLR, tripleLUD, tripleRUD,
                      fourDoors;
}
