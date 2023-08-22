using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;
using System;

//using UnityEngine.Networking;
//using System.IO;

public class ModelRotationController : MonoBehaviour {

    // Config
    [SerializeField] float speed = 0.2f;
    [SerializeField] Vector3 defaultRotation = Vector3.zero;
    [SerializeField] GameObject target;

    // Params
    bool isRotating = false;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    Vector3 globalRotation = Vector3.zero;
    Vector3 localRotation = Vector3.zero;

    Transform Transform { get { return Target.transform; } }
    public GameObject Target { get => target; set => target = value; }



    // Cache
    [DllImport("__Internal")]
    private static extern int SyncRotation(float x, float y, float z);
    [DllImport("__Internal")]
    private static extern void JSConsoleLog(string str);

    public void ResetRotation() {
        Transform.DORotate(defaultRotation, speed, RotateMode.Fast);
    }

    public void updateDisplayRotation() {
        Vector3 displayRotation = globalRotation + localRotation;   
        Debug.Log("Display Rotation: " + displayRotation);
        Transform.DORotate(displayRotation, speed, RotateMode.Fast);
    }

    public Vector3 GetRotation() {
        return Target.transform.eulerAngles;
    }

    public void SetRotation(Vector3 targetRotation) {
        Transform.DORotate(targetRotation, speed, RotateMode.Fast);
    }

    public void SetRotationJS(string jsonRotation) {
        var targetRotation = JsonUtility.FromJson<Vector3>(jsonRotation);
        globalRotation = targetRotation;
        updateDisplayRotation();
    }

    public void SetRotationMI(string jsonRotation) {
        // Split the string into components
        string[] components = jsonRotation.Split(',');

        if (components.Length == 3)
        {
            float x = float.Parse(components[0]);
            float y = float.Parse(components[1]);
            float z = float.Parse(components[2]);

            Vector3 vector = new Vector3(x, y, z);
            localRotation = vector;
            updateDisplayRotation();
        } else {
            Debug.Log("Error: Invalid rotation string");
        }
        
  }

    public void DragRotate(Vector3 delta) {
        delta = Quaternion.AngleAxis(-90, Vector3.forward) * delta;

        StartCoroutine(Rotate(delta));
    }

    private IEnumerator Rotate(Vector3 v) {
        isRotating = true;
        Tween myTween = Transform.DORotate(v, speed, RotateMode.WorldAxisAdd).SetRelative();
        yield return myTween.WaitForCompletion();
        isRotating = false;

        Vector3 r = Transform.localEulerAngles;
        globalRotation = r;

        r -= localRotation;
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncRotation(r.x, r.y, r.z);
#endif
    }


    // Update is called once per frame
    void Update() {
        //Click and Drag Handling
        if (Input.GetMouseButton(1)) {
            mPosDelta = Input.mousePosition - mPrevPos;
            DragRotate(mPosDelta);
        }

        mPrevPos = Input.mousePosition;
    }
}
