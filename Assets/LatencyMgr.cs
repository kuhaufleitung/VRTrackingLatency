using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatencyMgr : MonoBehaviour
{
    public static LatencyMgr Instance { get; private set; }
    [Tooltip("Global latency in seconds for HMD")]
    public float globalLatencyHmd = 0.1f;
    [Tooltip("Global latency in seconds for Controller")]
    public float globalLatencyController = 0.1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
    }
}
