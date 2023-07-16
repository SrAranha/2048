using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int currentScore;
    [SerializeField] private TMP_Text currentScoreCounter;
    [SerializeField] private TMP_Text highscoreCounter;
    private SO_SavedData savedData;
    private int currentHighscore;
    private void Start()
    {
        savedData = SavedDataManager.instance.savedData;
        HighscoreBasedOnGridSize(out int highscore);
        currentHighscore = highscore;
        highscoreCounter.text = currentHighscore.ToString();
    }
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreCounter();
    }
    private void UpdateScoreCounter()
    {
        currentScoreCounter.text = currentScore.ToString();

        if (currentScore > currentHighscore)
        {
            UpdateHighscore();
        }
    }
    private void UpdateHighscore()
    {
        currentHighscore = currentScore;
        highscoreCounter.text = currentHighscore.ToString();
        switch (savedData.gridSize)
        {
            case GameSettings.GridSizes.x4:
                savedData.highscoreX4 = currentHighscore;
                break;
            case GameSettings.GridSizes.x5:
                savedData.highscoreX5 = currentHighscore;
                break;
            case GameSettings.GridSizes.x6:
                savedData.highscoreX6 = currentHighscore;
                break;
        }
    }
    private void HighscoreBasedOnGridSize(out int highscore)
    {
        highscore = currentHighscore;
        switch (savedData.gridSize)
        {
            case GameSettings.GridSizes.x4:
                highscore = savedData.highscoreX4;
                break;
            case GameSettings.GridSizes.x5:
                highscore = savedData.highscoreX5;
                break;
            case GameSettings.GridSizes.x6:
                highscore = savedData.highscoreX6;
                break;
        }
    }
    #region DEBUG
    [NaughtyAttributes.Button]
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreCounter();
    }
    [NaughtyAttributes.Button]
    private void AddRandomScore()
    {
        SetScore(Random.Range(1,101));
    }
    public void ResetHighscore()
    {
        highscoreCounter.text = 0.ToString();
    }
    #endregion
}
