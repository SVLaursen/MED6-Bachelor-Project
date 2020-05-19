using System.Collections.Generic;
using UnityEngine;

public abstract class FeedbackComponent
{
    protected ModalController _controller;

    /// <summary>
    /// Collision mask used for differentiating objects on collision
    /// </summary>
    protected List<string> _collisionTags;

    /// <summary>
    /// See if the component has loaded through the init method
    /// </summary>
    public bool Loaded { get; set; }

    /// <summary>
    /// Initializes the feedback component
    /// </summary>
    public virtual void Init(List<string> tags)
    {
        _collisionTags = tags;
        _controller = ModalController.Instance;
    }

    /// <summary>
    /// Handles the logic for what should happen on impact between the VR controller and an object
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="collision"></param>
    public abstract void OnEnter(OVRInput.Controller controller, Collider collision);

    /// <summary>
    /// Handles the logic for what should happen on extended contact with a collision object
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="collision"></param>
    public abstract void OnStay(OVRInput.Controller controller, Collider collision);

    /// <summary>
    /// Handles the logic for what should happen after a contact with an object ends
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="collision"></param>
    public abstract void OnExit(OVRInput.Controller controller, Collider collision);

    /// <summary>
    /// Checks if the object collided with is supposed to give feedback
    /// Can be overwritten
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    protected virtual bool CheckCollision(GameObject collision)
    {
        Debug.LogWarning(_collisionTags.Count);
        foreach (var tag in _collisionTags)
            if (collision.tag == tag)
                return true;

        return false;
    }
}
