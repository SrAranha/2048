using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private float _hudRefreshRate = 1f;

    private TMP_Text _fpsText;
    private float _timer;

    private void Start()
    {
        _fpsText = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}