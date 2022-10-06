using UnityEngine;

public class PlaneMover : MonoBehaviour
{
    [SerializeField] private GameObject _plane;

    private void Update()
    {
        _plane.transform.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
    }
}