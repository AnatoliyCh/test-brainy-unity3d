using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{
    public bool emptyPosition = true; //проверка на задевание объектов (генерация препятствий, сетки для поиска пути)
    private void OnTriggerEnter2D(Collider2D collision) => emptyPosition = false;
    private void OnTriggerExit2D(Collider2D collision) => emptyPosition = true;
}
