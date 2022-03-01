using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Cell
{
    [SerializeField]
    public bool isBlocked;
    [SerializeField]
    public GameObject prefab;

}

public class GridManager : MonoBehaviour
{

    private int gridWidth;
    private int gridBreadth;
    private int cellPadding;
    private Vector3 gridPosition;
    [SerializeField]
    private Cell[][] grid;
    [SerializeField]
    private GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        if (grid[0].Length == 0)
        {
            Debug.Log("Invalid Array");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // @brief Set the grid's dimensions along the x and z axis.
    public void SetGridDimensions(int width, int breadth)
    {
        gridWidth   = width;
        gridBreadth = breadth;
    }

    // @brief Set the starting position of the grid.
    public void SetGridPosition(Vector3 position)
    {
        gridPosition = position;
    }

    // @brief Set the space in between each cell.
    public void SetCellPadding(int padding)
    {
        cellPadding = padding;
    }

    // @brief Generate a new grid.
    public void GenerateNewGrid(int width, int breadth, GameObject prefab, Vector3 position, int padding = 1)
    {
        if(root != null)
        {
            DestroyImmediate(root);
        }

        if(grid != null)
        {
            for(int i = 0; i < grid.Length; ++i)
            {
                for(int j = 0; j < grid[i].Length; ++j)
                {
                    DestroyImmediate(grid[i][j].prefab);
                    grid[i][j].isBlocked = false;
                }
            }
        }

        //Declare new grid.
        grid = new Cell[gridWidth][];

        for (int x = 0; x < gridWidth; ++x)
        {
            grid[x] = new Cell[gridBreadth];

            for (int z = 0; z < gridBreadth; ++z)
            {
                //The current cell's world position.
                Vector3 cellPosition = new Vector3(x * cellPadding, 0.0f, z * cellPadding);

                cellPosition += gridPosition;

                grid[x][z].isBlocked = false;
                grid[x][z].prefab = Instantiate(prefab, cellPosition, Quaternion.identity);

            }

        }
    }

    public bool IsOccupied(int rowIndex, int columnIndex)
    {
        bool isOccupied = false;
        if(grid[rowIndex][columnIndex].isBlocked)
        {
            isOccupied = true; 
        }

        return isOccupied;
    }

}
