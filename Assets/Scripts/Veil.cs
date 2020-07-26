using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Veil : MonoBehaviour
{
    [SerializeField] Material materialMain;
    [SerializeField] Material materialMouseEnter;
    [SerializeField] GameObject smokeFX;
    [SerializeField] float smokeTime;
    [SerializeField] AudioClip openSound;

    MeshRenderer renderer;
    Collider collider;


    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        GameManager.Instance.CheckScoutingPoint();
        StartCoroutine(ScoutingCoroutine());
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        renderer.material = materialMouseEnter;
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        renderer.material = materialMain;
    }

    IEnumerator ScoutingCoroutine()
    {
        GameObject newObject = Instantiate(smokeFX, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(openSound);

        Destroy(gameObject);

        yield return new WaitForSeconds(smokeTime);
        Destroy(newObject);

    }
}
