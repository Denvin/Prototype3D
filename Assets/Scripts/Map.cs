using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] GameObject[] mapCells;


    private int randomCell;

    
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
        GameManager.Instance.NextPhase();

        randomCell = Random.Range(0, mapCells.Length);
        Instantiate(mapCells[randomCell], transform.position, Quaternion.identity);
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
