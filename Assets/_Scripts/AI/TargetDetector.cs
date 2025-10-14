using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 100;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField]
    private bool showGizmos = false;

    //gizmo parameters
    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        // Find the player within range (or globally)
        Collider2D playerCollider =
            Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
            // Enemy always knows where the player is
            colliders = new List<Transform>() { playerCollider.transform };

            // Optional: visualize the detection
            Debug.DrawLine(transform.position, playerCollider.transform.position, Color.magenta);
        }
        else
        {
            // No player detected in range
            colliders = null;
        }

        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}