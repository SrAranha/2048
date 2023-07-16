using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class SavedDataManager : Singleton<SavedDataManager>
{
    public SO_SavedData savedData;

    protected override void Awake()
    {
        base.Awake();
    }
    [NaughtyAttributes.Button]
    public void ResetALLSavedData()
    {
        // GAMEPLAY
        savedData.highscoreX4 = 0;
        savedData.highscoreX5 = 0;
        savedData.highscoreX6 = 0;
        savedData.gridSize = GameSettings.GridSizes.x5;
        // LANGUAGE
        savedData.language = GameSettings.Language.ENGLISH;
        // AUDIO
        savedData.masterVolume = 80;

        Debug.Log("Restored " + savedData.name + " to default condition.", savedData);
    }
    public void ResetHighScore()
    {
        savedData.highscoreX4 = 0;
        savedData.highscoreX5 = 0;
        savedData.highscoreX6 = 0;
    }
    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
        // Then set the saved language from saved data
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)savedData.language];
    }
}
