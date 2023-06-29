using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;
using System;

using UnityEngine.Networking;
using System.IO;

//using Siccity.GLTFUtility;

public class Get3DModel : MonoBehaviour
{       

    // this is a temp method as in the future Download3DModel should be called by the teams client 
    void Update() {
        if (Input.GetKeyDown("t")) {
            Download3DModel();
        }
    }

    //public void Download3DModel(string hid)
    public void Download3DModel()
    {
        //string url = $"https://organsegmentation-storageaccessor-app.azurewebsites.net/api/v1/holograms/{hid}/download";
        // current hardcoded url    
        string url = "https://organsegmentation-storageaccessor-app.azurewebsites.net/api/v1/holograms/1867f18b7783921f6d80ab30020596c6/download";
        string downloadPath = "downloaded-file.glb";

        StartCoroutine(DownloadFile(url, downloadPath));
    }

    private IEnumerator DownloadFile(string url, string filePath)
    {       
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            byte[] content = webRequest.downloadHandler.data;

            string persistentPath = Path.Combine(Application.persistentDataPath, filePath);
            // string persistentPath = "./Assets/Models";
            File.WriteAllBytes(persistentPath, content);

            Debug.Log("File downloaded successfully.");

            //LoadModel(persistentPath);

            // Display the file path and file size
            FileInfo fileInfo = new FileInfo(persistentPath);
            Debug.Log($"File path: {fileInfo.FullName}");
            Debug.Log($"File size: {fileInfo.Length} bytes");
            ConvertGLBToString(persistentPath); // for debugging purposes, should be removed in the future  
        }
        else
        {
            Debug.Log($"Failed to download file. Error: {webRequest.error}");
        }
    }

    // for debugging purposes, should be removed in the future  
    private void ConvertGLBToString(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] glbBytes = File.ReadAllBytes(filePath);
            string glbString = Convert.ToBase64String(glbBytes);
            Debug.Log("GLB length as Base64 string:\n" + glbString.Length.ToString());
        }
        else
        {
            Debug.LogError("GLB file not found: " + filePath);
        }
    }
    
    // async void LoadModel(string filePath) {
    //     Debug.Log("Loading model...");
    //     byte[] data = File.ReadAllBytes(filePath);
    //     var gltf = new GLTFast.GltfImport();
    //     bool success = await gltf.LoadGltfBinary(
    //         data, 
    //         // The URI of the original data is important for resolving relative URIs within the glTF
    //         new Uri(filePath)
    //         );
    //     if (success) {
    //         Debug.Log("Model loaded successfully.");
    //         //success = await gltf.InstantiateMainSceneAsync(transform);
    //     }
    // }
    //void LoadModel(string filepath) {
        //GameObject result = Importer.LoadFromFile(filepath);
    //}
}