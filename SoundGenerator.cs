using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Sine : MonoBehaviour
{
    // Par�metros del envelope
    public float duration;
    public AnimationCurve envelopeCurve;


    public float maxAmplitude = 2f;

    // Otros par�metros
    public double p = 0;
    private double increment;
    private double phase;
    private double samplingFrequency = 48000;
    private bool noteOn = false;
    private float envelopeAmplitude = 0f;
    private float envelopeTime = 0.0f;

    void Start()
    {
        Keyframe[] keyframes = new Keyframe[]
        {
            new Keyframe(0.0f, 0.0f),               // Inicio del ataque
            new Keyframe(0.1f * duration, 1.0f),    // Fin del ataque
            new Keyframe(0.4f * duration, 0.7f),    // Fin del decaimiento
            new Keyframe(0.8f * duration, 0.7f),    // Fin del sostenido
            new Keyframe(1.0f * duration, 0.0f)     // Fin de la liberaci�n
        };

        envelopeCurve = new AnimationCurve(keyframes);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        // Calcula la frecuencia con el par�metro p
        float frequency = 440 * Mathf.Pow(2, (float)(p - 9) / 12.0f);
        increment = frequency * 2 * Math.PI / samplingFrequency;
        float[] amp_rel = Enumerable.Range(1, 30).Select(i => 1f / i).ToArray();

        for (int i = 0; i < data.Length; i += channels)
        {
            // Si la nota est� encendida, actualiza el envelope
            if (noteOn)
            {
                envelopeTime += 1.0f / (float)samplingFrequency;
                envelopeAmplitude = envelopeCurve.Evaluate(envelopeTime / duration) * maxAmplitude;
            }
            float sinAmplitude;
            
            // Calcula la amplitud del tono
            double harmonicos = 0;
            for (int j = 0; j < amp_rel.Length; j++)
            {
                harmonicos += amp_rel[j] * Math.Sin((j+1)*phase);
            }
            sinAmplitude = (float)(maxAmplitude * harmonicos);

            // Aplica el envelope a la amplitud del tono
            float sampleAmplitude = sinAmplitude * envelopeAmplitude;

            // Asigna la amplitud al canal de audio
            for (int j = 0; j < channels; j++)
            {
                data[i + j] = sampleAmplitude;
            }

            // Actualiza la fase del tono sinusoide
            phase += increment;
            if (phase > 2 * Math.PI) phase -= 2 * Math.PI;
        }
    }

    // M�todo para iniciar una nota
    public void PlayNote(float figura, int BPM, int pitch, bool acorde)
    {
        p = pitch;
        duration = 480f / (float)(BPM * figura);
        if (acorde) maxAmplitude *= 0.5f;
        noteOn = true;
        envelopeTime = 0.0f;
    }
}
