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

    [Header("Cursor")]

    [SerializeField] Texture2D cursoreScouting;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] Vector2 hotSpot = Vector2.zero;

    MeshRenderer rendererVeil;
    Collider colliderVeil;


    private void Awake()
    {
        colliderVeil = GetComponent<Collider>();
        rendererVeil = GetComponent<MeshRenderer>();
        
    }
    private void Start()
    {
    }

    public void InactiveCollider()
    {
        colliderVeil.enabled = false;
    }
    public void ActiveCollider()
    {
        colliderVeil.enabled = true;
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
        if (GameManager.Instance.CheckScoutingPhase())
        {
            Cursor.SetCursor(cursoreScouting, hotSpot, cursorMode);
        }
        rendererVeil.material = materialMouseEnter;
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Cursor.SetCursor(null, hotSpot, cursorMode);
        rendererVeil.material = materialMain;
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
