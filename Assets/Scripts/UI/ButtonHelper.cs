using TMPro;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Settings;

public class ButtonHelper : MonoBehaviour
{
    [Header("Reset Highscore")]
    [SerializeField] private GameObject warning;
    [SerializeField] private TMP_Text resetText;
    [SerializeField] private float timeToFade;
    [Header("Dropdown Settings")]
    [SerializeField] private DropdownType dropdownType;
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        if (dropdown)
        {
            switch (dropdownType)
            {
                case DropdownType.GRIDZISE:
                    dropdown.value = (int)SavedDataManager.instance.savedData.gridSize;
                    break;
                case DropdownType.LANGUAGE:
                    dropdown.value = (int)SavedDataManager.instance.savedData.language;
                    break;
            }
        }
    }
    #region ChangeScenes
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ChangeScene(string sceneString)
    {
        SceneManager.LoadScene(sceneString);
    }
    #endregion
    public void ExitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public void ResetHighscore()
    {
        SavedDataManager.instance.ResetHighScore();
        resetText.alpha = 1f;
        resetText.DOFade(0f, timeToFade);
    }
    public void ShowWarning(bool isActive)
    {
        warning.SetActive(isActive);
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    #region DROPDOWN
    public void ChangeGridSize(int value)
    {
        SavedDataManager.instance.savedData.gridSize = (GameSettings.GridSizes)value;
    }
    public void ChangeLanguage(int value)
    {
        SavedDataManager.instance.savedData.language = (GameSettings.Language)value;
        var locale = LocalizationSettings.AvailableLocales.Locales[value];
        LocalizationSettings.SelectedLocale = locale;
    }
    public enum DropdownType
    {
        GRIDZISE,
        LANGUAGE        
    }
    #endregion
}
