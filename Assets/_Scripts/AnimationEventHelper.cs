using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnMeleeAttackPerformed, OnRangedAttackPerformed;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        OnMeleeAttackPerformed?.Invoke();
    }

    public void TriggerRangedAttack()
    {
        OnRangedAttackPerformed?.Invoke();
    }
}
