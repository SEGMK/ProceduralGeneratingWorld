using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int NumberOfChecks = 4;
    public RoomTemplateGenerator RoomBase;
    public CreateCorridor Corridor;
    private GameObject[,] Map = new GameObject[99, 99]; //make public get and private set
    private void Start()
    {
        Map = Corridor.CreateCorridorMap(Map);
        //GenerateMap();
        PrintArrayIntoTheWorld();
    }
    private void PrintArrayIntoTheWorld()
    {
        int lengthX = Map.GetLength(0);
        int lengthY = Map.GetLength(1);
        for (int i = 0; i < lengthX; i++)
        {
            for (int j = 0; j < lengthY; j++)
            {
                if (Map[i, j] != null)
                {
                    Instantiate(Map[i, j], new Vector3(i, j, 0f), Quaternion.identity);
                }
            }
        }
    }
    public void GenerateMap()
    {
        bool thereIsStillSpace = true;
        while (thereIsStillSpace)
        {
            GameObject[,] room = GenerateRoom();
            (int, int) possiblePositionOfTheRoom;
            for (int i = 0; i < NumberOfChecks; i++)
            {
                possiblePositionOfTheRoom = (Random.Range(0, 99), Random.Range(0, 99));
                if (Map[possiblePositionOfTheRoom.Item1, possiblePositionOfTheRoom.Item2] == null)
                {
                    i = 0;
                    if (CheckIfPositionIsEmpty(possiblePositionOfTheRoom, (room.GetLength(0), room.GetLength(1))))
                    {
                        PasteRoomIntoTheMap(room, possiblePositionOfTheRoom);
                        break;
                    }
                }
                if (!(i < NumberOfChecks - 1))
                {
                    thereIsStillSpace = false;
                }
            }
        }
    }
    private void PasteRoomIntoTheMap(GameObject[,] room, (int, int) positionOnTheMap)
    {
        (int, int) roomSize = (room.GetLength(0), room.GetLength(1));
        int roomsXAxesDimention = 0;
        int roomsYAxesDimention = 0;
        for (int i = positionOnTheMap.Item1; i < positionOnTheMap.Item1 + roomSize.Item1; i++)
        {
            for (int j = positionOnTheMap.Item2; j < positionOnTheMap.Item2 + roomSize.Item2; j++)
            {
                Map[i, j] = room[roomsXAxesDimention, roomsYAxesDimention];
                roomsYAxesDimention++;
            }
            roomsYAxesDimention = 0;
            roomsXAxesDimention++;
        }
    }
    private bool CheckIfPositionIsEmpty((int, int) possiblePositionOfTheRoom, (int, int) roomSize)
    {
        if (Map.GetLength(0) < possiblePositionOfTheRoom.Item1 + roomSize.Item1 || Map.GetLength(1) < possiblePositionOfTheRoom.Item2 + roomSize.Item2)
        {
            //cuz of IndexOutOfRangeException and try...catch is SOOOOO slow
            return false;
        }
        bool isEmpty = true;
        for (int i = possiblePositionOfTheRoom.Item1; i < possiblePositionOfTheRoom.Item1 + roomSize.Item1; i++)
        {
            for (int j = possiblePositionOfTheRoom.Item2; j < possiblePositionOfTheRoom.Item2 + roomSize.Item2; j++)
            {
                if (!(Map[i, j] == null))
                {
                    isEmpty = false;
                }
            }
        }
        return isEmpty;
    }
    private GameObject[,] GenerateRoom()
    {
        GameObject[,] room = RoomBase.GenerateRoomBase();
        return room;
    }
}
