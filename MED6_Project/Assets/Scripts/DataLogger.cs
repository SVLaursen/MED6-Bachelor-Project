using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private bool loggingEnabled;

    [Header("Splat Map Logging")]
    [SerializeField] private ChiselImpacter chiselImpacter;

    [Header("Motion Logging")]
    [SerializeField] private Transform leftHand, rightHand, head;

    private string _desktopPath;
    private string _filePath;
    private bool _isTracking;
    private float _timer;

    private System.IO.StreamWriter leftWriter, rightWriter, headWriter;

    private void Awake()
    {
        if (!loggingEnabled) return;

        _desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        System.IO.Directory.CreateDirectory(_desktopPath + "/TestingData");
        _filePath = _desktopPath + "/TestingData";
        _isTracking = true;
    }

    private void Start()
    {
        if (!loggingEnabled) return;
        StartCoroutine(TrackData());
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    private void SaveRenderTexture()
    {
        if (!loggingEnabled) return;
        if (chiselImpacter == null)
        {
            Debug.LogWarning("Was unable to save the splatmap due to the chisel not being " +
                "loaded in the data logger");
            return;
        }

        var splatMap = chiselImpacter.SplatMap;

        RenderTexture.active = splatMap;
        Texture2D image = new Texture2D(splatMap.width, splatMap.height,
            TextureFormat.RGB24, false);

        image.ReadPixels(new Rect(0, 0, splatMap.width, splatMap.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = image.EncodeToPNG();

        string savePath = _filePath + "/user_splatMap.png";
        System.IO.File.WriteAllBytes(savePath, bytes);
    }

    private void SaveTimer()
    {
        float timeInSeconds = _timer % 60;
        string savePath = _filePath + "/TimePlayed.txt";

        System.IO.StreamWriter dataWriter = new System.IO.StreamWriter(savePath);
        dataWriter.WriteLine("Runtime length in seconds:");
        dataWriter.WriteLine(timeInSeconds);
        dataWriter.Close();
    }

    private void OnApplicationQuit()
    {
        if (!loggingEnabled) return;
        _isTracking = false;
        SaveRenderTexture();
        SaveTimer();
    }

    private IEnumerator TrackData()
    {
        string header = "id;x;y;z;";
        int index = 0;

        leftWriter = new System.IO.StreamWriter(_filePath + "/LeftTrackData.txt");
        leftWriter.WriteLine(header);

        rightWriter = new System.IO.StreamWriter(_filePath + "/RightTrackData.txt");
        rightWriter.WriteLine(header);

        headWriter = new System.IO.StreamWriter(_filePath + "/HeadTrackData.txt");
        headWriter.WriteLine(header);


        while (_isTracking)
        {
            leftWriter.WriteLine(index + ";" + leftHand.position.x + ";" + leftHand.position.y + ";" 
                + leftHand.position.z + ";");

            rightWriter.WriteLine(index + ";" + rightHand.position.x + ";" + rightHand.position.y + ";"
                + rightHand.position.z + ";");

            headWriter.WriteLine(index + ";" + head.position.x + ";" + head.position.y + ";"
                + head.position.z + ";");

            index++;
            yield return new WaitForEndOfFrame();
        }

        leftWriter.Close();
        rightWriter.Close();
        headWriter.Close();

        yield return null;
    }
}
