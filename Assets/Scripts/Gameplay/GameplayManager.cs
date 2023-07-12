using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private GameObject prefabSquare;
    [SerializeField] private GameObject grid;
    private GridGenerator gridGenerator;
    [Header("Set on Start")]
    [SerializeField] private List<Transform> freeSquares;
    [SerializeField] private List<Transform> usedSquares;
    [SerializeField] private Vector2 coordMinMax;
    [SerializeField] private List<Transform> movedBlocks;

    private void Start()
    {
        gridGenerator = grid.GetComponent<GridGenerator>();
    }
    private void Update()
    {
        CalculateScore();

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
            CreateNewBlock();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
            CreateNewBlock();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
            CreateNewBlock();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
            CreateNewBlock();
        }
    }
    public void StartGame()
    {
        coordMinMax = new Vector2(1, gridGenerator.actualGridSize);
        GetFreeSquares();
        CreateNewBlock();
    }
    [NaughtyAttributes.Button]
    public void ResetGame()
    {
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Transform currentChild = grid.transform.GetChild(i);
            Destroy(currentChild.gameObject);
        }
        GetFreeSquares();
        // Workaround fot this not working as intended;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void GetFreeSquares()
    {
        freeSquares.Clear();
        usedSquares.Clear();
        movedBlocks.Clear();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Transform currentChild = grid.transform.GetChild(i);
            if (currentChild.childCount == 0)
            {
                freeSquares.Add(currentChild);
            }
            else if (currentChild.childCount == 1)
            {
                usedSquares.Add(currentChild);
            }
            else if (currentChild.childCount >= 1)
            {
                usedSquares.Add(currentChild);
                Debug.LogError("<color=red>" + currentChild + " HAS " + currentChild.childCount + " CHILDS</color>", currentChild);
            }
        }
        if (freeSquares.Count == 0)
        {
            Debug.LogWarning("NO MORE SPACE FOR NEW SQUARES");
        }
    }
    private void CalculateScore()
    {
        GetFreeSquares();
        int score = 0;
        foreach (Transform square in usedSquares)
        {
            Block currentBlock = square.GetChild(0).GetComponent<Block>();
            int nextScore = currentBlock.setups[currentBlock.currentVariation].number;
            score += nextScore;
        }
        ScoreManager.instance.SetScore(score);
    }
    [NaughtyAttributes.Button]
    private void CreateNewBlock()
    {
        GetFreeSquares();
        int randomSquare = Random.Range(0, freeSquares.Count);
        Instantiate(prefabSquare, freeSquares[randomSquare]);
    }
    #region MOVING SQUARES
    private void GetCurrentSquare(Transform gridBG, out Transform block, out string[] nameSplited, out Vector2 cord)
    {
        var square = gridBG;
        block = square.GetChild(0);
        nameSplited = square.name.Split(' ');
        int.TryParse(nameSplited[1], out int x);
        int.TryParse(nameSplited[2], out int y);
        cord = new(x, y);
    }
    private void MoveBlock(Transform nextSquare, Transform block, out bool canMove)
    {
        canMove = false;
        if (nextSquare.childCount == 0)
        {
            block.SetParent(nextSquare, false);
            canMove = true;
        }
        else if (nextSquare.childCount == 1)
        {
            Block blockComp = block.GetComponent<Block>();
            Block nextBlockComp = nextSquare.GetChild(0).GetComponent<Block>();

            if (blockComp.currentVariation == nextBlockComp.currentVariation)
            {
                blockComp.MergeBlocks(nextBlockComp);
                block.SetParent(nextSquare, false);
                canMove = true;
            }
            else if (blockComp.currentVariation != nextBlockComp.currentVariation)
            {
                canMove = false;
            }
        }
        else if (nextSquare.childCount > 1)
        {
            Debug.LogWarning(nextSquare.name + " HAS " + nextSquare.childCount + " BLOCKS", nextSquare);
            canMove = false;
        }
    }
    private void MoveDown() // vvvv
    {
        movedBlocks.Clear();
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            var currentGridBG = grid.transform.GetChild(i);
            if (currentGridBG.childCount > 0)
            {
                GetCurrentSquare(currentGridBG, out Transform block, out string[] nameSplited, out Vector2 cord);
                if (!movedBlocks.Contains(block))
                {
                    while (cord.y < coordMinMax.y)
                    {
                        string nextSquareName = nameSplited[0] + ' ' + nameSplited[1] + ' ' + ++cord.y;
                        Transform nextSquare = GameObject.Find(nextSquareName).transform;
                        MoveBlock(nextSquare, block, out bool canMove);
                        if (canMove) { continue; }
                        else if (!canMove) { break; }
                    }
                    movedBlocks.Add(block);
                }
            }
        }
    }
    private void MoveUp() // ^^^^
    {
        movedBlocks.Clear();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            var currentGridBG = grid.transform.GetChild(i);
            if (currentGridBG.childCount > 0)
            {
                GetCurrentSquare(currentGridBG, out Transform block, out string[] nameSplited, out Vector2 cord);
                if (!movedBlocks.Contains(block))
                {
                    while (cord.y > coordMinMax.x)
                    {
                        string nextSquareName = nameSplited[0] + ' ' + nameSplited[1] + ' ' + --cord.y;
                        Transform nextSquare = GameObject.Find(nextSquareName).transform;
                        MoveBlock(nextSquare, block, out bool canMove);
                        if (canMove) { continue; }
                        else if (!canMove) { break; }
                    }
                    movedBlocks.Add(block);
                }
            }
        }
    }
    private void MoveRight() // >>>> 
    {
        movedBlocks.Clear();
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            var currentGridBG = grid.transform.GetChild(i);
            if (currentGridBG.childCount > 0)
            {
                GetCurrentSquare(currentGridBG, out Transform block, out string[] nameSplited, out Vector2 cord);
                if (!movedBlocks.Contains(block))
                {
                    while (cord.x < coordMinMax.y)
                    {
                        string nextSquareName = nameSplited[0] + ' ' + ++cord.x + ' ' + nameSplited[2];
                        Transform nextSquare = GameObject.Find(nextSquareName).transform;
                        MoveBlock(nextSquare, block, out bool canMove);
                        if (canMove) { continue; }
                        else if (!canMove) { break; }
                    }
                    movedBlocks.Add(block);
                }
            }
        }
    }
    private void MoveLeft() // <<<<
    {
        movedBlocks.Clear();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            var currentGridBG = grid.transform.GetChild(i);
            if (currentGridBG.childCount > 0)
            {
                GetCurrentSquare(currentGridBG, out Transform block, out string[] nameSplited, out Vector2 cord);
                if (!movedBlocks.Contains(block))
                {
                    while (cord.x > coordMinMax.x)
                    {
                        string nextSquareName = nameSplited[0] + ' ' + --cord.x + ' ' + nameSplited[2];
                        Transform nextSquare = GameObject.Find(nextSquareName).transform;
                        MoveBlock(nextSquare, block, out bool canMove);
                        if (canMove) { continue; }
                        else if (!canMove) { break; }
                    }
                    movedBlocks.Add(block);
                }
            }
        }
    }
    #endregion
}
