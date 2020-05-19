using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InteractableObject : OVRGrabbable
{
    [Header("Interactable Object Settings")]
    [SerializeField] private bool enableRayDebug;
    [SerializeField] private LayerMask visualFeedbackMask;
    [SerializeField] private float upRayLength = .25f;
    [SerializeField] private float downRayLength = .25f;
    [SerializeField] private float forwardRayLength = .25f;
    [SerializeField] private float backwardsRayLength = .25f;
    [SerializeField] private float leftRayLength = .25f;
    [SerializeField] private float rightRayLength = .25f;
    [SerializeField] private float stopDistance = .5f;
    [SerializeField] private float backwardsStopDistance = 10f;
    [SerializeField] private List<GameObject> objectsToIgnore;

    public bool IsActive { get; set; }
    public Transform HandObject { get; set; }

    private Vector3 _newPosition, _previousPosition;

    protected Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _previousPosition = transform.position;
        _rb = GetComponent<Rigidbody>();

        Physics.IgnoreCollision(GetComponent<Collider>(), 
            ModalController.Instance.PlayerCharacter.GetComponent<Collider>());
    }

    protected virtual void Update()
    {
        if (!IsActive) return;

        RaycastHit hit;
        _newPosition = transform.position;

        //UP
        if (Physics.Raycast(transform.position, transform.up, out hit, upRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, stopDistance);
        }     
        
        //DOWN
        if (Physics.Raycast(_newPosition, -transform.up, out hit, downRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, stopDistance);
        }      

        //FORWARD
        if (Physics.Raycast(transform.position, transform.forward, out hit, forwardRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, stopDistance);
        }     

        //BACK
        if (Physics.Raycast(transform.position, -transform.forward, out hit, backwardsRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, backwardsStopDistance);
        }

        //LEFT
        if (Physics.Raycast(transform.position, -transform.right, out hit, leftRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, stopDistance);
        }        

        //RIGHT
        if (Physics.Raycast(transform.position, transform.right, out hit, rightRayLength, visualFeedbackMask))
        {
            CalculatePosition(hit, stopDistance);
        }    

        transform.position = _newPosition;
        _previousPosition = transform.position;
    }

    public void SetPreviousPosition(Vector3 previousPosition) => _previousPosition = previousPosition;

    private bool HitIgnorableObject(GameObject obj)
    {
        foreach (var item in objectsToIgnore)
            if (obj == item)
                return true;
        return false;
    }

    private void CalculatePosition(RaycastHit hit, float distance)
    {
        if (Vector3.Distance(transform.position, hit.point) < distance && 
            hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
            _newPosition = _previousPosition;
        else
            _newPosition = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        if (!enableRayDebug) return;
        Gizmos.color = Color.yellow;

        Vector3 direction = -transform.forward * backwardsRayLength;
        Gizmos.DrawRay(transform.position, direction);

        direction = transform.forward * forwardRayLength;
        Gizmos.DrawRay(transform.position, direction);

        direction = transform.up * upRayLength;
        Gizmos.DrawRay(transform.position, direction);

        direction = -transform.up * downRayLength;
        Gizmos.DrawRay(transform.position, direction);

        direction = -transform.right * leftRayLength;
        Gizmos.DrawRay(transform.position, direction);

        direction = transform.right * rightRayLength;
        Gizmos.DrawRay(transform.position, direction);
    }
}
