using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefabSquareBackground;

    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameSettings.GridSizes gridSizeType;
    public int actualGridSize;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gridSizeType = SavedDataManager.instance.savedData.gridSize;
        // EASING THE SIZE OF THE GRID
        switch (gridSizeType)
        {
            case GameSettings.GridSizes.x4:
                actualGridSize = 4;
                break;
            case GameSettings.GridSizes.x5:
                actualGridSize = 5;
                break;
            case GameSettings.GridSizes.x6:
                actualGridSize = 6;
                break;
        }

        SetGridLayout();
    }
    private void SetGridLayout()
    {
        grid.constraintCount = actualGridSize;
        switch (gridSizeType)
        {
            case GameSettings.GridSizes.x4:
                grid.cellSize = new Vector2(100, 100);
                break;
            case GameSettings.GridSizes.x5:
                grid.cellSize = new Vector2(80, 80);
                break;
            case GameSettings.GridSizes.x6:
                grid.cellSize = new Vector2(60, 60);
                break;
        }
        GenerateGridBackground();
    }
    private void GenerateGridBackground()
    {
        // Actual Generate
        for (int y = 1; y <= actualGridSize; y++)
        {
            for (int x = 1; x <= actualGridSize; x++)
            {
                GameObject square = Instantiate(prefabSquareBackground, gameObject.transform);
                square.name = "GridBG " + x + ' ' + y;
            }
        }
        GameplayManager.instance.StartGame();
    }
}
