using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioVolumeChanger : MonoBehaviour
{
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private VolumeParams volumeParam;
    [SerializeField] private float thresholdVolume = 80f;
    private string exposedParam;
    [Header("Volume")]
    [SerializeField] private float minVolume = 0f;
    [SerializeField] private float maxVolume = 100f;
    [Header("Input")]
    private TMP_InputField inputField;
    private TMP_Text inputPlaceHolder;
    [Header("Slider")]
    private Slider slider;

    private float currentVolume;
    private SO_SavedData savedData;

    private void OnValidate()
    {
        slider = GetComponentInChildren<Slider>();
        inputField = GetComponentInChildren<TMP_InputField>();
        inputPlaceHolder = inputField.placeholder.GetComponent<TextMeshProUGUI>();
        SetVolumeExposedParam();
    }
    // Start is called before the first frame update
    void Start()
    {
        savedData = SavedDataManager.instance.savedData;
        slider.minValue = minVolume;
        slider.maxValue = maxVolume;
        StartVolumeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        inputPlaceHolder.text = slider.value.ToString();
    }

    private void StartVolumeLevel()
    {
        switch (volumeParam)
        {
            case VolumeParams.MASTER:
                currentVolume = savedData.masterVolume;
                break;
        }
        audioMixer.SetFloat(exposedParam, currentVolume);
        slider.value = currentVolume;
    }
    public void ClearText()
    {
        inputField.text = string.Empty;
    }
    public void ChangeVolumeInputToSlider(string value)
    {
        float.TryParse(value, out currentVolume);
        if (currentVolume > 100f)
        {
            currentVolume = 100f;
        }
        else if (currentVolume < 0f)
        {
            currentVolume = 0f;
        }
        slider.value = currentVolume;
        SaveVolumeLevel(currentVolume);
        ClearText();
    }
    public void SetVolumeLevel(float volume)
    {
        audioMixer.SetFloat(exposedParam, volume - thresholdVolume);
        SaveVolumeLevel(volume);
    }
    public void SaveVolumeLevel(float volume)
    {
        switch (volumeParam)
        {
            case VolumeParams.MASTER:
                savedData.masterVolume = volume;
                break;
        }
    }
    private void SetVolumeExposedParam()
    {
        switch (volumeParam)
        {
            case VolumeParams.MASTER:
                exposedParam = "masterVol";
                break;
        }
        
    }
    public enum VolumeParams
    {
        MASTER
    }
}
