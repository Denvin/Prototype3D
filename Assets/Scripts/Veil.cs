using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veil : MonoBehaviour
{
    Collider collider;


    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void InactiveCollider()
    {
        collider.enabled = false;
    }
    public void ActiveCollider()
    {
        collider.enabled = true;
    }
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
    private void OnMouseEnter()
    {
        Debug.Log("It is a cell!");
    }
    private void OnMouseExit()
    {

    }
}
