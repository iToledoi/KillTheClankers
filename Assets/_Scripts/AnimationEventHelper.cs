using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnMeleeAttackPerformed, OnRangedAttackPerformed, OnEnemyDeath;

    // Trigger the general animation event
    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    // Trigger the melee attack event
    public void TriggerAttack()
    {
        OnMeleeAttackPerformed?.Invoke();
    }

    // Trigger the ranged attack event
    public void TriggerRangedAttack()
    {
        OnRangedAttackPerformed?.Invoke();
    }

    // Trigger the enemy death event
    public void TriggerEnemyDeath()
    {
        OnEnemyDeath?.Invoke();
    }
}
