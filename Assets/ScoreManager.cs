using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int score;
    [SerializeField] private UnityEvent<int> onScoreChanged;

    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        onScoreChanged.Invoke(score);
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        score += amount;
        onScoreChanged.Invoke(score);
        Debug.Log($"Score: {score}");
    }

    public void ResetScore()
    {
        score = 0;
        onScoreChanged.Invoke(score);
    }
}
