using UnityEngine;
using System.Collections;

public class ProceduralMusicGenerator : MonoBehaviour
{
    public GameObject sinePrefab;
    public Circular marcador;
    public PianoRoll pianoroll;
    public List<PianoRoll> otrasEmociones;
    public int key;
    public bool enCamino = false;
    private List<GameObject> activeSineObjects = new List<GameObject>();
    public string emocion = "trst"; // "aleg" "tnso" "calm"
    private List<string> emociones = new List<string> {"trst", "aleg", "tnso", "calm"};

    void Start()
    {
        key = Random.Range(0, 12);
        pianoroll = ScriptableObject.CreateInstance<PianoRoll>();
        pianoroll.Empezar(key, emocion);PianoRoll p;
        foreach(var e in emociones)
        {
            if (e != emocion){
                p = ScriptableObject.CreateInstance<PianoRoll>();
                p.Empezar(key, e);
                otrasEmociones.Add(p);
            }
        }
        StartCoroutine(GenerateMusic());
        marcador = FindObjectOfType<Circular>();
    }

    void Update()
    {
        Vector3 circlePosition = marcador.GetCirclePosition();
        string emocionAnterior = emocion;
        emocion = CheckEmocion(circlePosition);
        if (emocion != emocionAnterior)
        {
            StopAllCoroutines();
            DestroyAllActiveSineObjects();
            foreach(var em in otrasEmociones){
                if (em.emocionn == emocion){
                    StartCoroutine(transicion(pianoroll, em));
                    pianoroll = em;
                    break;
                }
            }
            PianoRoll p;
            otrasEmociones.Clear();
            foreach(var e in emociones)
            {
                if (e != emocion){
                    p = ScriptableObject.CreateInstance<PianoRoll>();
                    p.Empezar(key, e);
                    otrasEmociones.Add(p);
                }
            }
            enCamino = false;
        }
        if (!enCamino)
        {
            enCamino = true;
            StartCoroutine(laQueViene(15f * (float)(pianoroll.largo - 1) / (float)(pianoroll.songStructure.BPM)));
        }
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

    IEnumerator transicion(PianoRoll p1, PianoRoll p2)
    {
        int raiz = p2.ultimo.root;
        string tipo = p2.ultimo.type;
        int BPM1 = p1.songStructure.BPM;
        int BPM2 = p2.songStructure.BPM;

        List<List<int>> dosCincoUno = new List<List<int>>();

        if (tipo == "M")
        {
            List<int> sublist1 = new List<int> { 2, 5, 9, 12 };
            List<int> sublist2 = new List<int> { 7, 11, 14, 17 };

            dosCincoUno.Add(sublist1);
            dosCincoUno.Add(sublist2);
        } else {
            List<int> sublist1 = new List<int> { 2, 5, 8, 12 };
            List<int> sublist2 = new List<int> { 7, 11, 14, 17 };

            dosCincoUno.Add(sublist1);
            dosCincoUno.Add(sublist2);
        }
        for (int i = 0; i < 2; i++){
            float BPM_actual = BPM1 + (i+1)*(BPM2 - BPM1) / 3;
            for (int j = 0; j < 4; j++){
                GameObject sineObj = Instantiate(sinePrefab);
                activeSineObjects.Add(sineObj);
                Sine seno2 = sineObj.GetComponent<Sine>();
                seno2.PlayNote(2, BPM_actual, raiz + dosCincoUno[i][j], true);
                StartCoroutine(DestroyAfterTime(sineObj, seno2.duration));
            }
            yield return new WaitForSeconds(15f / 30);
        }
            StartCoroutine(GenerateMusic());
    }

    IEnumerator laQueViene(float f)
    {
        yield return new WaitForSeconds(f);
        StartCoroutine(GenerateMusic());
        enCamino = false;
    }

    string CheckEmocion(Vector3 posicion)
    {
        if (posicion.x < 0 && posicion.y > 0) { return "aleg"; }
        if (posicion.x >= 0 && posicion.y > 0) { return "calm"; }
        if (posicion.x < 0 && posicion.y <= 0) { return "tnso"; }
        if (posicion.x >= 0 && posicion.y <= 0) { return "trst"; }
        return "indef";
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
        activeSineObjects.Remove(obj);
    }

    void DestroyAllActiveSineObjects()
    {
        foreach (var obj in activeSineObjects)
        {
            Destroy(obj);
        }
        activeSineObjects.Clear();
    }
}
