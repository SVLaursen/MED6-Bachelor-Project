using System.Collections;
using System.Collections.Generic;
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
        _previousPosition = transform.localPosition;
        _rb = GetComponent<Rigidbody>();

        Physics.IgnoreCollision(GetComponent<Collider>(), 
            ModalController.Instance.PlayerCharacter.GetComponent<Collider>());
    }

    protected virtual void Update()
    {
        Debug.DrawRay(transform.localPosition, Vector3.up * rayLength, Color.yellow); //UP
        Debug.DrawRay(transform.localPosition, Vector3.down * rayLength, Color.yellow); //DOWN
        Debug.DrawRay(transform.localPosition, Vector3.forward * rayLength, Color.yellow); //FORWARD
        Debug.DrawRay(transform.localPosition, Vector3.back * rayLength, Color.yellow); //BACK
        Debug.DrawRay(transform.localPosition, Vector3.left * rayLength, Color.yellow); // LEFT
        Debug.DrawRay(transform.localPosition, Vector3.right * rayLength, Color.yellow); //RIGHT

        if (!IsActive) return;

        RaycastHit hit;
        _newPosition = transform.localPosition;

        //UP
        if (Physics.Raycast(transform.localPosition, transform.up, out hit, rayLength))
        {
            if (transform.localPosition.y > _previousPosition.y && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.y = _previousPosition.y;
            else _newPosition.y = transform.localPosition.y;
        }        

        //DOWN
        if (Physics.Raycast(transform.localPosition, -transform.up, out hit, rayLength))
        {
            if (transform.localPosition.y < _previousPosition.y && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.y = _previousPosition.y;
            else _newPosition.y = transform.localPosition.y;
        }        

        //FORWARD
        if (Physics.Raycast(transform.localPosition, transform.forward, out hit, rayLength))
        {
            if (transform.localPosition.z > _previousPosition.z)
                _newPosition.z = _previousPosition.z;
            else _newPosition.z = transform.localPosition.z;
        }        

        //BACK
        if (Physics.Raycast(transform.localPosition, -transform.forward, out hit, rayLength))
        {
            if (transform.localPosition.z < _previousPosition.z && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.z = _previousPosition.z;
            else _newPosition.z = transform.localPosition.z;
        }        

        //LEFT
        if (Physics.Raycast(transform.localPosition, -transform.right, out hit, rayLength))
        {
            if (transform.localPosition.x < _previousPosition.x && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.x = _previousPosition.x;
            else _newPosition.x = transform.localPosition.x;
        }        

        //RIGHT
        if (Physics.Raycast(transform.localPosition, transform.right, out hit, rayLength))
        {
            if (transform.localPosition.x > _previousPosition.x && hit.transform.tag != "Player" && !HitIgnorableObject(hit.collider.gameObject))
                _newPosition.x = _previousPosition.x;
            else _newPosition.x = transform.localPosition.x;
        }        

        transform.localPosition = _newPosition;
        _previousPosition = transform.localPosition;
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
