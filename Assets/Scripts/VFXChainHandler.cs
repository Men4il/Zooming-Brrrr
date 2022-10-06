using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class VFXChainHandler : MonoBehaviour
{
    [SerializeField] private VFXChain _vfxChainPrefab;
    
    private ObjectPool<VFXChain> _vfxChainPool;

    private void Awake()
    {
        _vfxChainPool = new ObjectPool<VFXChain>(CreateVFXChain, OnVFXChainGet, OnVFXChainRelease);
    }

    public async Task CreateVFXChain(Vector3 startPosition, Vector3 endPosition)
    {
        var vfxChain = _vfxChainPool.Get();
        await vfxChain.Initialize(startPosition, endPosition);
    }
    
    private VFXChain CreateVFXChain()
    {
        VFXChain vfxChain = Instantiate(_vfxChainPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<VFXChain>();
        return vfxChain;
    }

    private void OnVFXChainGet(VFXChain vfxChain)
    {
        vfxChain.gameObject.SetActive(true);
        vfxChain.OnComplete.AddListener(ReleaseVFXChain);
    }

    private void OnVFXChainRelease(VFXChain vfxChain)
    {
        vfxChain.gameObject.SetActive(false);
        vfxChain.OnComplete.RemoveListener(ReleaseVFXChain);
    }

    private void ReleaseVFXChain(VFXChain vfxChain)
    {
        _vfxChainPool.Release(vfxChain);
    }
}