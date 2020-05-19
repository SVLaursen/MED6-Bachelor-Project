using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class ChiselImpacter : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Shader drawShader;
    [SerializeField] private GameObject stoneFace;
    [SerializeField] private LayerMask stoneMask;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private ImpactSound soundComponent;

    [Header("Drawing Settings")]
    [SerializeField, Range(0f, 100f)] private float brushSize;
    [SerializeField, Range(0f, 1f)] private float brushStrength;

    [Header("Debugging")]
    [SerializeField] private bool enableDebug;

    private Material _trackMaterial, _drawMaterial;

    public RenderTexture SplatMap { get; private set; }

    private void Start()
    {
        _drawMaterial = new Material(drawShader);
        _trackMaterial = stoneFace.GetComponent<MeshRenderer>().material;
        _trackMaterial.SetTexture("_Splat", SplatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    private void Update()
    {
        if (enableDebug)
            Debug.DrawRay(transform.position, -transform.forward * rayLength, Color.blue);
    }

    public void Impact()
    {
        RaycastHit hit;

        if (ModalController.Instance.UseAuditory && soundComponent != null)
            soundComponent.PlaySound();

        if (Physics.Raycast(transform.position, -transform.forward, out hit, rayLength, stoneMask))
        {
            _drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
            _drawMaterial.SetFloat("_BrushStrength", brushStrength);
            _drawMaterial.SetFloat("_BrushSize", brushSize);

            RenderTexture targetTexture = RenderTexture.GetTemporary(SplatMap.width, SplatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(SplatMap, targetTexture);
            Graphics.Blit(targetTexture, SplatMap, _drawMaterial);
            RenderTexture.ReleaseTemporary(targetTexture);
        }
    }

    private void OnGUI()
    {
        if(enableDebug)
            GUI.DrawTexture(new Rect(0, 0, 128, 128), SplatMap, ScaleMode.ScaleToFit, false, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 direction = -transform.forward * rayLength;
        Gizmos.DrawRay(transform.position, direction);
    }
}
