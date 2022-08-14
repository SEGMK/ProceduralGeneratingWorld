using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public static class Randomizeinator
{
    public static (int, int)? RandomPositionFrom2DArray(List<System.Func<object[,],int, int, bool>> conditions, object[,] map)
    {
        List<int> randomOrderedPositionsOfXAxies;
        List<int> randomOrderedPositionsOfYAxies;
        randomOrderedPositionsOfXAxies = Enumerable.Range(0, map.GetLength(0)).OrderBy(x => Random.Range(0, map.GetLength(0))).ToList();
        bool allConditionsTrue = true;
        foreach (var i in randomOrderedPositionsOfXAxies)
        {
            randomOrderedPositionsOfYAxies = Enumerable.Range(0, map.GetLength(1)).OrderBy(x => Random.Range(0, map.GetLength(1))).ToList();
            foreach (var j in randomOrderedPositionsOfYAxies)
            {
                foreach (var k in conditions)
                {
                    if (!k(map, i, j))
                        allConditionsTrue = false;
                }
                if (allConditionsTrue)
                    return (i, j);
                allConditionsTrue = true;
            }
        }
        return null;
    }
}
