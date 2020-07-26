using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public Action onScoutingPoint = delegate { };
    public Action onBulletChanged = delegate { };
    public Action onHealthChanged = delegate { };
    public Action onLose = delegate { };

    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] int damage = 10;
    [SerializeField] int bullets = 5;
    [SerializeField] int resources;
    [SerializeField] int maxScoutingPoints = 0;


    private int skillPoints = 0;
    private int scoutingPoints = 0;
    private bool detection = false;


    public int Health
    {
        get
        {
            return health;
        }
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    public int Bullets
    {
        get
        {
            return bullets;
        }
    }
    public int ScoutingPoints
    {
        get
        {
            return scoutingPoints;
        }
    }
    
    public void Detection()
    {
        detection = true;
    }
    public void ScoutinPointCount()
    {
        scoutingPoints--;
        onScoutingPoint();
    }
    public void AddScoutingPoint()
    {
        maxScoutingPoints++;
    }
    public void ResetScoutingPoints()
    {
        scoutingPoints = maxScoutingPoints;
        onScoutingPoint();
    }

    public void Shot()
    {
        bullets--;
        onBulletChanged();
    }

    public void AddBullets()
    {
        bullets++;
        onBulletChanged();
    }

    public void AddHealth()
    {
        if (!detection)
        {
            if (health == maxHealth)
            {
                maxHealth += GameManager.Instance.BonusHealth;
                health = maxHealth;
            }
            else if (health < maxHealth)
            {
                health += GameManager.Instance.BonusHealth;
            }
        }
        onHealthChanged();
        
        //health += GameManager.Instance.BonusHealth;
    }
    public void DoDamage()
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Player 1 destroyed");
            Death();
            Destroy(gameObject, 1f);
        }
        onHealthChanged();
    }
    public void Death()
    {
        //TODO anim
        onLose();
    }
    public void Position(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
