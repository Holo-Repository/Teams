using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public void FocusCanvas(string focus)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            if (focus == "0"){
                WebGLInput.captureAllKeyboardInput = false;
                Debug.Log("Unfocus Canvas");
            } else {
                WebGLInput.captureAllKeyboardInput = true;
                Debug.Log("Focus Canvas");
            }
        #endif  
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     FocusCanvas();  
        // }
    }
}
