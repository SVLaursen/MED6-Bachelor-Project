using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InteractableObject : OVRGrabbable
{
    [Header("Interactable Object Settings")]
    [SerializeField] private float rayLength = .25f;
    [SerializeField] private List<GameObject> objectsToIgnore;

    public bool IsActive { get; set; }
    public Transform HandObject { get; set; }

    private Vector3 _newPosition, _previousPosition;
    private float _stopX, _stopY, _stopZ;

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
        Debug.DrawRay(transform.localPosition, transform.up * rayLength, Color.yellow); //UP
        Debug.DrawRay(transform.localPosition, -transform.up * rayLength, Color.yellow); //DOWN
        Debug.DrawRay(transform.localPosition, transform.forward * rayLength, Color.yellow); //FORWARD
        Debug.DrawRay(transform.localPosition, -transform.forward * rayLength, Color.yellow); //BACK
        Debug.DrawRay(transform.localPosition, -transform.right * rayLength, Color.yellow); // LEFT
        Debug.DrawRay(transform.localPosition, transform.right * rayLength, Color.yellow); //RIGHT

        if (!IsActive) return;

        RaycastHit hit;
        _newPosition = transform.position;

        //UP
        if (Physics.Raycast(transform.position, transform.up, out hit, rayLength))
        {
            if (transform.position.y > _previousPosition.y && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.y = _previousPosition.y;
            else _newPosition.y = transform.position.y;
        }        

        //DOWN
        if (Physics.Raycast(transform.position, -transform.up, out hit, rayLength))
        {
            if (transform.position.y < _previousPosition.y && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.y = _previousPosition.y;
            else _newPosition.y = transform.position.y;
        }        

        //FORWARD
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength))
        {
            if (transform.position.z > _previousPosition.z)
                _newPosition.z = _previousPosition.z;
            else _newPosition.z = transform.position.z;
        }        

        //BACK
        if (Physics.Raycast(transform.position, -transform.forward, out hit, rayLength))
        {
            if (transform.position.z < _previousPosition.z && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.z = _previousPosition.z;
            else _newPosition.z = transform.position.z;
        }        

        //LEFT
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayLength))
        {
            if (transform.position.x < _previousPosition.x && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.x = _previousPosition.x;
            else _newPosition.x = transform.position.x;
        }        

        //RIGHT
        if (Physics.Raycast(transform.position, transform.right, out hit, rayLength))
        {
            if (transform.position.x > _previousPosition.x && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.x = _previousPosition.x;
            else _newPosition.x = transform.position.x;
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
}
