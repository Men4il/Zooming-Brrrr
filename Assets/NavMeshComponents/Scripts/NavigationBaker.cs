using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour {

    public NavMeshSurface surface;

    void Update () 
    {
        surface.BuildNavMesh(); 
    }

}