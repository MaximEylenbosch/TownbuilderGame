using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct Tile
{
    public GameObject Prefab;
    public TileNeighbours Neighbours;
    public float PlacePercentage;
}

[Serializable]
public struct TileNeighbours
{
    public TileNeighbour[] LeftNeighbours;
    public TileNeighbour[] RightNeighbours;
    public TileNeighbour[] TopNeighbours;
    public TileNeighbour[] BottomNeighbours;
}

[Serializable]
public struct TileNeighbour
{
    public GameObject Prefab;
    public float PlacePercentage;
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(1, 1);
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private Tile _godTile;

    [SerializeField]
    private MyTile test = null;

    private Tile[,] _tileGrid;

    private void GenerateMap()
    {
        int rows = _gridSize.y;
        int columns = _gridSize.x;

        _tileGrid = new Tile[rows, columns];

        Vector3 nextTilePos = Vector3.zero;

        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            nextTilePos = new Vector3(0, 0, -2 * rowIndex);
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                Tile tile = SelectTile(rowIndex, columnIndex);
                Instantiate(tile.Prefab, nextTilePos, Quaternion.identity, this.transform);
                _tileGrid[rowIndex, columnIndex] = tile;

                nextTilePos += new Vector3(2, 0, 0);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);

            GenerateMap();
        }
    }

    private Tile SelectTile(int rowIndex, int columnIndex)
    {
        Tile leftNeighbour;
        Tile topNeighbour;

        if (rowIndex != 0)
            topNeighbour = _tileGrid[rowIndex - 1, columnIndex];
        else
            topNeighbour = _godTile;

        if (columnIndex != 0)
            leftNeighbour = _tileGrid[rowIndex, columnIndex - 1];
        else
            leftNeighbour = _godTile;

        WeightedChanceExecutor weightedChanceExecutor = new WeightedChanceExecutor(
            _tiles[0], _tiles[1], _tiles[2]
            );

        return weightedChanceExecutor.Execute();
    }
}

public class WeightedChanceExecutor
{
    public Tile[] Tiles { get; }
    private System.Random r;

    private float _ratioSum = 0;

    public WeightedChanceExecutor(params Tile[] parameters)
    {
        Tiles = parameters;
        r = new System.Random();

        for (int i = 0; i < parameters.Length; i++)
        {
            _ratioSum += parameters[i].PlacePercentage;
        }
    }

    public Tile Execute()
    {
        double numericValue = r.NextDouble() * _ratioSum;

        foreach (var tile in Tiles)
        {
            numericValue -= tile.PlacePercentage;
            Debug.Log(numericValue);

            if (!(numericValue <= 0))
                continue;

            return tile;
        }

        return new Tile();
    }
}

[Serializable]
public class MyTile : NewCustomRuleTile
{
    public string tileId;
    public bool isWater;
}
