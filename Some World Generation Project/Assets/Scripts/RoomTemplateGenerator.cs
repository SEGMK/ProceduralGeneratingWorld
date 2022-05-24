using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomTemplate", menuName = "RoomsCreatingClasses/RoomBase")]
public class RoomTemplateGenerator : ScriptableObject
{
    [SerializeField] private GameObject[] NormalFloorTiles;
    [SerializeField] private GameObject[] Walls;
    [SerializeField] private int MinRoomSize;
    [SerializeField] private int MaxRoomSize;
    private const int WallThickness = 1; //leave it or rewrite GenerateWalls Method
    public GameObject[,] GenerateRoomBase()
    {
        GameObject[,] room = GenerateFloor(NormalFloorTiles, (Random.Range(MinRoomSize, MaxRoomSize), Random.Range(MinRoomSize, MaxRoomSize)));
        GenerateWalls(ref room, Walls);
        return room;
    }
    public void PrintRoomOnTheMap(GameObject[,] room)
    {
        for (int i = 0; i < room.GetLength(0); i++)
        {
            for (int j = 0; j < room.GetLength(1); j++)
            {
                Instantiate(room[i, j], new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }
    private GameObject[,] GenerateFloor(GameObject[] floorTiles, (int, int) size)
    {
        GameObject[,] roomFloor = new GameObject[size.Item1 + (WallThickness * 2), size.Item2 + (WallThickness * 2)];
        for (int i = 0; i < roomFloor.GetLength(0); i++)
        {
            for (int j = 0; j < roomFloor.GetLength(1); j++)
            {
                roomFloor[i, j] = floorTiles[Random.Range(0, floorTiles.Length)];
            }
        }
        return roomFloor;
    }
    private void GenerateWalls(ref GameObject[,] room, GameObject[] walls)
    {
        for (int i = 0; i < room.GetLength(0); i++)
        {
            room[i, 0] = walls[Random.Range(0, walls.Length)];
            room[i, room.GetLength(1) - 1] = walls[Random.Range(0, walls.Length)];
        }
        for (int i = 0; i < room.GetLength(1); i++)
        {
            room[0, i] = walls[Random.Range(0, walls.Length)];
            room[room.GetLength(0) - 1, i] = walls[Random.Range(0, walls.Length)];
        }
    }
}
