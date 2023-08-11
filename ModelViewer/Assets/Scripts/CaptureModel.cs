using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Runtime.InteropServices;

public class CaptureModel : MonoBehaviour
{   
    [SerializeField] Button saveButton;
    [SerializeField] Button deleteButton;

    // Call this method from JavaScript to trigger the screenshot capture.
    public void ToggleUI()
    {
        if (saveButton.gameObject.activeSelf){
            saveButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        } else {
            saveButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUI();  
        }
    }
}
