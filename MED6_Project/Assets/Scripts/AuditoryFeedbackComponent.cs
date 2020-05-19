using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AuditoryFeedbackComponent : FeedbackComponent
{
    public override void Init(List<string> tags)
    {
        AudioListener.volume = 1f; //Enables audio when the component is loaded.
        base.Init(tags);
        Loaded = true;
        Debug.LogWarning("Auditory feedback component loaded...");
    }

    public override void OnEnter(OVRInput.Controller controller, Collider collision)
    {
    }

    public override void OnExit(OVRInput.Controller controller, Collider collision)
    { 
    }

    public override void OnStay(OVRInput.Controller controller, Collider collision)
    {
    }
}
