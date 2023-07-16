using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO_SavedData")]
public class SO_SavedData : ScriptableObject
{
    [Header("Game Settings")]
    public GameSettings.GridSizes gridSize;
    public GameSettings.Language language;
    [Header("Highscores")]
    public int highscoreX4;
    public int highscoreX5;
    public int highscoreX6;
    [Header("Audio")]
    [Range(0,100)]
    public float masterVolume;
}
public class GameSettings
{
    public enum Language
    {
        ENGLISH,
        PORTUGUESE
    }
    public enum GridSizes
    {
        x4,
        x5,
        x6
    }
}