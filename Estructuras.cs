using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewSongStructure", menuName = "Music/SongStructure")]
public class SongStructure : ScriptableObject
{
    public int BPM = 360;
    public int[] structure;
    public List<ChordProgression> chordProgressions;

    public void Initialize()
    {
        GenerateRandomStructure();
        chordProgressions = new List<ChordProgression>();

        // Crear un diccionario para mapear partes a índices de progresión
        Dictionary<int, int> partToProgressionMap = new Dictionary<int, int>();

        // Crear una lista para verificar qué progresiones ya se han usado
        HashSet<int> usedProgressions = new HashSet<int>();

        foreach (int part in structure)
        {
            if (!partToProgressionMap.ContainsKey(part))
            {
                int progressionIndex = GetUniqueRandomIndex(usedProgressions, 18);
                partToProgressionMap[part] = progressionIndex;
            }

            int index = partToProgressionMap[part];

            ChordProgression progression = new ChordProgression();

            foreach (var acorde in progression.posibleProgresion[index])
            {
                progression.chords.Add(new Chord(acorde.root, acorde.type));
            }

            // Duplicar acordes si la progresión es corta
            if (progression.posibleProgresion[index].Length < 6)
            {
                foreach (var acorde in progression.posibleProgresion[index])
                {
                    progression.chords.Add(new Chord(acorde.root, acorde.type));
                }
            }

            chordProgressions.Add(progression);
        }
    }

    private void GenerateRandomStructure()
    {
        // Definimos algunos patrones posibles
        List<int[]> possibleStructures = new List<int[]>
        {
            new int[] { 1, 1, 2, 1 },
            new int[] { 1, 2, 1, 2 },
            new int[] { 1, 2, 3, 1, 2, 3 },
            new int[] { 1, 2, 1, 3 },
            new int[] { 1, 2, 3, 2, 1 },
            new int[] { 1, 1, 1, 2, 3 },
            new int[] { 1, 2, 2, 1 },
            new int[] { 1, 2, 3, 3, 1 },
            new int[] { 1, 1, 3, 2, 2 },
            new int[] { 1, 3, 1, 2 },
            new int[] { 1, 1, 2, 2, 3, 3 },
            new int[] { 1, 2, 3, 4, 1, 2, 3, 4 },
            new int[] { 1, 2, 1, 2, 3, 4 },
            new int[] { 1, 2, 3, 1, 3, 2 },
            new int[] { 1, 2, 3, 4, 5, 6 },
            new int[] { 1, 1, 2, 3, 1, 3 },
            new int[] { 1, 2, 3, 4, 2, 1 },
            new int[] { 1, 2, 3, 3, 4, 5 },
            new int[] { 1, 1, 2, 3, 4, 4 },
            new int[] { 1, 2, 1, 3, 2, 4 }
        };

        // Seleccionamos un patrón aleatorio
        int randomIndex = Random.Range(0, possibleStructures.Count);
        structure = possibleStructures[randomIndex];
    }

    private int GetUniqueRandomIndex(HashSet<int> usedIndices, int max)
    {
        int index;
        do
        {
            index = Random.Range(0, max);
        } while (usedIndices.Contains(index));

        usedIndices.Add(index);
        return index;
    }
}

public class ChordProgression
{
    public string part;
    public List<Chord> chords = new List<Chord>();
    public (int root, string type)[][] posibleProgresion = new (int, string)[][]
    {
        new (int, string)[] {(0, "m"), (3, "M"), (10, "M"), (5, "M")},
        new (int, string)[] {(0, "m"), (10, "M"), (7, "m"), (8, "M")},
        new (int, string)[] {(0, "M"), (7, "M"), (2, "m"), (5, "M")},
        new (int, string)[] {(5, "M"), (0, "M"), (7, "M"), (9, "m")},
        new (int, string)[] {(0, "M"), (5, "M"), (9, "m"), (7, "M")},
        new (int, string)[] {(0, "M"), (0, "M"), (0, "M"), (0, "M"), (5, "M"), (5, "M"), (0, "M"), (0, "M"), (7, "M"), (5, "M"), (0, "M"), (0, "M")},
        new (int, string)[] {(0, "M"), (2, "M"), (5, "M"), (0, "M")},
        new (int, string)[] {(0, "M"), (7, "M"), (10, "M"), (5, "M")},
        new (int, string)[] {(0, "M"), (2, "m"), (5, "M"), (7, "M")},
        new (int, string)[] {(0, "M"), (5, "M"), (0, "M"), (7, "M")},
        new (int, string)[] {(0, "m"), (1, "M")},
        new (int, string)[] {(5, "M"), (0, "M"), (7, "M"), (7, "M")},
        new (int, string)[] {(0, "m"), (0, "m"), (8, "M"), (7, "M")},
        new (int, string)[] {(5, "M"), (7, "M"), (0, "M"), (9, "m")},
        new (int, string)[] {(0, "M"), (5, "M"), (7, "M"), (5, "M")},
        new (int, string)[] {(0, "m"), (5, "M")},
        new (int, string)[] {(0, "m"), (6, "M"), (0, "M"), (0, "M")},
        new (int, string)[] {(5, "M"), (10, "m7"), (3, "m6"), (2, "dim7"), (11, "dim7"), (0, "M")},
    };
}

public class Chord
{
    public int root;
    public string type;
    public List<int> notes;

    public Chord(int root, string type)
    {
        this.root = root;
        this.type = type;
        notes = GenerateNotes(root, type);
    }

    private List<int> GenerateNotes(int root, string type)
    {
        List<int> chordNotes = new List<int>();

        chordNotes.Add(root);

        if (type == "M")
        {
            chordNotes.Add(root + 4);
            chordNotes.Add(root + 7);
        }
        else if (type == "m")
        {
            chordNotes.Add(root + 3);
            chordNotes.Add(root + 7);
        }
        else if (type == "M7")
        {
            chordNotes.Add(root + 4);
            chordNotes.Add(root + 7);
            chordNotes.Add(root + 10);
        }
        else if (type == "m7")
        {
            chordNotes.Add(root + 3);
            chordNotes.Add(root + 7);
            chordNotes.Add(root + 10);
        }
        else if (type == "M6")
        {
            chordNotes.Add(root + 4);
            chordNotes.Add(root + 7);
            chordNotes.Add(root + 9);
        }
        else if (type == "m6")
        {
            chordNotes.Add(root + 3);
            chordNotes.Add(root + 7);
            chordNotes.Add(root + 9);
        }
        else if (type == "dim7")
        {
            chordNotes.Add(root + 3);
            chordNotes.Add(root + 6);
            chordNotes.Add(root + 10);
        }

        return chordNotes;
    }
}
