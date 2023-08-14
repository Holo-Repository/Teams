using UnityEngine;

using System;
using System.Text.Json;
using System.IO;

using PaintIn3D;

public class TextureGetter : MonoBehaviour
{

    public class Model {
        public string url;
    }

    [SerializeField] public Renderer rendererWithAlbedo; // Reference to the renderer that has the albedo texture

    private P3dPaintableTexture paintableTexture;
    private void Awake()
    {
        paintableTexture = GetComponent<P3dPaintableTexture>();
        rendererWithAlbedo = GetComponent<Renderer>();
    }
    public void SaveAlbedoToPNG()
    {
        if (rendererWithAlbedo == null)
        {
            Debug.LogError("Renderer with albedo texture not assigned!");
            return;
        }

        Texture2D albedoTexture = GetAlbedoTexture(rendererWithAlbedo);

        if (albedoTexture == null)
        {
            Debug.LogError("Albedo texture not found!");
            return;
        }

        byte[] textureBytes = albedoTexture.EncodeToPNG();

        // to json
        // Convert byte array to base64 string
        string base64Texture = Convert.ToBase64String(textureBytes);

        SetAlbedoTexture(base64Texture);
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

    private void SetAlbedoTexture(string albedoTexture)
    {   
        Debug.Log("albedoTexture apply");

        byte[] textureBytes = Convert.FromBase64String(albedoTexture);

        // string imagePath = Path.Combine(Application.persistentDataPath, "image.png");
        // Texture2D texture = new Texture2D(2, 2); // Create a Texture2D object
        // byte[] imageBytes = File.ReadAllBytes(imagePath); 
        
        paintableTexture.LoadFromData(textureBytes);

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SaveAlbedoToPNG();
        } 
    

    }
}
