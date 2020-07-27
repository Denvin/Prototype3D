using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    [SerializeField] Material materialMain;
    [SerializeField] Material materialMouseEnter;

    [Header("Effects")]
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject buildFX;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip fireSound;

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
            //GameManager.Instance.BuildCastle(transform.position);
            //GameManager.Instance.NextPhase();
            //TODO корутина с эффектом строительства
            StartCoroutine(BuildCoroutine());
        }
        else if (GameManager.Instance.CheckBulletPlayers())
        {
            if (explosionFX != null)
            {
                StartCoroutine(ExplosionCoroutine());
            }
        }
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        rendererCell.material = materialMouseEnter;
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        rendererCell.material = materialMain;
    }
    private void OnEnable()
    {
        rendererCell.material = materialMain;
    }

    IEnumerator ExplosionCoroutine()
    {
        GameManager.Instance.DamagePosition(transform.position);
        GameObject newObject = Instantiate(explosionFX, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(fireSound);

        yield return new WaitForSeconds(explosionTime);

        GameManager.Instance.NextPhase();
        Destroy(newObject);
    }
    IEnumerator BuildCoroutine()
    {
        GameObject newObject = Instantiate(buildFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(explosionTime);

        GameManager.Instance.BuildCastle(transform.position);
        yield return new WaitForSeconds(explosionTime);

        GameManager.Instance.NextPhase();
        Destroy(newObject);
    }
}
