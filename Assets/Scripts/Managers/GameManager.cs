using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Board board;
    
    private int maxReachedNumber = 4;

    public int MaxReachedNumber
    {
        get => maxReachedNumber;
        set
        {
            if (value == 0)
                maxReachedNumber = 4;
            else
                maxReachedNumber *= 2;
        }
    }

    private void Start() => StartCoroutine(NewGame());

    private IEnumerator NewGame()
    {
        yield return new WaitForSeconds(1f);
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        StartCoroutine(NewGame());
    }
}