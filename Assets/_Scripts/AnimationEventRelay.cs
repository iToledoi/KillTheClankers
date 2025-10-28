using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{   
    // This function is called via animation event to notify the parent AIEnemy to handle death
    public void CallEnemyDeathParentFunction()
    {
        // Calls "killEnemy" function on the parent object
        GetComponentInParent<AIEnemy>()?.killEnemy();
    }
}
