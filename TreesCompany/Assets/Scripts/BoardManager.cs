using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    // TODO: Create quad prefabs for game pieces
    // TODO: Rotate whole board 90

    public static int columns = 13;
    public static int rows = 13;
    
    public static GameObject[,] gridGameObjects = new GameObject[rows, columns];    // Game Objects in the grid

    public GameObject[] PowerPlantTiles;        // Array of power plant prefabs
    public GameObject[] TreeTiles;              // Array of tree prefabs
    public GameObject[] HouseTiles;             // Array of house prefabs
    public GameObject[] FloorTiles;             // Array of floor prefabs

    public Count TreeCount = new Count(5, 9);   //Lower and upper limit for our random number of walls per level.
    public Count HouseCount = new Count(1, 5);   //Lower and upper limit for our random number of food items per level.

    private Transform boardHolder;                               //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.
    

    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Sets up the outer walls and floor(background) of the game board.
    void BoardSetup ()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis. Start from 1 so you don't have anything at the edges.
        for (int x = 0; x < columns; x++)
        {
            //Loop along y axis. Start from 1 so you don't have anything at the edges.
            for (int y = 0; y < rows; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);

                // Add the GameObject to the data structure
                gridGameObjects[x, y] = instance;
            }
        }
    }

    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene()
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(TreeTiles, TreeCount.minimum, TreeCount.maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(HouseTiles, HouseCount.minimum, HouseCount.maximum);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    // Update is called once per frame
    void Update()
    {
        var tmpGrid = new StateCalculator().CalculateNextState(gridGameObjects);
        foreach (GameObject g in gridGameObjects)
        {
            Destroy(g);
        }

        gridGameObjects = tmpGrid;
    }
}
