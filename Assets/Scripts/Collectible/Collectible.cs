using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool IsVictoryCondition;

    public int Score;

    public event Action<int, Collectible> OnCollected;
    public event Action<Collectible, GameObject> OnSpecialFruits;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Cherry") || gameObject.CompareTag("Strawberry"))
        {
            Debug.Log("CollectibleEvent");
            OnSpecialFruits?.Invoke(this, gameObject);
        }

        OnCollected?.Invoke(Score, this);


        Destroy(gameObject);
    }
}
