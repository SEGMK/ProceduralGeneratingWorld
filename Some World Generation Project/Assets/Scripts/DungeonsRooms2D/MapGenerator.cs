using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public RoomTemplateGenerator RoomBase;
    public CreateCorridor Corridor;
    [SerializeField] private int MapWidth;
    [SerializeField] private int MapHeight;
    private GameObject[,] Map;
    private void Start()
    {
        Map = new GameObject[MapWidth, MapHeight];
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
        (int, int)? possiblePositionOfTheRoom;
        while (true) //change it in the way that it will be running until the min num of rooms are spawned
        {
            GameObject[,] room = GenerateRoom();
            possiblePositionOfTheRoom = PossibleRoomPositionOnTheMap((room.GetLength(0), room.GetLength(1)));
            if (possiblePositionOfTheRoom == null)
                break;
            PasteRoomIntoTheMap(room, ((int, int))possiblePositionOfTheRoom);
        }
    }
    private (int, int)? PossibleRoomPositionOnTheMap((int, int) roomSize)
    {
        (int, int)? possiblePosition;
        List<System.Func<object[,], int, int, bool>> listOfConditions = new List<System.Func<object[,], int, int, bool>>();
        listOfConditions.Add((object[,] map, int i, int j) => 
        {
            GameObject[,] unityMap = (GameObject[,])map;
            return unityMap[i, j] != null && unityMap[i, j].tag == "Corridor";
        });
        possiblePosition = Randomizeinator.RandomPositionFrom2DArray(listOfConditions, Map);
        if (possiblePosition == null)
            return possiblePosition;
        return RandomizePosiblePosition(roomSize, ((int, int))possiblePosition); // may return null even if there is still empty place for a room
    }
    private (int, int)? RandomizePosiblePosition((int, int) roomSize, (int, int) possiblePosition)
    {
        (int, int) newPosition;
        int allCombinations = (roomSize.Item1 * 2) + (roomSize.Item2 * 2) - 2;
        for(int i = 0; i <= allCombinations; i++)
        {
            newPosition.Item1 = possiblePosition.Item1 + Random.Range(-roomSize.Item1, roomSize.Item1);
            newPosition.Item2 = possiblePosition.Item1 + Random.Range(-roomSize.Item2, roomSize.Item2);
            if (!IsRoomOutOfTheMapBoundaries(newPosition, roomSize) && !IsThereAnotherRoom(newPosition, roomSize))
                return newPosition;
            newPosition = (0, 0);
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
    private bool IsThereAnotherRoom((int, int) possiblePositionOfTheRoom, (int, int) roomSize)
    {
        for (int i = possiblePositionOfTheRoom.Item1; i <= possiblePositionOfTheRoom.Item1 + roomSize.Item1; i++)
        {
            for (int j = possiblePositionOfTheRoom.Item2; j <= possiblePositionOfTheRoom.Item2 + roomSize.Item2; j++) //IndexOutOfRangeException
            {
                if (Map[i, j] != null && Map[i, j].tag != "Corridor")
                {
                    return true;
                }
            }
        }
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
    
    private GameObject[,] GenerateRoom()
    {
        GameObject[,] room = RoomBase.GenerateRoomBase();
        return room;
    }
}
