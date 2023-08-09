using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<Row> rows;
    
    [HideInInspector] public List<Cell> cells = new();

    public int Size => cells.Count;
    public int Height => rows.Count;
    public int Width => Size / Height;

    private void Awake()
    {
        rows.ForEach(x=>cells.AddRange(x.cells));

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
                rows[y].cells[x].SetCoordinates(new Vector2Int(x, y));
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return rows[y].cells[x];

        return null;
    }

    private Cell GetCell(Vector2Int coordinates) => GetCell(coordinates.x, coordinates.y);

    public Cell GetAdjacentCell(Cell cell, Vector2Int direction)
    {
        var tempCoordinates = cell.coordinates;
        tempCoordinates.x += direction.x;
        tempCoordinates.y -= direction.y;

        return GetCell(tempCoordinates);
    }

    public Cell GetRandomEmptyCell()
    {
        var firstEmptyCell = cells.FirstOrDefault(x => x.IsEmpty);

        if (firstEmptyCell is null)
            return null;

        var cell = cells.GetRandomItem();

        while (!cell.IsEmpty)
            cell = cells.GetRandomItem();

        return cell;
    }
}