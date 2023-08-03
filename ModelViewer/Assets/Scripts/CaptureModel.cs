using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Runtime.InteropServices;

public class CaptureModel : MonoBehaviour
{   
    [SerializeField] Camera screenshotCamera; 
    
    [DllImport("__Internal")]
    private static extern int HandleScreenshotDataURL(string dataURL);

    // Call this method to capture the screenshot.
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshot());
    }

    private System.Collections.IEnumerator TakeScreenshot()
    {
        // Ensure the screenshotCamera is rendering to a RenderTexture.
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        screenshotCamera.targetTexture = renderTexture;

        // Wait for the next frame to ensure the rendering is complete.
        yield return new WaitForEndOfFrame();

        // Create a Texture2D to read the pixels from the RenderTexture.
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Encode the Texture2D to a PNG format and convert it to a base64 encoded data URL.
        byte[] bytes = texture.EncodeToPNG();
        string base64Data = System.Convert.ToBase64String(bytes);
        string dataURL = "data:image/png;base64," + base64Data;

        // Call JavaScript function to handle the screenshot (e.g., display it in an <img> element or save it).
        HandleScreenshotDataURL(dataURL);

        // Clean up.
        screenshotCamera.targetTexture = null;
        Destroy(texture);
    }

    // Call this method from JavaScript to trigger the screenshot capture.
    public void TriggerScreenshot()
    {
        CaptureScreenshot();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CaptureScreenshot();  
        }
    }
}
