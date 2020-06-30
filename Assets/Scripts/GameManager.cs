﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player1 and Player2")]
    [SerializeField] GameObject playerOne;
    [SerializeField] GameObject playerTwo;
    [SerializeField] float playerPosition = 0.1f;
    [Header("Active player")]
    [SerializeField] bool playerOneActive;
    [SerializeField] bool playerTwoActive;

    [Header("Map of Player 1")]
    [SerializeField] GameObject[] cellsPlayerOne;

    [Header("Map of player 2")]
    [SerializeField] GameObject[] cellsPlayerTwo;



    private int randomCellPlayerOne;
    private int randomCellPlayerTwo;
    private Vector3 positionPlayerOne;
    private Vector3 positionPlayerTwo;

    private void Awake()
    {
        //GeneratePosition();
    }
    // Start is called before the first frame update
    void Start()
    {
        GeneratePosition();

        StartCoroutine(GamePlay());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }


    private void GeneratePosition()
    {
        
        
        if (playerOne != null && playerTwo != null)
        {
            randomCellPlayerOne = Random.Range(0, cellsPlayerOne.Length);
            randomCellPlayerTwo = Random.Range(0, cellsPlayerTwo.Length);
            if (randomCellPlayerOne == randomCellPlayerTwo)
            {
                while (randomCellPlayerTwo != randomCellPlayerOne)
                {
                    randomCellPlayerTwo = Random.Range(0, cellsPlayerTwo.Length);
                }
            }
            else
            {
                positionPlayerOne = cellsPlayerOne[randomCellPlayerOne].transform.position - new Vector3(0, playerPosition, 0);
                positionPlayerTwo = cellsPlayerTwo[randomCellPlayerTwo].transform.position - new Vector3(0, playerPosition, 0);

                Instantiate(playerOne, positionPlayerOne, Quaternion.identity);
                Instantiate(playerTwo, positionPlayerTwo, Quaternion.identity);


                //cellsPlayerOne[randomCellPlayerOne].SetActive(false);//TODO delete
                //cellsPlayerTwo[randomCellPlayerTwo].SetActive(false);//TODO delete
            }
        }       
    }

    private void ActivePlayerOne()
    {
        playerOneActive = true;
        playerTwoActive = false;

        playerOne.SetActive(false);
        playerTwo.SetActive(true);
        
        
        
        for (int i = 0; i < cellsPlayerOne.Length; i++)
        {
            if (cellsPlayerOne[i] != null)
            {
                cellsPlayerOne[i].SetActive(false);
            }
            
        }
        for (int i = 0; i < cellsPlayerTwo.Length; i++)
        {
            if (cellsPlayerTwo[i] != null)
            {
                cellsPlayerTwo[i].SetActive(true);
            }
            
        }
        /*foreach(var cell in cellsPlayerOne)
        {
            if (cell != null)
            {
                cell.SetActive(false);
            }
            
        }
        foreach(var cell in cellsPlayerTwo)
        {
            if (cell != null)
            {
                cell.SetActive(true);
            }
            
        }*/
    }
    private void ActivePlayerTwo()
    {
        playerOneActive = false;
        playerTwoActive = true;

        playerOne.SetActive(true);
        playerTwo.SetActive(false);


        for (int i = 0; i < cellsPlayerOne.Length; i++)
        {
            if (cellsPlayerOne[i] != null)
            {
                cellsPlayerOne[i].SetActive(true);
            }
        }
        for (int i = 0; i < cellsPlayerTwo.Length; i++)
        {
            if (cellsPlayerTwo[i] != null)
            {
                cellsPlayerTwo[i].SetActive(false);
            }

        }
        /*foreach (var cell in cellsPlayerTwo)
        {
            if (cell != null)
            {
                cell.SetActive(false);
            }
            
        }
        foreach (var cell in cellsPlayerTwo)
        {
            if (cell != null)
            {
                cell.SetActive(true);
            }
            
        }*/
    }

    IEnumerator GamePlay()
    {
        while (true)
        {
            ActivePlayerOne();
            yield return new WaitForSeconds(5f);

            ActivePlayerTwo();
            yield return new WaitForSeconds(5f);
        }
    }
}