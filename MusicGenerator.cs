using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicGenerator : MonoBehaviour
{
    public PianoRoll pianoRoll;
    public GameObject sinePrefab;
    public int BPM = 120; // Puedes ajustar esto según tus necesidades
    public int iteraciones;

    void Start()
    {
        StartCoroutine(Proceso());
    }

    IEnumerator Proceso()
    {
        pianoRoll.Start();
        while (iteraciones < 4)
        {
            pianoRoll.Actualizar(iteraciones);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    //Debug.LogWarning(x + ", " + y);
                    var nota = pianoRoll.GetNota(x, y);
                    if (nota == (0, 0)) continue;
                    GameObject sineObj = Instantiate(sinePrefab);
                    Sine sine = sineObj.GetComponent<Sine>();
                    sine.maxAmplitude = 0.5f;
                    sine.PlayNote(2, BPM, nota.p, y > 1);
                    StartCoroutine(DestroyAfterTime(sineObj, sine.duration));
                }
                yield return new WaitForSeconds(60f / BPM);
            }
            iteraciones++;
        }
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
