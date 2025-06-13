using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LatencySimulation : MonoBehaviour
{
    private struct PoseSample
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;
    }

    private readonly System.Collections.Generic.Queue<PoseSample> poseHistory =
        new System.Collections.Generic.Queue<PoseSample>();

    private XRInputSubsystem inputSubsystem;
    private Camera vrCamera;

    void Start()
    {
        vrCamera = GetComponent<Camera>();
        if (vrCamera == null)
        {
            vrCamera = Camera.main;
        }

        var inputSubsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances(inputSubsystems);
        if (inputSubsystems.Count > 0)
        {
            inputSubsystem = inputSubsystems[0];
        }
    }

    void Update()
    {
        // Get current pose
        Vector3 currentPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
        Quaternion currentRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);

        // Store current pose with timestamp
        poseHistory.Enqueue(new PoseSample
        {
            position = currentPosition,
            rotation = currentRotation,
            time = Time.time
        });

        // Remove old poses
        while (poseHistory.Count > 0 && poseHistory.Peek().time < Time.time - LatencyMgr.Instance.globalLatencyHmd)
        {
            PoseSample delayedPose = poseHistory.Dequeue();

            // Apply the delayed pose to the camera
            if (vrCamera != null)
            {
                vrCamera.transform.localPosition = delayedPose.position;
                vrCamera.transform.localRotation = delayedPose.rotation;
            }
        }
    }
}
