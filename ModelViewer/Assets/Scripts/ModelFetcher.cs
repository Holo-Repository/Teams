using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using GLTFast;
using PaintIn3D;


public class ModelFetcher : MonoBehaviour {

    // Config
    [SerializeField] float modelScale = 0.03f;
    //[SerializeField] public GameObject LoadingScreen;
    [SerializeField] Image ProgressBar;
    [SerializeField] Image Background;
    [SerializeField] GameObject LoadingText;

    [SerializeField] Material[] paintMaterials;

    // Params
    string fileName;
    string persistentPath;
    string fullPath;
    string hid;

    // Cache
    ModelRotationController rotationController;
    ModelScaleController scaleController;

    P3dSeamFixer seamFixer;
    
    // define the model type to pass to the JSON utility
    [Serializable]
    public class Model {
        public string url;
        public Vector3 rotation;
        public float scale;
    }

    // Cache
    [DllImport("__Internal")]
    private static extern void SignalDownloaded();

    public void Start() {
        rotationController = GetComponent<ModelRotationController>();
        scaleController = GetComponent<ModelScaleController>();
        persistentPath = Application.persistentDataPath;
        Debug.Log(persistentPath);

        seamFixer = (P3dSeamFixer)(ScriptableObject.CreateInstance("P3dSeamFixer"));

        //Placeholder Hardcode
        hid = "lung1"; 
        //hid = "whole_body_demo";
    }

    public void Download3DModel(string jsonModel) {
        // Extract the hid and rotation from the JSON object
        var model = JsonUtility.FromJson<Model>(jsonModel);
        // hid = model.hid;
        string url = model.url;
        Vector3 rotation = model.rotation;
        float scale = model.scale;

        // string url = $"https://organsegmentation-storageaccessor-app.azurewebsites.net/api/v1/holograms/{hid}/download"; 
        // fileName = $"{hid}.glb";
        fileName = "downloaded.glb";
        fullPath = Path.Combine(Application.persistentDataPath, fileName);

        StartCoroutine(DownloadFile(url, rotation, scale));
    }

    private IEnumerator DownloadFile(string url, Vector3 rotation = default(Vector3), float scale = default(float)) {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        
        //display loading screen
        ProgressBar.enabled = true;
        Background.enabled = true;
        LoadingText.SetActive(true);

        // Start the request
        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            // Calculate the progress as a float value between 0 and 1
            float progress = webRequest.downloadProgress;

            Debug.Log($"Request progress: {progress * 100}%");

            ProgressBar.fillAmount = progress;
            // Wait for the next frame
            yield return null;
        }

        if (webRequest.result == UnityWebRequest.Result.Success) {
            byte[] content = webRequest.downloadHandler.data;
            File.WriteAllBytes(fullPath, content);
            Debug.Log("File downloaded successfully.");

            // Display the file path and file size
            FileInfo fileInfo = new FileInfo(fullPath);
            Debug.Log($"File path: {fileInfo.FullName}");
            Debug.Log($"File size: {fileInfo.Length} bytes");
        } else {
            Debug.Log($"Failed to download file. Error: {webRequest.error}");
        }

        LoadModel(rotation, scale);

        ProgressBar.enabled = false;
        Background.enabled = false;
        LoadingText.SetActive(false);

        // sent js message 
        SignalDownloaded();
    }

    

    async void LoadModel(Vector3 rotation = default(Vector3), float scale = default(float)) {
        // Check hid argument and update
        // if (pHid != null)
        // hid = pHid;
        // Debug.Log($"model loading hid: {hid}");
        //hid = "lung1"; 

        // Update filename to current hid
        // fileName = $"{hid}.glb";
        fileName = "downloaded.glb";

        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        // Load the GLB file from the Resources folder
        byte[] data = File.ReadAllBytes(fullPath);
        var gltf = new GltfImport();

        bool success = await gltf.LoadGltfBinary(
            data, 
            // The URI of the original data is important for resolving relative URIs within the glTF
            new Uri(fullPath)
            );

        // Check if the Model was loaded successfully
        if (success) {
            Debug.Log("Success spawning model");
            success = await gltf.InstantiateMainSceneAsync(transform);
        }     
        else {
            Debug.LogError("Failed to load object from Resources folder.");
            return;
        }

        // Destroy Current Target Model
        Destroy(GameObject.Find("Target Model"));

        GameObject targetModel = transform.GetChild(2).gameObject;
        rotationController.Target = targetModel;
        scaleController.Target = targetModel;

        // Set Model Properties
        targetModel.name = "Target Model";
        Debug.Log(scale);
        targetModel.transform.localScale *= scale;
        targetModel.transform.localRotation = Quaternion.Euler(rotation);

        // Make Components of Model Paintable
        MakePaintableParent(targetModel);
    }

    void MakePaintableParent(GameObject target) {
        // Check if the target mesh has a valid UV Map
        var uvs = new List<Vector2>();
        target.transform.GetChild(0).GetComponent<MeshFilter>().mesh.GetUVs(0, uvs);
        if (uvs.Count == 0) {
            Debug.Log("Warning: Model does not contain a valid UV Map! Painting feature is disabled.");
            return;
        } Debug.Log("UVs Valid!");

        foreach (Transform child in target.transform) {
            MakePaintableChild(child.gameObject);
        }

        seamFixer.Generate();
    }

    void MakePaintableChild(GameObject target) {
        // Change Material to be Paint Compatible
        target.GetComponent<MeshRenderer>().material = paintMaterials[0];

        target.AddComponent<P3dPaintable>();
        target.AddComponent<P3dPaintableTexture>();
        target.AddComponent<P3dMaterialCloner>();
        target.AddComponent<MeshCollider>();

        target.AddComponent<TextureGetter>();

        // Debug.Log(target.name);

        seamFixer.AddMesh(target.GetComponent<MeshFilter>().mesh);
        
    }


    // this is a temp method as in the future Download3DModel should be called by the teams client 
    void Update() {
        if (Input.GetKeyDown("t")) {
            string input = @"{
                ""url"": ""https://organsegmentation-storageaccessor-app.azurewebsites.net/api/v1/holograms/lung1/download"",
                ""rotation"": {""x"": 18, ""y"": 18, ""z"": 30},
                ""scale"": 0.03
            }";

            Download3DModel(input);
        }
        // if (Input.GetKeyDown("y")) {
        //     LoadModel();
        // }
    }
}