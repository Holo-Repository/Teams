using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

public class ModelScaleController : MonoBehaviour
{
    // Config
    // The speed of scaler when using keys
    [SerializeField] float speed = 5f;
    // Default scale
    [SerializeField] Vector3 defaultScale = new Vector3(250f, 250f, 250f);
    // Get the transform of target object
    [SerializeField] GameObject target;
    Transform Transform { get { return Target.transform; } }

    // Params
    Vector3 currentScale;

    // Cache
    [DllImport("__Internal")]
    private static extern int SyncScale(float x, float y, float z);


    public GameObject Target { get => target; set { target = value; defaultScale = target.transform.lossyScale; } }

    void ResetScale()
    {
        Transform.localScale = defaultScale;
    }

    void ChangeScale(float delta)
    {
        Transform.localScale += new Vector3(delta, delta, delta);
    }


    void Update()
    {
        // Use keyboad to control the scale
    if (Input.GetKey(KeyCode.Space))
        {
            ResetScale();
        }
    }

    public void SetScaleJS(string jsonScale) {
        var targetScale = JsonUtility.FromJson<Vector3>(jsonScale);
        Transform.localScale = targetScale;

        // Debug.Log(targetScale);
    }

    void ChangeScaleRelative(float delta) {
        float percentageChange = delta / 100;
        delta = 1 + percentageChange;
        Transform.localScale = Vector3.Scale(Transform.localScale, new Vector3(delta, delta, delta));
    
        Vector3 s = Transform.localScale;
        if (currentScale != s)
        {
            currentScale = s;
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncScale(s.x, s.y, s.z);
#endif
            // Debug.Log(s);
        }

    }

    void OnGUI() {
        ChangeScaleRelative(speed * Input.mouseScrollDelta.y);
    }
}

