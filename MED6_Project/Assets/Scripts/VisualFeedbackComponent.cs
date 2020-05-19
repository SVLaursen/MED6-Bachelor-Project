using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisualFeedbackComponent : FeedbackComponent
{
    [SerializeField] private GameObject avatar;

    private GameObject _leftHand, _rightHand;
    private InteractableObject[] _interactableObjects;

    public override void Init(List<string> tags)
    {
        base.Init(tags);
        Debug.LogWarning("Loading Visual Feedback Component");

        _interactableObjects = Object.FindObjectsOfType<InteractableObject>();

        foreach(Transform child in avatar.transform)
        {
            if (child.gameObject.name == "hand_right")
                _rightHand = child.gameObject;
            if (child.gameObject.name == "hand_left")
                _leftHand = child.gameObject;
        }

        Loaded = _leftHand != null && _rightHand != null;
    }

    public override void OnEnter(OVRInput.Controller controller, Collider collision)
    {
        if (!CheckCollision(collision.gameObject)) return;

        var interactable = GetInteractableObjectFromArray(collision.gameObject);
        if (interactable == null) return;

        interactable.IsActive = true;
        interactable.SetPreviousPosition(OVRInput.Controller.RTouch == controller ? _rightHand.transform.position : _leftHand.transform.position);
        interactable.HandObject = OVRInput.Controller.RTouch == controller ? _rightHand.transform : _leftHand.transform;
    }

    public override void OnExit(OVRInput.Controller controller, Collider collision)
    {
        if (!CheckCollision(collision.gameObject)) return;

        var interactable = GetInteractableObjectFromArray(collision.gameObject);
        if (interactable == null) return;

        interactable.HandObject = null;
        interactable.IsActive = false;
    }

    public override void OnStay(OVRInput.Controller controller, Collider collision)
    {
        if (!CheckCollision(collision.gameObject)) return;
    }

    private InteractableObject GetInteractableObjectFromArray(GameObject input)
    {
        foreach(var obj in _interactableObjects)
            if (input.name == obj.gameObject.name)
                return obj;

        return null;
    }
}
