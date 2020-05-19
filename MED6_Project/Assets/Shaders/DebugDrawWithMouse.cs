using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawWithMouse : MonoBehaviour
{
    public Camera camera;
    public Shader drawShader;

    [Range(1f, 500f)] public float brushSize;
    [Range(0f, 1f)] public float brushStrength;

    private RenderTexture _splatMap;
    private Material _trackMaterial, _drawMaterial;
    private RaycastHit _hit;

    private void Start()
    {
        _drawMaterial = new Material(drawShader);
        _drawMaterial.SetVector("_Color", Color.red);

        _trackMaterial = GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _trackMaterial.SetTexture("_Splat", _splatMap);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_BrushStrength", brushStrength);
                _drawMaterial.SetFloat("_BrushSize", brushSize);

                RenderTexture targetTexture = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, targetTexture);
                Graphics.Blit(targetTexture, _splatMap, _drawMaterial);
                RenderTexture.ReleaseTemporary(targetTexture);
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 128, 128), _splatMap, ScaleMode.ScaleToFit, false, 1);
    }
}
