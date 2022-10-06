using System;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyData[] _difficulties;
    [SerializeField] private AnimationCurve _difficultyCurve;
    
    private float _maxDifficultyThreshold;
    private float _currentDifficulty;
    private DifficultyState _currentDifficultyId;
    private int _currentDifficultyIndex;
    
    public event Action<DifficultyState> OnDifficultyStateChanged; 

    private void Start()
    {
        if (_difficulties != null && _difficulties.Length != 0)
        {
            _currentDifficulty = _difficulties[0].Threshold;
            _maxDifficultyThreshold = _difficulties[^1].Threshold;
            
            UpdateDifficultyState(_difficulties[0]);
        }
        else
        {
            Debug.LogError("Dura where is difficulties???");
        }
    }

    private void Update()
    {
        _currentDifficulty += Time.deltaTime * _difficultyCurve.Evaluate(_currentDifficulty / _maxDifficultyThreshold) * _maxDifficultyThreshold;

        if (ShouldChangeDifficultyState(_currentDifficulty, out var difficultyIndex))
        {
            _currentDifficultyIndex = difficultyIndex;
            UpdateDifficultyState(_difficulties[difficultyIndex]);
        }
    }

    public float GetDifficulty()
    {
        return _currentDifficulty;
    }

    public float GetCurrentDifficultyThreshold()
    {
        return _difficulties[_currentDifficultyIndex < _difficulties.Length - 1 ? _currentDifficultyIndex + 1 : _currentDifficultyIndex].Threshold;
    }

    private void UpdateDifficultyState(DifficultyData difficulty)
    {
        _currentDifficultyId = difficulty.Id;
        OnDifficultyStateChanged?.Invoke(difficulty.Id);
        
    }

    private bool ShouldChangeDifficultyState(float currentDifficultyValue, out int difficultyIndex)
    {
        difficultyIndex = 0;
        
        for (var i = _currentDifficultyIndex; i < _difficulties.Length; i++)
        {
            var difficulty = _difficulties[i];
            
            if (_currentDifficultyId != difficulty.Id && currentDifficultyValue >= difficulty.Threshold)
            {
                difficultyIndex = i;
                return true;
            }
        }
        
        return false;
    }
    

    [Serializable]
    private struct DifficultyData
    {
        [SerializeField] private DifficultyState _id;
        [SerializeField] private float _threshold;

        public DifficultyState Id => _id;
        public float Threshold => _threshold;
    }
    

    public enum DifficultyState
    {
        Easy,
        Normal,
        Hard,
        Hell,
        Impossible
    }
}