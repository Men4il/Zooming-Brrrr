using UnityEngine;

public struct NearbySearchResult
{
    public NearbySearchResult(float sqrDistance, GameObject gameObject)
    {
        SqrDistance = sqrDistance;
        GameObject = gameObject;
    }
    public float SqrDistance { get; }
    public GameObject GameObject { get; }
}