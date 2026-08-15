using UnityEngine;

public class Coin : Pickup
{
    [SerializeField] private int scoreAmount = 100;
    [SerializeField] private AudioClip coinCollectSound;

    private ScoreManager scoreManager;

    public void Init(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickup()
    {
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager is not initialized!");
            return;
        }

        // Increase score
        scoreManager.IncreaseScore(scoreAmount);

        // Play coin collection sound
        if (coinCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                coinCollectSound,
                transform.position
            );
        }
    }
}