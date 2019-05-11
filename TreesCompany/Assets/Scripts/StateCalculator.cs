using Assets.Playables;
using UnityEngine;
using Tree = Assets.Playables.Tree;

public class StateCalculator : MonoBehaviour
{
    void GetNeighbors(GameObject[,] grid, int x, int y, out int treeCount, out int houseCount, out int powerHouseCount)
    {
        treeCount = 0;
        houseCount = 0;
        powerHouseCount = 0;

        for(int tmpX = x - 1; tmpX < grid.GetLength(1) && tmpX <= x + 1; tmpX++)
        {
            for (int tmpY = y - 1; tmpY < grid.GetLength(0) && tmpY <= y + 1; tmpY++)
            {
                if (tmpX == x && tmpY == y) continue;

                switch (grid[tmpY, tmpX].GetComponent<PlayableBase>())
                {
                    case Assets.Playables.Tree thing:
                        treeCount++;
                        break;
                    case House thing:
                        houseCount++;
                        break;
                    case PowerHouse thing:
                        powerHouseCount++;
                        break;
                }
            }           
        }
    }

    public GameObject[,] CalculateNextState(GameObject[,] originalGrid)
    {
        var nextState = new GameObject[originalGrid.GetLength(0), originalGrid.GetLength(1)];

        for (int x = 0; x < nextState.GetLength(1); x++)
        {
            for (int y = 0; y < nextState.GetLength(0); y++)
            {
                var cur = originalGrid[y, x];
                GetNeighbors(originalGrid, x, y, out int treeCount, out int houseCount, out int powerHousecount);

                nextState[y, x] = originalGrid[y, x];

                if (IsEmpty(cur))
                {
                    // Generate new stuff.
                    if (treeCount == 3)
                    {
                        nextState[y, x].AddComponent(typeof(Tree));
                    }
                    else if (houseCount < 5 && powerHousecount >= 1)
                    {
                        nextState[y, x].AddComponent(typeof(House));
                    }
                    continue;
                }
                // Potentially kill things.
                else if (IsTree(cur))
                {
                    if (treeCount >= 5 || houseCount >= 5 || powerHousecount >= 2)
                    {
                        Destroy(nextState[y, x].GetComponent<Tree>());
                    }
                }
                //else if (IsHouse(cur) && treeCount > 0 && powerHousecount > 0)
                //{
                //    nextState[y, x].AddComponent(typeof(Hou))
                //}
            }
        }

        return nextState;
    }

    bool IsEmpty(GameObject g)
    {
        return g.GetComponents<PlayableBase>().Length == 0;
    }

    bool IsTree(GameObject g)
    {
        return g.GetComponent<Assets.Playables.Tree>() != null;
    }

    bool IsPower(GameObject g)
    {
        return g.GetComponent<PowerHouse>() != null;
    }

    bool IsHouse(GameObject g)
    {
        return g.GetComponent<House>() != null;
    }
}