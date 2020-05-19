using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactNode : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private FeedbackExtender parent;
    [SerializeField] private List<Transform> stopWhenHitting;
    [SerializeField] private float waitTime;

    public bool IsActive { get; set; }
    public bool IsHit { get; private set; }
    public Collider ColliderHit { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive) return;
        IsHit = true;
        ColliderHit = other;

        foreach(var obj in stopWhenHitting)
            if (other.gameObject == obj.gameObject)
                StartCoroutine(StopMovementBriefly());
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderHit = null;
    }

    private IEnumerator StopMovementBriefly()
    {
        parent.SuspendedPosition = parent.transform.position;
        parent.MovementSuspended = true;

        yield return new WaitForSeconds(waitTime);

        parent.MovementSuspended = false;
        yield return null;
    }
}
