using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    AudioSource source;

    void Start() => source = GetComponent<AudioSource>();

    private void OnCollisionEnter(Collision collision) => PlaySound();

    public void PlaySound() => source.Play();

}