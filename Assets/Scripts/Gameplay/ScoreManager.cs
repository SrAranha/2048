using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int currentScore;
    [SerializeField] private TMP_Text currentScoreCounter;
    [SerializeField] private TMP_Text highscoreCounter;
    private SO_SavedData savedData;

    private void Start()
    {
        savedData = SavedDataManager.instance.savedData;
        highscoreCounter.text = savedData.highscore.ToString();
    }
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreCounter();
    }
    private void UpdateScoreCounter()
    {
        currentScoreCounter.text = currentScore.ToString();

        if (currentScore > savedData.highscore)
        {
            UpdateHighscore();
        }
    }
    private void UpdateHighscore()
    {
        savedData.highscore = currentScore;
        highscoreCounter.text = currentScore.ToString();
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
