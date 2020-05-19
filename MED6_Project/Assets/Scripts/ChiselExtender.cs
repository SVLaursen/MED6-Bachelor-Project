using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiselExtender : FeedbackExtender
{
    [Header("Chisel Settings")]
    [SerializeField] private ImpactNode[] impactNodes;
    [SerializeField] private ChiselImpacter impactPoint;    

    private OVRInput.Controller _controller;

    public static ChiselExtender Instance { get; private set; }

    protected override void Start()
    {
        base.Start();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        _controller = hand.Controller;
        base.GrabBegin(hand, grabPoint);

        foreach (var node in impactNodes)
            node.IsActive = true;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);

        foreach (var node in impactNodes)
            node.IsActive = false;
    }

    public void HammerImpact()
    {
        foreach(var node in impactNodes)
            if(ModalController.Instance.UseHaptics)
                ApplyHaptic(_controller, node.ColliderHit);

        impactPoint.Impact();
    }

    protected override void Update()
    {
        base.Update();

        foreach(var node in impactNodes)
        {
            if(node.IsHit && node.ColliderHit != null)
            {
                if (node.ColliderHit.name == "L" || node.ColliderHit.name == "R") return;
                ApplyHaptic(_controller, node.ColliderHit);
            }
        }
    }
}
