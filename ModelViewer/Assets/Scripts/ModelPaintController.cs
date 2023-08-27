using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using Unity.VisualScripting;

public class ModelPaintController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var paint = transform.AddComponent<P3dPaintable>();
        paint.Activate();
       
    }

    // Update is called once per frame
    void Update()
    {
    }
}
