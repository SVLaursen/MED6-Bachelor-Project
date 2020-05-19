using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HapticFeedbackComponent : FeedbackComponent
{
    [SerializeField] private HapticFeedbackSettings defaultSettings;

    public bool OverrideFeedback { get; set; }

    public override void Init(List<string> tags)
    {
        base.Init(tags);
        Loaded = true;
        Debug.LogWarning("Haptic feedback component loaded...");
    }

    public override void OnEnter(OVRInput.Controller controller, Collider collision)
    {
        if (!CheckCollision(collision.gameObject) || OverrideFeedback) return;
        Debug.LogWarning("On enter: haptic feedback");
        OVRInput.SetControllerVibration(defaultSettings.Frequency, defaultSettings.Amplitude, controller);
    }

    public void OnEnter(OVRInput.Controller controller, Collider collision, HapticFeedbackSettings settings)
    {
        if (!CheckCollision(collision.gameObject)) return;
        //Debug.LogWarning("On enter: haptic feedback");
        OVRInput.SetControllerVibration(settings.Frequency, settings.Amplitude, controller);
    }

    public override void OnStay(OVRInput.Controller controller, Collider collision)
    {        
        if (!CheckCollision(collision.gameObject) || OverrideFeedback) return;
        Debug.LogWarning("On Stay: Haptic Feedback");
        OVRInput.SetControllerVibration(defaultSettings.ContinousFrequency, defaultSettings.ContinousAmplitude, controller);
    }

    public void OnStay(OVRInput.Controller controller, Collider collision, HapticFeedbackSettings settings)
    {
        if (!CheckCollision(collision.gameObject)) return;
        //Debug.LogWarning("On Stay: Haptic Feedback");
        OVRInput.SetControllerVibration(settings.ContinousFrequency, settings.ContinousAmplitude, controller);
    }

    public override void OnExit(OVRInput.Controller controller, Collider collision)
    {
        OVRInput.SetControllerVibration(0, 0, controller);
    }


}

[System.Serializable]
public struct HapticFeedbackSettings
{
    [Header("Frequency Settings")]
    [Tooltip("The frequency that the haptics will use to create vibrations on impact")]
    [Range(0f, 1f)] public float Frequency;

    [Tooltip("The frequency that the haptics will use to create vibrations on continuos object contact")]
    [Range(0f, 1f)] public float ContinousFrequency;

    [Header("Amplitude Settings")]
    [Tooltip("The amplitude that the haptics will use to create vibrations on impact")]
    [Range(0f, 1f)] public float Amplitude;

    [Tooltip("The amplitude that the haptics will use to create vibrations on continuos object contact")]
    [Range(0f, 1f)] public float ContinousAmplitude;
}
