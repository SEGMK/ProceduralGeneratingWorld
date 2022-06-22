using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public RoomTemplateGenerator RoomBase;
    public CreateCorridor Corridor;
    private GameObject[,] Map = new GameObject[99, 99];
    private void Start()
    {
        GenerateMap();
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
        Map = Corridor.CreateCorridorMap(Map);
        bool thereIsStillSpace = true;
        (int, int) possiblePositionOfTheRoom;
        while (thereIsStillSpace)
        {
            GameObject[,] room = GenerateRoom();
            try
            {
                possiblePositionOfTheRoom = PossibleRoomPositionOnTheMap((room.GetLength(0) - 1, room.GetLength(1) - 1));
            }
            catch (System.IndexOutOfRangeException) //create your own ExceptionClass
            {
                break;
            }
            PasteRoomIntoTheMap(room, possiblePositionOfTheRoom);
        }
    }
    private (int, int) PossibleRoomPositionOnTheMap((int, int) roomSize) //push this randomizing thing to static class
    {
        List<int> randomOrderedPositionsOfXAxies = new List<int>();
        List<int> randomOrderedPositionsOfYAxies = new List<int>();
        randomOrderedPositionsOfXAxies = Enumerable.Range(0, Map.GetLength(0)).OrderBy(x => Random.Range(0, Map.GetLength(0))).ToList();
        foreach (var i in randomOrderedPositionsOfXAxies)
        {
            randomOrderedPositionsOfYAxies = Enumerable.Range(0, Map.GetLength(1)).OrderBy(x => Random.Range(0, Map.GetLength(1))).ToList();
            foreach (var j in randomOrderedPositionsOfYAxies)
            {
                if (Map[i, j] != null && Map[i, j].tag == "Corridor")
                {
                    (int, int)? roomRandomizedPosition = RandomizePosiblePosition(roomSize, (i, j));
                    if(roomRandomizedPosition != null)
                        return ((int, int))roomRandomizedPosition;
                }
            }
        }
        throw new System.IndexOutOfRangeException("There can not be room of this size on the map");
    }
    private (int, int)? RandomizePosiblePosition((int, int) roomSize, (int, int) possiblePosition)
    {
        List<int> rangeOfPosiblePositionsX = new List<int>();
        List<int> rangeOfPosiblePositionsY = new List<int>();
        rangeOfPosiblePositionsX = Enumerable.Range(0, roomSize.Item1).OrderBy(x => Random.Range(0, roomSize.Item1)).ToList();
        foreach (var i in rangeOfPosiblePositionsX)
        {
            rangeOfPosiblePositionsY = Enumerable.Range(0, roomSize.Item2).OrderBy(x => Random.Range(0, roomSize.Item2)).ToList();
            foreach (var j in rangeOfPosiblePositionsY)
            {
                int possiblePositionX = possiblePosition.Item1 - i;
                int possiblePositionY = possiblePosition.Item2 - j;
                if (!IsRoomOutOfTheMapBoundaries((possiblePositionX, possiblePositionY), roomSize) && !IsThereAnotherRoom((possiblePositionX, possiblePositionY), roomSize))
                {
                    return (possiblePositionX, possiblePositionY);
                }
            }
        }
        return null;
    }
    private bool IsRoomOutOfTheMapBoundaries((int, int) roomPosition, (int, int) roomSize)
    {
        if (Map.GetLength(0) < roomPosition.Item1 + roomSize.Item1 || Map.GetLength(1) < roomPosition.Item2 + roomSize.Item2)
            return true;
        else
            return false;
    }
    private void PasteRoomIntoTheMap(GameObject[,] room, (int, int) possiblePositionOfTheRoom)
    {
        (int, int) roomSize = (room.GetLength(0), room.GetLength(1));
        int roomsXAxesDimention = 0;
        int roomsYAxesDimention = 0;
        for (int i = possiblePositionOfTheRoom.Item1; i < possiblePositionOfTheRoom.Item1 + roomSize.Item1; i++)
        {
            for (int j = possiblePositionOfTheRoom.Item2; j < possiblePositionOfTheRoom.Item2 + roomSize.Item2; j++)
            {
                Map[i, j] = room[roomsXAxesDimention, roomsYAxesDimention];
                roomsYAxesDimention++;
            }
            roomsYAxesDimention = 0;
            roomsXAxesDimention++;
        }
    }
    private bool IsThereAnotherRoom((int, int) possiblePositionOfTheRoom, (int, int) roomSize)
    {
        for (int i = possiblePositionOfTheRoom.Item1; i <= possiblePositionOfTheRoom.Item1 + roomSize.Item1; i++)
        {
            for (int j = possiblePositionOfTheRoom.Item2; j <= possiblePositionOfTheRoom.Item2 + roomSize.Item2; j++)
            {
                if (Map[i, j] != null && Map[i, j].tag != "Corridor")
                {
                    return true;
                }
            }
        }
        return false;
    }
    private GameObject[,] GenerateRoom()
    {
        GameObject[,] room = RoomBase.GenerateRoomBase();
        return room;
    }
}
