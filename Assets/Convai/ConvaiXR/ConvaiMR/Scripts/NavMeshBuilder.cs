using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBuilder : MonoBehaviour
{
    private NavMeshSurface _navMeshSurface;
    private int _minimumNavMeshSurfaceArea = 0;

    private void Awake()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();

        if (_navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface component not found!");
        }
    }

    public bool TryToBuildNavMesh()
    {
        _navMeshSurface.BuildNavMesh();

        if (_navMeshSurface.navMeshData.sourceBounds.extents.x * _navMeshSurface.navMeshData.sourceBounds.extents.z > _minimumNavMeshSurfaceArea)
        {
            return true;
        }

        Debug.LogError("NavMeshBuilder failed to generate a nav mesh!");
        return false;
    }
}