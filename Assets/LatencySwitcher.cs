using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LatencySwitcher : MonoBehaviour
{
    private LatencyInput _input;
    [SerializeField]
    private float stepSize;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = new LatencyInput();
        _input.Default.Enable();
        _input.Default.DownController.started += DownLatencyController;
        _input.Default.DownHMD.started += DownLatencyHmd;
        _input.Default.UpController.started += UpLatencyController;
        _input.Default.UpHMD.started += UpLatencyHmd;
    }

    void UpLatencyHmd(InputAction.CallbackContext ctx)
    {
        LatencyMgr.Instance.globalLatencyHmd += stepSize;
        Clamp(ref LatencyMgr.Instance.globalLatencyHmd);
        Debug.Log("HMD: " + LatencyMgr.Instance.globalLatencyHmd);
    }

    void DownLatencyHmd(InputAction.CallbackContext ctx)
    {
        LatencyMgr.Instance.globalLatencyHmd -= stepSize;
        Clamp(ref LatencyMgr.Instance.globalLatencyHmd);
        Debug.Log("HMD: " + LatencyMgr.Instance.globalLatencyHmd);
    }

    void UpLatencyController(InputAction.CallbackContext ctx)
    {
        LatencyMgr.Instance.globalLatencyController += stepSize;
        Clamp(ref LatencyMgr.Instance.globalLatencyController);
        Debug.Log("Controller: " + LatencyMgr.Instance.globalLatencyController);
    }

    void DownLatencyController(InputAction.CallbackContext ctx)
    {
        LatencyMgr.Instance.globalLatencyController -= stepSize;
        Clamp(ref LatencyMgr.Instance.globalLatencyController);
        Debug.Log("Controller: " + LatencyMgr.Instance.globalLatencyController);
    }

    private void Clamp(ref float value)
    {
        if (value < 0.0f)
        {
            value = 0.0f;
        }
    }
}
