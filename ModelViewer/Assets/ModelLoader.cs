using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Downloaded
public class ModelLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("y")) {
            CreateObject();
        }
    }

    public void CreateObject() {
        // Load the GLB file from the Resources folder
        GameObject loadedObject = Resources.Load<GameObject>("Downloaded");

        if (loadedObject != null)
        {
            // Replace the existing GameObject with the loaded object
            Destroy(gameObject); // Destroy the current GameObject
            GameObject instantiatedObject = Instantiate(loadedObject, transform.position, transform.rotation);
            float scaleFactor = 0.05f;
            instantiatedObject.transform.localScale = scaleFactor * loadedObject.transform.localScale; // Set the scaled size
        }
        else
        {
            Debug.LogError("Failed to load object from Resources folder.");
        }
    }
}
