using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<TileData> tileDatas;

    private readonly List<Tile> tiles = new();
    private bool IsEmptyCellExist => tiles.Count != grid.Size;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            MoveTiles(new MovingParams(Vector2Int.up, 0, 1, 1, 1));
        if (Input.GetKeyDown(KeyCode.A))
            MoveTiles(new MovingParams(Vector2Int.left, 1, 1, 0, 1));
        if (Input.GetKeyDown(KeyCode.S))
            MoveTiles(new MovingParams(Vector2Int.down, 0, 1, grid.Height - 2, -1));
        if (Input.GetKeyDown(KeyCode.D))
            MoveTiles(new MovingParams(Vector2Int.right, grid.Width - 2, -1, 0, 1));
    }

    private void MoveTiles(MovingParams movingParams)
    {
        for (var x = movingParams.startX; x >= 0 && x < grid.Width; x += movingParams.incrementX)
        {
            for (var y = movingParams.startY; y >= 0 && y < grid.Height; y += movingParams.incrementY)
            {
                var cell = grid.GetCell(x, y);

                if (!cell.IsEmpty)
                    MoveTile(cell.tile, movingParams.direction);
            }
        }

        CreateTile();

        tiles.ForEach(tile => tile.isLocked = false);

        if (GameOverCheck())
            GameManager.Instance.GameOver();
    }

    private void MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        var adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent is not null)
        {
            if (!adjacent.IsEmpty)
            {
                if (CanMerge(tile, adjacent.tile))
                    Merge(tile, adjacent.tile);

                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell is not null)
            tile.Move(newCell);
    }

    private static bool CanMerge(Tile tile, Tile adjacent) => tile.number == adjacent.number && !adjacent.isLocked;

    private void Merge(Tile tile, Tile adjacent)
    {
        tiles.Remove(tile);
        tile.Merge(adjacent.cell);

        var index = Mathf.Clamp(tileDatas.IndexOf(adjacent.data) + 1, 0, tiles.Count - 1);
        var number = adjacent.number * 2;
        adjacent.SetTile(tileDatas[index], number);
    }

    public void CreateTile()
    {
        if (!IsEmptyCellExist) return;

        var tile = Instantiate(tilePrefab, grid.transform);

        var data = tileDatas.GetRandomItem(tileDatas.IndexOf(tileDatas.FirstOrDefault(x =>
            x.name.Equals(GameManager.Instance.MaxReachedNumber++.ToString()))));

        tile.SetTile(data);

        var cell = grid.GetRandomEmptyCell();

        if (cell != null)
            tile.Spawn(cell);
        else
            Debug.LogError("There is no empty CELL");

        tiles.Add(tile);
    }

    private bool GameOverCheck()
    {
        if (tiles.Count != grid.Size) return false;

        foreach (var tile in tiles)
        {
            var up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            var down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            var left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            var right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
                return false;

            if (down != null && CanMerge(tile, down.tile))
                return false;

            if (left != null && CanMerge(tile, left.tile))
                return false;

            if (right != null && CanMerge(tile, right.tile))
                return false;
        }

        return true;
    }

    public void ClearBoard()
    {
        grid.cells.ForEach(x => x.tile = null);
        tiles.ForEach(x => Destroy(x.gameObject));
        tiles.Clear();
    }
}

public class MovingParams
{
    public Vector2Int direction;
    public int startX;
    public int incrementX;
    public int startY;
    public int incrementY;

    public MovingParams(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        this.direction = direction;
        this.startX = startX;
        this.incrementX = incrementX;
        this.startY = startY;
        this.incrementY = incrementY;
    }
}