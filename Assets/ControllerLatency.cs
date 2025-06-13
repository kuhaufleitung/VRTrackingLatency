using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class ControllerLatency : MonoBehaviour
{
    public enum ControllerType { Left, Right }
    public ControllerType controllerType;

    private struct PoseSample
    {
        public Vector3 position;
        public Quaternion rotation;
        public double time;
    }

    private System.Collections.Generic.List<PoseSample> poseSamples =
        new System.Collections.Generic.List<PoseSample>();

    private XRNode controllerNode;
    private TrackedPoseDriver trackedPoseDriver;

    void Start()
    {
        controllerNode = controllerType == ControllerType.Left ?
            XRNode.LeftHand : XRNode.RightHand;

        trackedPoseDriver = GetComponent<TrackedPoseDriver>();
        if (trackedPoseDriver != null)
        {
            trackedPoseDriver.enabled = false;
        }
    }

    void Update()
    {
        // Get current controller pose
        Vector3 currentPosition = InputTracking.GetLocalPosition(controllerNode);
        Quaternion currentRotation = InputTracking.GetLocalRotation(controllerNode);
        
        // Store with timestamp
        poseSamples.Add(new PoseSample
        {
            position = currentPosition,
            rotation = currentRotation,
            time = Time.timeAsDouble
        });

        // Find and apply delayed pose
        double targetTime = Time.timeAsDouble - LatencyMgr.Instance.globalLatencyController;
        int index = poseSamples.FindIndex(p => p.time >= targetTime);

        if (index >= 0)
        {
            transform.localPosition = poseSamples[index].position;
            transform.localRotation = poseSamples[index].rotation;

            // Remove old samples
            if (index > 0) poseSamples.RemoveRange(0, index);
        }
    }
}
