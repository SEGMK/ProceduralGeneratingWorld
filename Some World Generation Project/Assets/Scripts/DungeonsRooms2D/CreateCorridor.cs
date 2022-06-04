using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CreateCorridor", menuName = "RoomsCreatingClasses/CreateCorridor")]
public class CreateCorridor : ScriptableObject
{
    public GameObject[] CorridorTiles;
    [SerializeField] private int MaxCorridorLeangth = 20;
    [SerializeField] private int ChanceToEndCreationOfCorridor = 20;
    [SerializeField] private int NumberOfCorridors = 30;
    [SerializeField] private int ChanceToCreateCorridorSideways = 4;
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
            return (ScanMapAxiesToFindNewPosition(map, map.GetLength(0), map.GetLength(1), true), true);
        }
        else
        {
            return (ScanMapAxiesToFindNewPosition(map, map.GetLength(1), map.GetLength(0), false), false);
        }
    }
    private (int, int) ScanMapAxiesToFindNewPosition(GameObject[,] map, int lengthOfAxisWhereMethodIsScanningAlong, 
        int lengthofScannedAxies, bool isScaningAlongXAxies)
    {
        int r = Random.Range(0, lengthOfAxisWhereMethodIsScanningAlong - 1);
        for (int i = r; i < lengthOfAxisWhereMethodIsScanningAlong;)
        {
            int r2 = Random.Range(0, lengthofScannedAxies - 1);
            for (int j = r2; j < lengthofScannedAxies;)
            {
                if (isScaningAlongXAxies)
                {
                    if (map[i, j] != null)
                    {
                        return (i, j);
                    }
                }
                else
                {
                    if (map[j, i] != null)
                    {
                        return (j, i);
                    }
                }
                j++;
                if (lengthofScannedAxies == j)
                    j = 0;
                if (j == r2)
                    break;
            }
            i++;
            if (lengthOfAxisWhereMethodIsScanningAlong == i)
                i = 0;
            if (i == r)
                break;
        }
        throw new System.Exception("Map seems empty");
    }
    private void GenerateCorridor(ref GameObject[,] map, (int, int) startingPoint, bool sidewayes)
    {
        bool positiveWay = System.Convert.ToBoolean(Random.Range(0, 1));
        for (int i = 1; i <= MaxCorridorLeangth; i++)
        {
            if (Random.Range(1, ChanceToEndCreationOfCorridor) == 1)
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
