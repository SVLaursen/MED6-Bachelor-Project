using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandComponent : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller controller;

    public OVRInput.Controller Controller => controller;

    private void OnTriggerEnter(Collider other) => 
        ModalController.Instance.OnTouchEnter(controller, other);

    private void OnTriggerStay(Collider other) => 
        ModalController.Instance.OnTouchStay(controller, other);

    private void OnTriggerExit(Collider other) => 
        ModalController.Instance.OnTouchExit(controller, other);

}
