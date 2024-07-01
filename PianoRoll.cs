using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PianoRoll : ScriptableObject
{
    public (int p, int figura)[][] seccion;
    public int largo;
    public SongStructure songStructure;
    private int key;
    public string emocionn;
    public Chord ultimo;

    public void Empezar(int k, string emocion)
    {
        key = k;
        emocionn = emocion;
        songStructure = ScriptableObject.CreateInstance<SongStructure>();
        songStructure.Initialize(emocion);
        largo = 0;
        foreach (var progression in songStructure.chordProgressions)
        {
            foreach (var chord in progression.chords)
            {
                largo += 8;
            }
        }
        largo++;
        ultimo = songStructure.chordProgressions[0].chords[0];
        Inicializar(ref seccion);
        Actualizar();
    }

    void Inicializar(ref (int p, int figura)[][] seccion)
    {
        seccion = new (int p, int figura)[8][];
        for (int i = 0; i < 8; i++)
        {
            seccion[i] = new (int p, int figura)[largo];
            for (int j = 0; j < largo; j++)
            {
                seccion[i][j] = (0, 0);
            }
        }
    }

    private void Llenar((int p, int figura) nota, int x, int y)
    {
        seccion[y][x] = nota;
    }

    public (int p, int figura) GetNota(int x, int y)
    {
        if (seccion != null && y < seccion.Length && x < seccion[y].Length)
        {
            return seccion[y][x];
        }
        return (0, 0);
    }

    public void Actualizar()
    {
        Inicializar(ref seccion);

        int estamos = 0;
        foreach (var progression in songStructure.chordProgressions)
        {
            key += 0;
            foreach (var chord in progression.chords)
            {
                List<int> intervalos = chord.notes;
                intervalos.Add(intervalos[0] + 12);

                for (int i = 0; i < chord.notes.Count; i++)
                {
                    Llenar(((chord.notes[i] + key) % 12, 1), estamos * 8, 2 + i);
                }
                for(int i = 0; i < 8; i++)
                {
                    if (Random.Range(0,2) == 0 || i == 0)
                    {
                        Llenar(((intervalos[Random.Range(0, intervalos.Count)] + key) % 12, 4), estamos * 8 + i, 0);
                    }
                }
                estamos++;
            }
        }
    }
}

