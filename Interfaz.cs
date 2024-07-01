using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular : MonoBehaviour
{
    public LineRenderer renderizador;
    public float grosorLinea = 0.1f; // Ajusta el grosor de la línea

    void Start()
    {
        Dibujar(10, 0.05f, -2.5f, 2.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta un clic izquierdo del ratón
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Ajusta la distancia desde la cámara

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Borra el círculo anterior
            renderizador.positionCount = 0;

            // Dibuja un nuevo círculo en la posición del clic
            Dibujar(10, 0.05f, worldPosition.x, worldPosition.y);
        }
    }

    void Dibujar(int steps, float radio, float cenX, float cenY)
    {
        renderizador.startWidth = grosorLinea; // Establece el grosor de la línea
        renderizador.endWidth = grosorLinea;   // Establece el grosor de la línea
        renderizador.positionCount = steps + 1;

        for (int i = 0; i < steps + 1; i++)
        {
            float x = Mathf.Cos(2 * Mathf.PI * i / steps) * radio;
            float y = Mathf.Sin(2 * Mathf.PI * i / steps) * radio;

            Vector3 posicion = new Vector3(x + cenX, y + cenY, 0);
            renderizador.SetPosition(i, posicion);
        }
    }


}
