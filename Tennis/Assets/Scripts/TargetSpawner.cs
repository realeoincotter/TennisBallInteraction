using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public Transform wall;              // drag the wall cube here
    public GameObject targetPrefab;     // your target prefab
    [Range(0f, 0.45f)]
    public float edgeMargin = 0.1f;     // keeps targets away from the edges (0.5 = the edge)
    public bool frontFace = true;       // flip this if targets appear on the back of the wall
    public float surfaceOffset = 0.02f; // small gap so the target doesn't flicker into the wall

    GameObject current;

    void Start() => SpawnTarget();

    public void SpawnTarget()
    {
        if (current) Destroy(current);

        // Random point on the face, in the cube's local space
        float x = Random.Range(-0.5f + edgeMargin, 0.5f - edgeMargin);
        float y = Random.Range(-0.5f + edgeMargin, 0.5f - edgeMargin);
        float z = frontFace ? -0.5f : 0.5f;

        // Convert to world space (handles the wall's position, rotation and scale)
        Vector3 pos = wall.TransformPoint(new Vector3(x, y, z));
        Vector3 normal = frontFace ? -wall.forward : wall.forward;

        pos += normal * surfaceOffset;
        Quaternion rot = Quaternion.LookRotation(normal); // target's +Z points out of the wall

        current = Instantiate(targetPrefab, pos, rot);
    }
}