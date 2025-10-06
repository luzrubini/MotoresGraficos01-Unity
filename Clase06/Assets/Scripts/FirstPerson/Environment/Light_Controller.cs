using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Controller : MonoBehaviour
{
    public enum WaveForm { Sin, Triangle, Square, Sawtooth, InvertedSawtooth, Noise };

    public WaveForm waveForm = WaveForm.Sin;

    public float start = 0.0f;
    public float amplitude = 1.0f;
    public float phase = 0.0f;
    public float frequency = 0.5f;

    private Color originalColor;
    private Light lightComponent;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        originalColor = lightComponent.color;
    }

    void Update()
    {
        lightComponent.color = originalColor * EvaluateWave();
    }

    float EvaluateWave()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x);

        switch (waveForm)
        {
            case WaveForm.Sin:
                y = Mathf.Sin(x * 2 * Mathf.PI);
                break;
            case WaveForm.Triangle:
                if (x < 0.5f)
                    y = 4.0f * x - 1.0f;
                else
                    y = -4.0f * x + 3.0f;
                break;
            case WaveForm.Square:
                y = (x < 0.5f) ? 1.0f : -1.0f;
                break;
            case WaveForm.Sawtooth:
                y = x;
                break;
            case WaveForm.InvertedSawtooth:
                y = 1.0f - x;
                break;
            case WaveForm.Noise:
                y = 1f - (Random.value * 2);
                break;
            default:
                y = 1.0f;
                break;
        }
        return (y * amplitude) + start;
    }
}
