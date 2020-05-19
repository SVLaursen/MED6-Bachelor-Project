using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerExtender : FeedbackExtender
{
    [Header("Hammer Settings")]
    [SerializeField] private ImpactNode[] impactNodes;

    private OVRInput.Controller _controller;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        _controller = hand.Controller;
        base.GrabBegin(hand, grabPoint);

        foreach(var node in impactNodes)
            node.IsActive = true;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);

        foreach (var node in impactNodes)
            node.IsActive = false;
    }

    protected override void Update()
    {
        base.Update();

        foreach(var node in impactNodes)
        {
            if (node.IsHit && node.ColliderHit != null)
            {
                if (node.ColliderHit.name == "L" || node.ColliderHit.name == "R") return;
                if(ModalController.Instance.UseHaptics)
                    ApplyHaptic(_controller, node.ColliderHit);

                if(node.ColliderHit.name == "Chisel" || node.ColliderHit.name =="impact_point")
                {
                    ChiselExtender.Instance.HammerImpact();
                }
            }                
        }
    }
}
