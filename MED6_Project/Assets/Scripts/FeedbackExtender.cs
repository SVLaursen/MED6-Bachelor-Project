using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackExtender : InteractableObject
{
    [Header("Feedback Extender Settings")]
    [SerializeField] private HapticFeedbackSettings hapticSettings;
    [SerializeField] private List<Collider> collidersToIgnore;

    public bool MovementSuspended { get; set; }
    public Vector3 SuspendedPosition { get; set; }

    private Transform _avatar;
    private Transform _handRenderer;    

    protected override void Start()
    {
        base.Start();
        _avatar = ModalController.Instance.playerAvatar.transform;

        var ownColl = GetComponents<Collider>();

        foreach (var coll in collidersToIgnore)
            foreach(var own in ownColl)
                Physics.IgnoreCollision(own, coll);
    }

    protected override void Update()
    {
        if (MovementSuspended)
            transform.position = SuspendedPosition;

        base.Update();        
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        ModalController.Instance.HapticComponent.OverrideFeedback = true;

        transform.position = hand.transform.position;
        _handRenderer = FindHandRenderer(hand.Controller);

        if (_handRenderer != null)
            _handRenderer.gameObject.SetActive(false);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        ModalController.Instance.HapticComponent.OverrideFeedback = false;

        if (_handRenderer != null)
            _handRenderer.gameObject.SetActive(true);

        _handRenderer = null;
    }

    protected void ApplyHaptic(OVRInput.Controller target, Collider collision)
    {
        ModalController.Instance.HapticComponent.OnEnter(target, collision, hapticSettings);
    }

    private Transform FindHandRenderer(OVRInput.Controller hand)
    {
        string handName = hand == OVRInput.Controller.RTouch ? "hand_right" : "hand_left";

        foreach (Transform child in _avatar)
            if (child.gameObject.name == handName)
                return child;

        return null;
    }
}
