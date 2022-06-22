using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "CreateCorridor", menuName = "RoomsCreatingClasses/CreateCorridor")]
public class CreateCorridor : ScriptableObject
{
    [SerializeField] private GameObject[] CorridorTiles;
    [SerializeField] private int MaxCorridorLeangth = 20;
    [SerializeField] private int ChanceToEndCreationOfCorridor = 20;
    [SerializeField] private int NumberOfCorridors = 30;
    [SerializeField] private int ChanceToCreateCorridorSideways = 4;
    [SerializeField] private int MinLenghtOfCorridor = 5;
    public GameObject[,] CreateCorridorMap(GameObject[,] map)
    {
        ((int, int), bool) startingPointAndDirection = ((Random.Range(1, map.GetLength(0) - 1), Random.Range(1, map.GetLength(1) - 1)), true);
        for (int i = 0; i < NumberOfCorridors; i++)
        {
            GenerateCorridor(ref map, startingPointAndDirection.Item1, startingPointAndDirection.Item2);
            startingPointAndDirection = FindNewStartingPosition(map);
        }
        return map;
    }
    private void DebugThing(GameObject[,] map) //delete it
    {
        foreach (var i in map)
        {
            if (i != null)
            {
                Debug.Log("Hit");
            }
        }
        Debug.Log("End of the map");
    }
    private ((int, int), bool) FindNewStartingPosition(GameObject[,] map)
    {
        if (Random.Range(1, ChanceToCreateCorridorSideways) == 1)
        {
            return (ScanMapAxiesToFindNewPosition(map, true), true);
        }
        else
        {
            return (ScanMapAxiesToFindNewPosition(map, false), false);
        }
    }
    private (int, int) ScanMapAxiesToFindNewPosition(GameObject[,] map, bool sidewayes)
    {
        List<int> randomOrderedPositionsOfXAxies = new List<int>();
        List<int> randomOrderedPositionsOfYAxies = new List<int>();
        randomOrderedPositionsOfXAxies = Enumerable.Range(0, map.GetLength(0)).OrderBy(x => Random.Range(0, map.GetLength(0))).ToList();
        foreach (var i in randomOrderedPositionsOfXAxies)
        {
            randomOrderedPositionsOfYAxies = Enumerable.Range(0, map.GetLength(1)).OrderBy(x => Random.Range(0, map.GetLength(1))).ToList();
            foreach (var j in randomOrderedPositionsOfYAxies)
            {
                if (sidewayes)
                {
                    if (map[j, i] != null && IsNotOverlapping(map, (i, j), sidewayes) && IsNotNearToAnotherCorridor(map, (i, j)))
                        return (j, i);
                    else
                        continue;
                }
                else
                {
                    if (map[i, j] != null && IsNotOverlapping(map, (i, j), sidewayes) && IsNotNearToAnotherCorridor(map, (i, j)))
                        return (i, j);
                    else
                        continue;
                }
            }
        }
        //StackOverflowException here
        return ScanMapAxiesToFindNewPosition(map, !sidewayes); //if there can not be any more corridors in exact asxies then change the axies
    }
    private bool IsNotNearToAnotherCorridor(GameObject[,] map, (int, int) position)
    {
        try
        {
            if (map[position.Item1 + 1, position.Item2 + 1] != null || map[position.Item1 + 1, position.Item2 - 1] != null ||
                map[position.Item1 - 1, position.Item2 + 1] != null || map[position.Item1 - 1, position.Item2 - 1] != null)
                return false;
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        return true;
    }
    private bool IsNotOverlapping(GameObject[,] map, (int, int) position, bool sidewayes)
    {
        try
        {
            if (sidewayes)
            {
                if (map[position.Item1 - 1, position.Item2] != null || map[position.Item1 + 1, position.Item2] != null)
                    return false;
            }
            else
            {
                if (map[position.Item1, position.Item2 + 1] != null || map[position.Item1, position.Item2 + 1] != null)
                    return false;
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
        return true;
    }
    private void GenerateCorridor(ref GameObject[,] map, (int, int) startingPoint, bool sidewayes)
    {
        bool positiveWay = System.Convert.ToBoolean(Random.Range(0, 2));
        for (int i = 1; i <= MaxCorridorLeangth; i++)
        {
            if (Random.Range(1, ChanceToEndCreationOfCorridor) == 1 && MinLenghtOfCorridor <= i)
                break;
            try
            {
                //doing it this way cuz these are all possibilities
                if (!sidewayes)
                {
                    if (positiveWay)
                    {
                        map[startingPoint.Item1, startingPoint.Item2 + i] = CorridorTiles[Random.Range(0, CorridorTiles.Length)];
                    }
                    else
                    {
                        map[startingPoint.Item1, startingPoint.Item2 - i] = CorridorTiles[Random.Range(0, CorridorTiles.Length)];
                    }
                }
                else
                {
                    if (positiveWay)
                    {
                        map[startingPoint.Item1 + i, startingPoint.Item2] = CorridorTiles[Random.Range(0, CorridorTiles.Length)];
                    }
                    else
                    {
                        map[startingPoint.Item1 - i, startingPoint.Item2] = CorridorTiles[Random.Range(0, CorridorTiles.Length)];
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                break;
            }
        }
    }
}
