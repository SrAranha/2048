using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private GameObject prefabSquare;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject gameOverWarning;
    [SerializeField] private TMP_Text text2048;
    private GridGenerator gridGenerator;
    [Header("Set on Start")]
    [SerializeField] private List<Transform> freeSquares;
    [SerializeField] private List<Transform> usedSquares;
    [SerializeField] private Vector2 coordMinMax;
    [Header("OnGoing")]
    [SerializeField] private List<Transform> movedBlocks;
    [SerializeField] private bool isGameOver;
    private bool has2048block;

    private void Start()
    {
        gridGenerator = grid.GetComponent<GridGenerator>();
        isGameOver = false;
        gameOverWarning.SetActive(false);
        has2048block = false;
        text2048.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!isGameOver)
        {
            OnReach2048Block();
            CalculateScore();

            // JUST FOR TESTINGS
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                Invoke(nameof(RandomMove), 0f);
            }
            //InvokeRepeating(nameof(RandomMove), 0f, 1f);
            // JUST FOR TESTINGS

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
    }
    #region DEBUG
    private void RandomMove()
    {
        int dirToMove = Random.Range(1, 4);
        switch (dirToMove)
        {
            case 1:
                MoveDown();
                break;
            case 2:
                MoveUp();
                break;
            case 3:
                MoveRight();
                break;
            case 4:
                MoveLeft();
                break;
        }
        CreateNewBlock();
    }
    #endregion
    private void OnReach2048Block()
    {
        if (has2048block && text2048.gameObject.activeInHierarchy == false)
        {
            text2048.gameObject.SetActive(true);
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
                Block block = currentChild.GetChild(0).GetComponent<Block>();
                if (block.currentVariation == 10)
                {
                    has2048block = true;
                }
                usedSquares.Add(currentChild);
            }
            else if (currentChild.childCount >= 1)
            {
                usedSquares.Add(currentChild);
                //Debug.LogError("<color=red>" + currentChild + " HAS " + currentChild.childCount + " CHILDS</color>", currentChild);
            }
        }
        if (freeSquares.Count == 0)
        {
            Debug.LogWarning("NO MORE SPACE FOR NEW SQUARES, CHECKING FOR GAME OVER!");
            if (IsGameOver())
            {
                isGameOver = true;
                gameOverWarning.SetActive(true);
                Debug.LogError("GAME OVER");
            }
        }
    }
    private bool IsGameOver()
    {
        Debug.LogWarning("CHECKING GAME OVER");
        bool gameover = true;
        for (int i = 0; i < usedSquares.Count; i++)
        {
            if (gameover == false)
            {
                break;
            }
            Block curBlock = usedSquares[i].GetChild(0).GetComponent<Block>();
            var nameSplited = usedSquares[i].name.Split(' ');
            int.TryParse(nameSplited[1], out int x);
            int.TryParse(nameSplited[2], out int y);
            // Check x direction
            int curX = x - 1;
            for (int j = 0; j < 3; j++)
            {
                if (curX < coordMinMax.x || curX == x - 2 || curX == x)
                {
                    curX++;
                    continue;
                }
                else if (curX == x + 2 || curX > coordMinMax.y)
                {
                    break;
                }
                else
                {
                    string nextSquareName = nameSplited[0] + ' ' + curX + ' ' + nameSplited[2];
                    Block nextBlock = GameObject.Find(nextSquareName).GetComponentInChildren<Block>();
                    if (CheckNextSquare(curBlock, nextBlock))
                    {
                        //Debug.LogWarning("<color=green>" + usedSquares[i].name + " CAN MOVE TO " + nextSquareName + "</color>", nextBlock.gameObject);
                        gameover = false;
                        break;
                    }
                    //Debug.LogWarning("<color=red>" + usedSquares[i].name + " CANT MOVE TO " + nextSquareName + "</color>", nextBlock.gameObject);
                }
                curX++;
            }
            // Check y direction
            int curY = y - 1;
            for (int k = 0; k < 3; k++)
            {
                if (curY < coordMinMax.x || curY == y - 2 || curY == y)
                {
                    curY++;
                    continue;
                }
                else if (curY == y + 2 || curY > coordMinMax.y)
                {
                    break;
                }
                else
                {
                    string nextSquareName = nameSplited[0] + ' ' + nameSplited[1] + ' ' + curY;
                    Block nextBlock = GameObject.Find(nextSquareName).GetComponentInChildren<Block>();
                    if (CheckNextSquare(curBlock, nextBlock))
                    {
                        Debug.LogWarning("<color=green>" + usedSquares[i].name + " CAN MOVE TO " + nextSquareName + "</color>", nextBlock.gameObject);
                        gameover = false;
                        break;
                    }
                    Debug.LogWarning("<color=red>" + usedSquares[i].name + " CANT MOVE TO " + nextSquareName + "</color>", nextBlock.gameObject);
                }
                curY++;
            }
        }
        return gameover;
    }
    private bool CheckNextSquare(Block curBlock, Block nextBlock)
    {
        return curBlock.currentVariation == nextBlock.currentVariation;
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
            //Debug.LogWarning(nextSquare.name + " HAS " + nextSquare.childCount + " BLOCKS", nextSquare);
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
