using UnityEngine;
using System.Collections;

public class ProceduralMusicGenerator : MonoBehaviour
{
    public GameObject sinePrefab;
    public PianoRoll pianoroll;
    public bool SeRepite = true;
    public int key;

    public string emocion = "aleg"; // "trst" "tnso" "calm"

    void Start()
    {
        key = Random.Range(0, 12);
        pianoroll = ScriptableObject.CreateInstance<PianoRoll>();
        pianoroll.Empezar(key, emocion);
        StartCoroutine(GenerateMusic());
    }

    IEnumerator GenerateMusic()
    {
        while(SeRepite)
        {
            for (int i = 0; i < pianoroll.largo; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var nota = pianoroll.seccion[j][i];
                    if (nota == (0, 0)) { continue; }
                    GameObject sineObj = Instantiate(sinePrefab);
                    Sine sine = sineObj.GetComponent<Sine>();
                    sine.PlayNote(nota.figura, pianoroll.songStructure.BPM, nota.p, j >= 2);
                    StartCoroutine(DestroyAfterTime(sineObj, sine.duration));
                }
                yield return new WaitForSeconds(60f / (float)(pianoroll.songStructure.BPM * 4)); // Asume que cada acorde dura una medida
            }
        }
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
}
