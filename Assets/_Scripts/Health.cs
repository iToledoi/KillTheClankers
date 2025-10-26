using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private AudioClip[] playerDeathSounds;

    // Initializes health values
    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    // Amount is damage subtracted from health, if health <= 0, object is destroyed
    public void GetHit(int amount, GameObject sender)
    {

        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            //if player death, play death sound
            if (gameObject.CompareTag("Player"))
            {
                SoundFXManager.instance.PlayRandomSound(playerDeathSounds, transform, 5f);
                ScoreManager.instance.OnGameOver();
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.GameOver();
                }
            }
            isDead = true;
            Destroy(gameObject, 0.4f);
        }
    }
}