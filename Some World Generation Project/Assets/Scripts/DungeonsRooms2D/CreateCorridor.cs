using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "CreateCorridor", menuName = "RoomsCreatingClasses/CreateCorridor")]
public class CreateCorridor : ScriptableObject
{
    [SerializeField] private GameObject[] CorridorTiles;
    [SerializeField] private GameObject CorridorTileVertical;
    [SerializeField] private GameObject CorridorTileHorizontal;
    [SerializeField] private int MaxCorridorLeangth = 20;
    [SerializeField] private int ChanceToEndCreationOfCorridor = 20;
    [SerializeField] private int NumberOfCorridors = 30;
    [SerializeField] private int ChanceToCreateCorridorHorizontal = 4;
    [SerializeField] private int MinLenghtOfCorridor = 5;
    public GameObject[,] CreateCorridorMap(GameObject[,] map)
    {
        ((int, int), bool) startingPointAndDirection = ((map.GetLength(0) / 2, map.GetLength(1) / 2), true);
        for (int i = 0; i < NumberOfCorridors; i++)
        {
            map = GenerateCorridor(map, startingPointAndDirection.Item1, startingPointAndDirection.Item2);
            startingPointAndDirection = FindNewStartingPosition(map);
        }
        return map;
    }
    private ((int, int), bool) FindNewStartingPosition(GameObject[,] map)
    {
        if (Random.Range(1, ChanceToCreateCorridorHorizontal) == 1)
            return (ScanMapAxiesToFindNewPosition(map, true), true);
        else
            return (ScanMapAxiesToFindNewPosition(map, false), false);
    }
    private (int, int) ScanMapAxiesToFindNewPosition(GameObject[,] map, bool horizontal)
    {
        (int, int)? position = (0, 0);
        List<System.Func<object[,], int, int, bool>> list = new List<System.Func<object[,], int, int, bool>>();
        if (horizontal)
        {
            list.Add((object[,] map, int i, int j) => { return map[j, i] != null; });
            position = Randomizeinator.RandomPositionFrom2DArray(list, map);
            if (position != null)
            {
                return (position.Value.Item2, position.Value.Item1);
            }
        }
        else
        {
            list.Add((object[,] map, int i, int j) => { return map[i, j] != null; });
            position = Randomizeinator.RandomPositionFrom2DArray(list, map);
            if (position != null)
            {
                return position.Value;
            }
        }
        return ScanMapAxiesToFindNewPosition(map, !horizontal);
    }
    private GameObject[,] GenerateCorridor(GameObject[,] map, (int, int) position, bool horizontal)
    {
        GameObject[,] newMap = new GameObject[map.GetLength(0),map.GetLength(1)];
        GameObject tile = null;
        System.Array.Copy(map, 0, newMap, 0, map.Length);
        (int, int) newPosition = position;
        int loopInitialValue = 0;
        string option;
        option = DefineOption(horizontal, ref tile, System.Convert.ToBoolean(Random.Range(0, 2)));

        for (int i = loopInitialValue; i < MaxCorridorLeangth; i++)
        {
            if (Random.Range(1, ChanceToEndCreationOfCorridor) == 1 && MinLenghtOfCorridor <= i)
                break;
            try
            {
                switch (option)
                {
                    case "XP":
                        newPosition = (position.Item1 + i, position.Item2);
                        CreateCorridorOnSpecifiedAxies(map, ref newMap, ref tile, newPosition, ref option, ref position, ref horizontal, ref i, loopInitialValue);
                        break;
                    case "XN":
                        newPosition = (position.Item1 - i, position.Item2);
                        CreateCorridorOnSpecifiedAxies(map, ref newMap, ref tile, newPosition, ref option, ref position, ref horizontal, ref i, loopInitialValue);
                        break;
                    case "YP":
                        newPosition = (position.Item1, position.Item2 + i);
                        CreateCorridorOnSpecifiedAxies(map, ref newMap, ref tile, newPosition, ref option, ref position, ref horizontal, ref i, loopInitialValue);
                        break;
                    case "YN":
                        newPosition = (position.Item1, position.Item2 - i);
                        CreateCorridorOnSpecifiedAxies(map, ref newMap, ref tile, newPosition, ref option, ref position, ref horizontal, ref i, loopInitialValue);
                        break;
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                if (MinLenghtOfCorridor <= i)
                    break;
                else
                    ResetGenerateCorridorMethod(ref newMap, map, ref tile, ref position, ref horizontal, ref i, loopInitialValue, ref option);
            }
        }
        return newMap;
    }
    private void CreateCorridorOnSpecifiedAxies(GameObject[,] map, ref GameObject[,] newMap, ref GameObject tile, (int, int) newPosition, ref string option,
        ref (int, int) position, ref bool horizontal, ref int i, int loopInitialValue)
    {
        if (!(IsNotOverlapping(map, (newPosition.Item1, newPosition.Item2), option, tile) && IsNotNearToAnotherCorridor(map, newPosition, tile)))
        {
            ResetGenerateCorridorMethod(ref newMap, map, ref tile, ref position, ref horizontal, ref i, loopInitialValue, ref option);
            return;
        }
        newMap[newPosition.Item1, newPosition.Item2] = tile;
    }
    private string DefineOption(bool horizontal, ref GameObject tile, bool positiveWay)
    {
        string option = "";
        if (!horizontal)
        {
            option += "X";
            tile = CorridorTileHorizontal;
        }
        else
        {
            option += "Y";
            tile = CorridorTileVertical;
        }
        option += positiveWay ? "P" : "N";
        return option;
    }
    private bool IsNotNearToAnotherCorridor(GameObject[,] map, (int, int) position, GameObject tile)
    {
        if (map[position.Item1 + 1, position.Item2 + 1] == tile || map[position.Item1 + 1, position.Item2 - 1] == tile ||
            map[position.Item1 - 1, position.Item2 + 1] == tile || map[position.Item1 - 1, position.Item2 - 1] == tile)
            return false;
        return true;
    }
    private bool IsNotOverlapping(GameObject[,] map, (int, int) position, string option, GameObject tile)
    {
        if (map[position.Item1, position.Item2] == tile)
            return false;
        return true;
    }
    private void ResetGenerateCorridorMethod(ref GameObject[,] newMap, GameObject[,] map, ref GameObject tile, ref (int, int) position, ref bool direction, ref int iterableVariable,
        int loopInitialValue, ref string option)
    {
        newMap = map;
        var positionAndDirection = FindNewStartingPosition(map);
        position = positionAndDirection.Item1;
        direction = positionAndDirection.Item2;
        tile = direction ? CorridorTileVertical : CorridorTileHorizontal;
        option = DefineOption(direction, ref tile, System.Convert.ToBoolean(Random.Range(0, 2)));
        iterableVariable = loopInitialValue - 1;
    }
}
