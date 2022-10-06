using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class VFXChain : MonoBehaviour
{
    [SerializeField] private VisualEffect _visualEffect;

    public UnityEvent<VFXChain> OnComplete;

    private void OnEnable()
    {
        _visualEffect.Stop();
    }

    public async Task Initialize(Vector3 startPosition, Vector3 endPosition)
    {
        transform.position = startPosition;
        _visualEffect.SetVector3("EndingPosition", endPosition - startPosition);
        
        var sparksMinMaxLifetime = _visualEffect.GetVector2("SparksMinMaxLifetime");
        var randomSparksLifeTime = Random.Range(sparksMinMaxLifetime.x, sparksMinMaxLifetime.y);
        
        _visualEffect.SetFloat("SparksLifetime", randomSparksLifeTime);
        _visualEffect.Play();
        var duration = _visualEffect.GetFloat("ChainLifetime");
        
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            await Task.Yield();
        }
        
        SparksWait(randomSparksLifeTime);
    }

    private async void SparksWait(float duration)
    {
        await Task.Delay((int)(duration * 1000));
        OnComplete.Invoke(this);
    }
}