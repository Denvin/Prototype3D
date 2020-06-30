using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
    private void OnMouseEnter()
    {
        Debug.Log("It is a cell!");
    }
}
