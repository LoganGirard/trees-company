using Assets.Playables;
using System;
using UnityEngine;

public class StateCalculator : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject housePrefab;
    public  GameObject floorPrefab;
    public GameObject powerPrefab;

     void GetNeighbors(GameObject[,] grid, int x, int y, out int treeCount, out int houseCount, out int powerHouseCount)
    {
        treeCount = 0;
        houseCount = 0;
        powerHouseCount = 0;

        for(int tmpX = Math.Max(x - 1, 0); tmpX < grid.GetLength(1) && tmpX <= x + 1; tmpX++)
        {
            for (int tmpY = Math.Max(y - 1, 0); tmpY < grid.GetLength(0) && tmpY <= y + 1; tmpY++)
            {
                if (tmpX == x && tmpY == y) continue;


                var name = grid[tmpY, tmpX].name;
                if(name.Contains("Tree"))
                {
                    treeCount++;

                }
                else if (name.Contains("House") && !name.Contains("Power"))
                {
                    houseCount++;
                }
                else if(name.Contains("Power"))
                {
                    powerHouseCount++;

                }
            }           
        }
    }

     public GameObject[,] CalculateNextState(GameObject[,] originalGrid)
    {
        Debug.Log("Stating, baby!");
        var nextState = new GameObject[originalGrid.GetLength(0), originalGrid.GetLength(1)];

        for (int x = 0; x < nextState.GetLength(1); x++)
        {
            for (int y = 0; y < nextState.GetLength(0); y++)
            {
                var cur = originalGrid[y, x];

                GetNeighbors(originalGrid, x, y, out int treeCount, out int houseCount, out int powerHousecount);

                Debug.Log($"cur.name counts: t: {treeCount}, h {houseCount}, ph {powerHousecount}");

                if (IsEmpty(cur))
                {
                    // Generate new stuff.
                    
                    if (treeCount - houseCount*1.5 >= 0 && treeCount >=2)
                    {
                        nextState[y, x] = Instantiate(housePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }
                    else if (treeCount >= 2 || UnityEngine.Random.Range(0f, 1f) > 0.95f)
                    {
                            nextState[y, x] = Instantiate(treePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }
                    else
                    {
                        nextState[y, x] = Instantiate(floorPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    };
                }
                //// Potentially kill things.
                else if (IsTree(cur))
                {
                    if (houseCount >= 3 || treeCount >= 5)
                    {
                        nextState[y, x] = Instantiate(floorPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }
                    else
                    {
                        nextState[y, x] = Instantiate(treePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }
                }
                else if (IsHouse(cur))
                {
                    if(treeCount - houseCount*1.5 <= 0)
                    {
                        nextState[y, x] = Instantiate(floorPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }
                    else
                    {
                        nextState[y, x] = Instantiate(housePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                    }  
                }
                else if(IsPower(cur))
                {
                    nextState[y, x] = Instantiate(powerPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                }
                else
                {
                    nextState[y, x] = Instantiate(floorPrefab, new Vector3(x, y, 0f), Quaternion.identity);
                }

                nextState[y, x].GetComponent<PlayableBase>().X = x;
                nextState[y, x].GetComponent<PlayableBase>().Y = y;
            }
        }

        return nextState;
    }

     bool IsEmpty(GameObject g)
    {
        return g.name.Contains("Floor");
    }

     bool IsTree(GameObject g)
    {
        return g.name.Contains("Tree");
    }

     bool IsPower(GameObject g)
    {
        return g.name.Contains("Power");
    }

     bool IsHouse(GameObject g)
    {
        return g.name.Contains("House") && !g.name.Contains("Power");
    }
}