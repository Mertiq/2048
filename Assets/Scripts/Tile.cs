using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private TMP_Text text;

    [HideInInspector] public TileData data;
    [HideInInspector] public Cell cell;
    [HideInInspector] public int number;
    [HideInInspector] public bool isLocked;

    private bool isMoving;

    public void SetTile(TileData data, int number = 0)
    {
        this.data = data;

        this.number = number == 0 ? int.Parse(data.name) : number;
        bgImage.color = data.bgColor;
        text.color = data.textColor;
        text.text = this.number.ToString();
    }

    private void SetCell(Cell cell)
    {
        if (this.cell is not null)
            this.cell.tile = null;
        this.cell = cell;
        this.cell.tile = this;
    }

    public void Spawn(Cell cell)
    {
        SetCell(cell);
        transform.position = cell.transform.position;
    }

    public void Move(Cell cell)
    {
        if (isMoving)
            return;

        SetCell(cell);
        StartCoroutine(MovementAnimation(cell.transform.position));
    }

    public void Merge(Cell adjacent)
    {
        if (cell is not null)
            cell.tile = null;

        cell = null;
        adjacent.tile.isLocked = true;

        StartCoroutine(MovementAnimation(adjacent.transform.position, true));
    }

    private IEnumerator MovementAnimation(Vector3 targetPosition, bool isMerging = false)
    {
        isMoving = true;

        const float duration = 0.1f;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

        if (isMerging)
            Destroy(gameObject);
    }
}