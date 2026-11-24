using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public Transform wall;      // the wall cube
    public Transform target;    // the Target in your scene (not the prefab)
    [Range(0f, 0.45f)]
    public float edgeMargin = 0.15f;   // keeps the target away from the wall edges
    public float minMoveDistance = 1f; // new spot must be at least this far from the last one

    int normalAxis; // which local axis of the wall is its thickness

    void Start()
    {
        // The thinnest axis of the wall is the one pointing out of its face
        Vector3 s = wall.lossyScale;
        normalAxis = 0;
        if (Mathf.Abs(s.y) < Mathf.Abs(s[normalAxis])) normalAxis = 1;
        if (Mathf.Abs(s.z) < Mathf.Abs(s[normalAxis])) normalAxis = 2;

        SpawnTarget();
    }

    public void SpawnTarget()
    {
        // Where the target is now, in the wall's local space
        Vector3 current = wall.InverseTransformPoint(target.position);
        Vector3 local = current;

        for (int tries = 0; tries < 20; tries++)
        {
            local = current; // keeps the same distance from the wall
            for (int i = 0; i < 3; i++)
            {
                if (i != normalAxis)
                    local[i] = Random.Range(-0.5f + edgeMargin, 0.5f - edgeMargin);
            }

            if (Vector3.Distance(wall.TransformPoint(local), target.position) >= minMoveDistance)
                break;
        }

        target.position = wall.TransformPoint(local);
    }
}