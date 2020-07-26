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
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip fireSound;

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
        if (GameManager.Instance.CheckBulletPlayers())
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
    //private void OnEnable()
    //{
    //    renderer.material = materialMain;
    //}

    IEnumerator ExplosionCoroutine()
    {
        GameManager.Instance.DamagePosition(transform.position);
        GameObject newObject = Instantiate(explosionFX, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(fireSound);

        yield return new WaitForSeconds(explosionTime);

        GameManager.Instance.NextPhase();
        Destroy(newObject);
    }
}
