using System;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBarScript : MonoBehaviour
{
    [SerializeField] private DifficultyManager _difficultyManager;
    [SerializeField] private Image _progressBarFilling;
    [SerializeField] private ProgressBarData[] _barData;
    
    private float _currentDifficultyThreshold = 1f;
    private float _prevDifficultyThreshold;

    private void OnEnable()
    {
        _difficultyManager.OnDifficultyStateChanged += UpdateProgressBar;
    }
    
    private void OnDisable()
    {
        _difficultyManager.OnDifficultyStateChanged -= UpdateProgressBar;
    }

    private void Update()
    {
        var currentDifficultyValue = _difficultyManager.GetDifficulty();
        SetProgressBarValue((currentDifficultyValue - _prevDifficultyThreshold) / (_currentDifficultyThreshold - _prevDifficultyThreshold));
    }

    private void UpdateProgressBar(DifficultyManager.DifficultyState currentState)
    {
        for (int i = 0; i < _barData.Length; i++)
        {
            var tempBarData = _barData[i];
            if (tempBarData.State == currentState)
            {
                _prevDifficultyThreshold = _currentDifficultyThreshold;
                _currentDifficultyThreshold = _difficultyManager.GetCurrentDifficultyThreshold();
                SetProgressBarColor(tempBarData.Color);
            }
        }
    }

    private void SetProgressBarValue(float value)
    {
        _progressBarFilling.fillAmount = Mathf.Clamp01(value);
    }

    private void SetProgressBarColor(Color color)
    {
        _progressBarFilling.color = color;
    }
    
    [Serializable]
    private struct ProgressBarData
    {
        [SerializeField] private DifficultyManager.DifficultyState _state;
        [SerializeField] private Color _color;

        public DifficultyManager.DifficultyState State => _state;
        public Color Color => _color;
    }
}