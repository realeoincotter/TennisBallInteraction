using UnityEngine;

public class Target : MonoBehaviour
{
    int bestHit = 0;

    public void RegisterHit(int points)
    {
        if (points > bestHit) bestHit = points;
    }

    void LateUpdate()
    {
        if (bestHit == 0) return;

        ScoreManager.Instance.AddPoints(bestHit);
        bestHit = 0;

        FindFirstObjectByType<TargetSpawner>().SpawnTarget();
    }
}