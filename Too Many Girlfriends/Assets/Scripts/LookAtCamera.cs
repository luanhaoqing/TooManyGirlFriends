using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject mainCamera;
    private Vector3 cameraFacing;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFacing = mainCamera.transform.forward;
        this.transform.forward = -cameraFacing;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = -cameraFacing;
    }
}
