using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector] public Vector2Int coordinates;
    [HideInInspector] public Tile tile;
    public bool IsEmpty => tile == null;

    public void SetCoordinates(Vector2Int newCoordinates) => coordinates = newCoordinates;
}