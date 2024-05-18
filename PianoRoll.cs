using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PianoRoll : MonoBehaviour
{
    private (int p, int figura)[][] seccion;

    public void Start()
    {
        Inicializar(ref seccion);
    }

    void Inicializar(ref (int p, int figura)[][] seccion)
    {
        seccion = new (int p, int figura)[8][];
        for (int i = 0; i < 8; i++)
        {
            seccion[i] = new (int p, int figura)[8];
            for (int j = 0; j < 8; j++)
            {
                seccion[i][j] = (0, 0); // Inicializar cada tupla con (0, 0)
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
        return (0, 0); // Nota por defecto si está fuera de los límites
    }

    public void Actualizar(int iteraciones)
    {
        Inicializar(ref seccion);
        if (iteraciones == 0) {
            Llenar((12, 4),0,0);
            Llenar((0, 2),2,0);
            Llenar((4, 2),3,0);
            Llenar((7, 2),4,0);
            Llenar((16, 4),0,2);
            Llenar((19, 4),0,4);
            Llenar((12, 4),0,4);
            Llenar((17, 4),0,6);
            Llenar((18, 4),0,7);
        } else if (iteraciones == 1){
            Llenar((2, 2),2,0);
            Llenar((5, 2),3,0);
            Llenar((7, 2),4,0);
            Llenar((11, 2),5,0);
        } else if (iteraciones == 2){
            Llenar((0, 2),2,0);
            Llenar((4, 2),3,0);
            Llenar((9, 2),4,0);
        } else if (iteraciones == 3){
            Llenar((5, 2),2,0);
            Llenar((9, 2),3,0);
            Llenar((0, 2),4,0);
        }
        
    }

    private (int p, int figura)[][] CopiarMatriz((int p, int figura)[][] matrizOriginal)
    {
        (int p, int figura)[][] nuevaMatriz = new (int p, int figura)[matrizOriginal.Length][];
        for (int i = 0; i < matrizOriginal.Length; i++)
        {
            nuevaMatriz[i] = new (int p, int figura)[matrizOriginal[i].Length];
        }

        for (int i = 0; i < matrizOriginal.Length; i++)
        {
            for (int j = 0; j < matrizOriginal[i].Length; j++)
            {
                nuevaMatriz[i][j] = matrizOriginal[i][j];
            }
        }

        return nuevaMatriz;
    }
}
