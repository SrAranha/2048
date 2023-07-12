using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SO_SavedData")]
public class SO_SavedData : ScriptableObject
{
    public int highscore;
    public GameSettings.GridSizes gridSize;
    public GameSettings.Language language;
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