using UnityEngine;

public class EnemyScoreTarget : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private string hitterTag = "Bullet";
    [SerializeField] private bool destroyEnemyOnHit = true;
    [SerializeField] private bool destroyHitterOnHit;

    private bool hasBeenHit;

    private void OnCollisionEnter(Collision collision)
    {
        TryScore(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryScore(other.gameObject);
    }

    private void TryScore(GameObject hitter)
    {
        if (hasBeenHit || !hitter.CompareTag(hitterTag))
        {
            return;
        }

        hasBeenHit = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }
        else
        {
            Debug.LogWarning("ScoreManager is missing from the scene.");
        }

        if (destroyHitterOnHit)
        {
            Destroy(hitter);
        }

        if (destroyEnemyOnHit)
        {
            Destroy(gameObject);
        }
    }
}
