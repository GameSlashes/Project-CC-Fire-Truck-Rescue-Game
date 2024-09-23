using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class HeartBeatRenderer : MonoBehaviour
{
    public Text heartRate;
    public int points;
    public float amplitude = 1;
    public float waveLength = 1;
    public float waveSpeed=10;

    private float timeForAnimation = 5;
    
    
    
    private LineRenderer _lineRenderer;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private IEnumerator Start()
    {
        InvokeRepeating(nameof(UpdateHeartRate), 1, 1);
        yield return new WaitForSeconds(timeForAnimation);
    }

    private void Update()
    {
        DrawTravellingSineWave(Vector3.zero, amplitude, waveLength, waveSpeed);
    }

    private void UpdateHeartRate()
    {
        heartRate.text = UnityEngine.Random.Range(70, 90) + "\n<color=red><size=9>bpm</size></color>";
    }
    
    void DrawTravellingSineWave(Vector3 startPoint, float amplitude, float wavelength, float waveSpeed){

        float x = 0f;
        float y;
        float k = 2 * Mathf.PI / wavelength;
        float w = k * waveSpeed;
        _lineRenderer.positionCount = points;
        float time = Time.time;
        for (int i = 0; i < _lineRenderer.positionCount; i++){
            x += i * 0.001f;
            y = amplitude * Mathf.Sin(k * x + w * time);
            _lineRenderer.SetPosition(i, new Vector3(x, y, 0) + startPoint);
        }
    }
    
    
}
