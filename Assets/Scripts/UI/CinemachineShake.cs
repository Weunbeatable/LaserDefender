using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CinemachineShake : MonoBehaviour
{
    // create an instance of this camera shake script so its easily reusable elsewhere
    public static CinemachineShake Instance {  get; private set; }
    private CinemachineVirtualCamera _virtualCamera;
    private float _shakeTime;
    private float _startingIntensity;
    private float _shakeTimerTotal; 
    private void Awake()
    {
        Instance = this;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel =
            _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannel.m_AmplitudeGain = intensity;
        _shakeTime = time;
        _shakeTimerTotal = time;
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            if (_shakeTime <= 0)
            {
                // time to shake camera has elapsed
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                // reset the amplitutde
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                // interpolate intensity change
                Mathf.Lerp(_startingIntensity, 0f_, _shakeTime / _shakeTimerTotal);
            }
        }
    }
}
