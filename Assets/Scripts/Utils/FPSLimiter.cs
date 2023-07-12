using UnityEngine;

public class FPSLimiter : Singleton<FPSLimiter>
{
    [SerializeField] private int targetFrameRate = 30;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}