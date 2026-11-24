using UnityEngine;

public class Target : MonoBehaviour
{
    public static int totalScore = 0;
    int bestHit = 0;

    public void RegisterHit(int points)
    {
        if (points > bestHit) bestHit = points;
    }

    void LateUpdate()
    {
        if (bestHit == 0) return;

        totalScore += bestHit;
        Debug.Log($"Hit for {bestHit}! Total: {totalScore}");
        bestHit = 0;

        FindFirstObjectByType<TargetSpawner>().SpawnTarget();
    }
}