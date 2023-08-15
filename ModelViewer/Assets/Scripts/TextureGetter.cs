using UnityEngine;

using System;
using System.Runtime.InteropServices;
using PaintIn3D;

public class TextureGetter : MonoBehaviour
{
    [SerializeField] public Renderer textureRenderer; // Reference to the renderer that has the albedo texture

    // Params
    string currentTexture = null;

    private P3dPaintableTexture paintableTexture;

    // Cache
    [DllImport("__Internal")]
    private static extern int SyncTexture(string textureBytes);

    private void Awake()
    {
        paintableTexture = GetComponent<P3dPaintableTexture>();
        textureRenderer = GetComponent<Renderer>();
    }
    public void SetTexture()
    {
        if (textureRenderer == null)
        {
            Debug.LogError("Renderer with albedo texture not assigned!");
            return;
        }

        Texture2D texture = GetAlbedoTexture(textureRenderer);

        byte[] textureBytes = texture.EncodeToPNG();

        // Convert byte array to base64 string
        string newBase64Texture = Convert.ToBase64String(textureBytes);

        // TODO: sync base64Texture to liveshare
        if (currentTexture!=newBase64Texture)
        {
            currentTexture = newBase64Texture;
            SyncTexture(currentTexture);
        }
    }

    private Texture2D GetAlbedoTexture(Renderer renderer)
    {
        Material material = renderer.sharedMaterial;
        if (material != null)
        {
            Texture albedoTexture = material.GetTexture("_MainTex");

            if (albedoTexture != null)
            {
                Texture2D texture2D = new Texture2D(albedoTexture.width, albedoTexture.height);
                RenderTexture currentRT = RenderTexture.active;
                RenderTexture renderTexture = new RenderTexture(albedoTexture.width, albedoTexture.height, 32);

                Graphics.Blit(albedoTexture, renderTexture);
                RenderTexture.active = renderTexture;

                texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture2D.Apply();

                RenderTexture.active = currentRT;
                Destroy(renderTexture);

                return texture2D;
            }
            else
            {
                Debug.LogError("Albedo texture not found!");
            }
        }
        return null;
    }

    private void SetTextureJS(string albedoTexture)
    {   
        Debug.Log("JS setting texture");

        byte[] textureBytes = Convert.FromBase64String(albedoTexture);      
        paintableTexture.LoadFromData(textureBytes);

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetTexture();
        } 
    

    }
}
