using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] GameObject[] mapCells;


    private int randomCell;

    
    Collider colliderMap;


    private void Start()
    {
        colliderMap = GetComponent<Collider>();
    }

    public void InactiveCollider()
    {
        colliderMap.enabled = false;
    }
    public void ActiveCollider()
    {
        colliderMap.enabled = true;
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
