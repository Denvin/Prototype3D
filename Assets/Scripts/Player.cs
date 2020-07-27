using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public Action onScoutingPoint = delegate { };
    public Action onBulletChanged = delegate { };
    public Action onCastleChanged = delegate { };
    public Action onBuildingPoint = delegate { };
    public Action onLose = delegate { };

    [SerializeField] int castles;
    [SerializeField] int maxCastles;
    [SerializeField] int damage = 10;
    [SerializeField] int bullets = 1;
    [SerializeField] int buildingPoints = 0;
    [SerializeField] int resources;
    [SerializeField] int maxScoutingPoints = 0;


    private int skillPoints = 0;
    private int scoutingPoints = 0;
    private bool detection = false;


    public int Castles
    {
        get
        {
            return castles;
        }
    }
    public int MaxCastles
    {
        get
        {
            return maxCastles;
        }
    }
    public int BuildingPoints
    {
        get
        {
            return buildingPoints;
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

    public void AddBuildingPoints()
    {
        buildingPoints++;
        onBuildingPoint();
    }

    public void ZeroBuildingPoints()
    {
        buildingPoints = 0;
        onBuildingPoint();
    }
    public void AddCastle()
    {
        if (castles == maxCastles)
        {
            maxCastles++;
            castles = maxCastles;
        }
        else if (castles < maxCastles)
        {
            castles++;
        }
        onCastleChanged();
        
        //health += GameManager.Instance.BonusHealth;
    }
    public void DoDamage()
    {
        castles --;
        if (castles <= 0)
        {
            Debug.Log("Castle destroyed");
            Death();
            //Destroy(gameObject, 1f);
        }
        onCastleChanged();
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
