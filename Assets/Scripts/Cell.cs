using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    [SerializeField] Material materialMain;
    [SerializeField] Material materialMouseEnter;
    [SerializeField] Material materialPlayerOne;
    [SerializeField] Material materialPlayerTwo;

    [Header("Effects")]
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject buildFX;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip buildSound;

    [Header("Cursor")]
    [SerializeField] Texture2D cursoreBuild;
    [SerializeField] Texture2D cursoreScouting;
    [SerializeField] Texture2D cursoreFire;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] Vector2 hotSpot = Vector2.zero;

    MeshRenderer rendererCell;
    Collider colliderCell;



    private void Awake()
    {
        rendererCell = GetComponent<MeshRenderer>();
        colliderCell = GetComponent<Collider>();
    }

    public void InactiveCollider()
    {
        colliderCell.enabled = false;
    }
    public void ActiveCollider()
    {
        colliderCell.enabled = true;
    }




    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (GameManager.Instance.CheckBuildPhase())
        {
            StartCoroutine(BuildCoroutine());
        }
        else if (GameManager.Instance.CheckBulletPlayers())
        {
            if (explosionFX != null)
            {
                StartCoroutine(ExplosionCoroutine());
            }
        }
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (GameManager.Instance.CheckBuildPhase())
        {
            Cursor.SetCursor(cursoreBuild, hotSpot, cursorMode);

            if (GameManager.Instance.PlayerOneActive)
            {
                rendererCell.material = materialPlayerOne;
            }
            else if (GameManager.Instance.PlayerTwoActive)
            {
                rendererCell.material = materialPlayerTwo;
            }
        }
        else if (GameManager.Instance.CheckFirePhase())
        {
            Cursor.SetCursor(cursoreFire, hotSpot, cursorMode);
            rendererCell.material = materialMouseEnter;
        }
        
        //rendererCell.material = materialMouseEnter;
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Cursor.SetCursor(null, hotSpot, cursorMode);
        rendererCell.material = materialMain;
    }
    private void OnEnable()
    {
        rendererCell.material = materialMain;
    }

    IEnumerator ExplosionCoroutine()
    {
        AudioManager.Instance.PlaySound(fireSound);
        GameManager.Instance.DamagePosition(transform.position);
        GameObject newObject = Instantiate(explosionFX, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(explosionTime);

        GameManager.Instance.NextPhase();
        Destroy(newObject);
    }
    IEnumerator BuildCoroutine()
    {
        
        AudioManager.Instance.PlaySound(buildSound);
        GameManager.Instance.BuildCastle(transform.position);

        GameObject newObject = Instantiate(buildFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(explosionTime*2);
        
        GameManager.Instance.NextPhase();
        Destroy(newObject);
    }
}
