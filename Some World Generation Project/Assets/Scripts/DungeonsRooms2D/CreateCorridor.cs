using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CreateCorridor", menuName = "RoomsCreatingClasses/CreateCorridor")]
public class CreateCorridor : ScriptableObject
{
    public GameObject[] CorridorTiles;
    [SerializeField] private int MaxCorridorLeangth = 20;
    [SerializeField] private int ChanceToEndCreationOfCorridor = 10;
    [SerializeField] private int NumberOfCorridors = 10;
    private (int, int) MapSize;
    [SerializeField] private int ChanceToCreateCorridorSideways = 2;
    public GameObject[,] CreateCorridorMap(GameObject[,] map)
    {
        int dimensionX = map.GetLength(0);
        int dimensionY = map.GetLength(1);
        MapSize = (dimensionX - 1, dimensionY - 1);
        (int, int) startingPoint = (Random.Range(0, dimensionX), Random.Range(0, dimensionY));
        for (int i = 0; i < NumberOfCorridors; i++)
        {
            GenerateCorridor(ref map, startingPoint);
            startingPoint = FindNewStartingPosition(map);
        }
        DebugThing(map);
        return map;
    }
    private void DebugThing(GameObject[,] map)
    {
        foreach (var i in map)
        {
            if (i != null)
            {
                Debug.Log("Hit");
            }
        }
    }
    private (int, int) FindNewStartingPosition(GameObject[,] map)
    {
        int scanStartingPosition = Random.Range(0, MapSize.Item1);
        if(Random.Range(1, ChanceToCreateCorridorSideways) != 1)
            return ScanMapAxiesToFindNewPosition(scanStartingPosition, map, MapSize.Item1, MapSize.Item2, true);
        else
            return ScanMapAxiesToFindNewPosition(scanStartingPosition, map, MapSize.Item2, MapSize.Item1, false);
    }
    private (int, int) ScanMapAxiesToFindNewPosition(int scanStartingPosition, GameObject[,] map, int lengthOfAxisWhereMethodIsScanningAlong, 
        int lengthofScannedAxies, bool isScaningAlongXAxies)
    {
        while (true)
        {
            for (int i = 0; i <= lengthOfAxisWhereMethodIsScanningAlong / 10; i++) //how many scansc are made every 10 tiles
            {
                for (int j = 0; j < lengthofScannedAxies; j++) //scan whole axies row at scanPosition position
                {
                    switch (isScaningAlongXAxies) //try to write it clearer //if...else is killing my eyes here thats why I did it in switch...case at the moment
                    {
                        case true:
                            if (map[scanStartingPosition, j] != null)
                            {
                                return (scanStartingPosition, j);
                            }
                            break;
                        case false:
                            if (map[j, scanStartingPosition] != null)
                            {
                                return (j, scanStartingPosition);
                            }
                            break;
                    }
                }
                if (lengthOfAxisWhereMethodIsScanningAlong < scanStartingPosition + 10)
                {
                    scanStartingPosition = (scanStartingPosition + 10) - lengthOfAxisWhereMethodIsScanningAlong;
                }
                else
                {
                    scanStartingPosition += 10;
                }
            }
            scanStartingPosition += 1;
        }
    }
    private void GenerateCorridor(ref GameObject[,] map, (int, int) startingPoint)
    {
        bool positiveWay = System.Convert.ToBoolean(Random.Range(0, 1));
        bool VerticalWay = map[startingPoint.Item1 - 1, startingPoint.Item2] != null || map[startingPoint.Item1 + 1, startingPoint.Item2] != null;
        for (int i = 1; i <= MaxCorridorLeangth; i++)
        {
            if (Random.Range(1, ChanceToEndCreationOfCorridor) == 1)
                break;
            try
            {
                //doing it this way cuz these are all possibilities
                if (VerticalWay)
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
