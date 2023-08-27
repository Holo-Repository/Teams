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

    public Vector3 GetRotation() {
        return Target.transform.eulerAngles;
    }

    public void SetRotation(Vector3 targetRotation) {
        Transform.DORotate(targetRotation, speed, RotateMode.Fast);
    }

    public void SetRotationJS(string jsonRotation) {
        var targetRotation = JsonUtility.FromJson<Vector3>(jsonRotation);
        Transform.DORotate(targetRotation, speed, RotateMode.Fast);

        // float[] e = new float[] { targetRotation.x, targetRotation.y, targetRotation.z };
        // Debug.Log(e);
    }

    public void Rotate90(string direction) {
        if (isRotating) {
            return;
        }

        switch (direction) {
            case "up":
                StartCoroutine(Rotate(new Vector3(90, 0, 0)));
                break;
            case "down":
                StartCoroutine(Rotate(new Vector3(-90, 0, 0)));
                break;
            case "left":
                StartCoroutine(Rotate(new Vector3(0, 90, 0)));
                break;
            case "right":
                StartCoroutine(Rotate(new Vector3(0, -90, 0)));
                break;
            case "clock":
                StartCoroutine(Rotate(new Vector3(0, 0, 90)));
                break;
            case "cClock":
                StartCoroutine(Rotate(new Vector3(0, 0, -90)));
                break;
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
        float[] e = new float[] { r.x, r.y, r.z };

#if UNITY_WEBGL && !UNITY_EDITOR
            SyncRotation(r.x, r.y, r.z);
#endif
    }


    // Update is called once per frame
    void Update() {
        // 90 Degree Rotation Handling
        // if (Input.GetKeyDown("w")) {
        //     Rotate90("up");
        // } else if (Input.GetKeyDown("s")) {
        //     Rotate90("down");
        // } else if (Input.GetKeyDown("a")) {
        //     Rotate90("left");
        // } else if (Input.GetKeyDown("d")) {
        //     Rotate90("right");
        // } else if (Input.GetKeyDown("q")) {
        //     Rotate90("clock");
        // } else if (Input.GetKeyDown("e")) {
        //     Rotate90("cClock");
        // } else if (Input.GetKeyDown("space")) {
        //     ResetRotation();
        // }

        //Click and Drag Handling
        if (Input.GetMouseButton(1)) {
            mPosDelta = Input.mousePosition - mPrevPos;
            DragRotate(mPosDelta);
        }

        mPrevPos = Input.mousePosition;
    }
}
