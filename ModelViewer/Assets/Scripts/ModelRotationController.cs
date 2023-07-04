using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;
using System;
using System.Security.Cryptography;

//using UnityEngine.Networking;
//using System.IO;

public class ModelRotationController : MonoBehaviour {

    // Config
    [SerializeField] float speed = 0.2f;
    [SerializeField] UnityEngine.Vector3 defaultRotation = Vector3.zero;
    [SerializeField] public GameObject target;

    // Params
    bool isRotating = false;

    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    Transform Transform { get { return target.transform; } }



    // Cache
    [DllImport("__Internal")]
    private static extern int SyncRotation(float x, float y, float z);
    [DllImport("__Internal")]
    private static extern void JSConsoleLog(string str);

    public void ResetRotation() {
        Transform.DORotate(defaultRotation, speed, RotateMode.FastBeyond360);
    }

    public void UpdateRotation(string jsonRotation) {
        var targetRotation = JsonUtility.FromJson<Vector3>(jsonRotation);
        Transform.DORotate(targetRotation, speed, RotateMode.FastBeyond360);

        float[] e = new float[] { targetRotation.x, targetRotation.y, targetRotation.z };
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

        try {
            SyncRotation(r.x, r.y, r.z);
        } catch { }
    }


    // Update is called once per frame
    void Update() {
        // 90 Degree Rotation Handling
        if (Input.GetKeyDown("w")) {
            Rotate90("up");
        } else if (Input.GetKeyDown("s")) {
            Rotate90("down");
        } else if (Input.GetKeyDown("a")) {
            Rotate90("left");
        } else if (Input.GetKeyDown("d")) {
            Rotate90("right");
        } else if (Input.GetKeyDown("q")) {
            Rotate90("clock");
        } else if (Input.GetKeyDown("e")) {
            Rotate90("cClock");
        } else if (Input.GetKeyDown("space")) {
            ResetRotation();
        }

        // Click and Drag Handling
        if (Input.GetMouseButton(0)) {
            mPosDelta = Input.mousePosition - mPrevPos;
            DragRotate(mPosDelta);
        }

        mPrevPos = Input.mousePosition;
    }
}
