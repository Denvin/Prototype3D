using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingleTon

    public static GameManager Instance { get; private set; }



    private void Awake()

    {

        if (Instance != null)

        {

            Destroy(gameObject);

        }

        else

        {

            Instance = this;

        }

    }

    #endregion
    [Header("Player1 and Player2")]
    [SerializeField] GameObject playerOnePrefab;
    [SerializeField] int healthPlayerOne = 100;
    [SerializeField] GameObject playerTwoPrefab;
    [SerializeField] int healthPlayerTwo = 100;
    [SerializeField] float playerPosition = 0.1f;
    [SerializeField] float timeToMove = 5f;
    [SerializeField] int damage = 20; //урон наносится одинаковый. В будущем, возможно, сделаю что и наносимый урон можно прокачивать

    [Header("Active player")]
    [SerializeField] bool playerOneActive;
    [SerializeField] bool playerTwoActive;

    [Header("Map of Player 1")]
    [SerializeField] GameObject[] cellsPlayerOne;
    [SerializeField] GameObject mapPlayerOne;

    [Header("Map of player 2")]
    [SerializeField] GameObject[] cellsPlayerTwo;
    [SerializeField] GameObject maoPlayerTwo;

    [Header("General map")]
    [SerializeField] GameObject[] generaMapCells;



    private int randomCellPlayerOne;
    private int randomCellPlayerTwo;


    private Vector3 positionPlayerOne;
    private Vector3 newPositionOne;

    private Vector3 positionPlayerTwo;
    private Vector3 newPositionTwo;


    private GameObject playerOne;
    private GameObject playerTwo;

    private Cell[] cells;
    private Veil[] veil;



    // Start is called before the first frame update
    void Start()
    {
        cells = FindObjectsOfType<Cell>();
        veil = FindObjectsOfType<Veil>();
        GeneratePosition();
        InactiveGeneralMap();

        StartCoroutine(GamePlay());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePosition(GameObject cellObject)
    {
        if(playerOneActive)
        {
            if (cellObject.transform.position == positionPlayerTwo)
            {
                Debug.Log("Попадание в игрока 2");
                DoDamagePlayerTwo();
            }
        }
        else if (playerTwoActive)
        {
            if (cellObject.transform.position == positionPlayerOne)
            {
                Debug.Log("Попадание в игрока 1");
                DoDamagePlayerOne();
            }
        }
    }
    public bool ActivePlayer()
    {
        if (playerOneActive)
        {
            return playerOneActive;
        }
        else
        {
            return playerTwoActive;
        }
    }

    public void DoDamagePlayerOne()
    {
        healthPlayerOne -= damage;

        if (healthPlayerOne <= 0)
        {
            Debug.Log("Player 1 destroyed");
        }
    }
    public void DoDamagePlayerTwo()
    {
        healthPlayerTwo -= damage;
        if (healthPlayerTwo <= 0)
        {
            Debug.Log("Player 2 destroyed");
        }
    }

    private void GeneratePosition()
    {  
        if (playerOnePrefab != null && playerTwoPrefab != null)
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
                //positionPlayerOne = cellsPlayerOne[randomCellPlayerOne].transform.position - new Vector3(0, playerPosition, 0);
                //positionPlayerTwo = cellsPlayerTwo[randomCellPlayerTwo].transform.position - new Vector3(0, playerPosition, 0);
                positionPlayerOne = generaMapCells[randomCellPlayerOne].transform.position;
                positionPlayerTwo = generaMapCells[randomCellPlayerTwo].transform.position;

                newPositionOne = positionPlayerOne - new Vector3(0, playerPosition, 0);
                newPositionTwo = positionPlayerTwo - new Vector3(0, playerPosition, 0);

                playerOne = Instantiate(playerOnePrefab, newPositionOne, Quaternion.identity);
                playerTwo = Instantiate(playerTwoPrefab, newPositionTwo, Quaternion.identity);
                //playerOne = Instantiate(playerOnePrefab, positionPlayerOne, Quaternion.identity);
                //playerTwo = Instantiate(playerTwoPrefab, positionPlayerTwo, Quaternion.identity);

                //playerOne.SetActive(false);
                //playerTwo.SetActive(false);


                //cellsPlayerOne[randomCellPlayerOne].SetActive(false);//TODO delete
                //cellsPlayerTwo[randomCellPlayerTwo].SetActive(false);//TODO delete
            }
        }       
    }

    private void InactiveGeneralMap()
    {
        for (int i = 0; i < generaMapCells.Length; i++)
        {
            if (generaMapCells[i] != null)
            {
                generaMapCells[i].SetActive(false);
            }
        }
    }
    private void ActiveGeneralMap()
    {
        for (int i = 0; i < generaMapCells.Length; i++)
        {
            if (generaMapCells[i] != null)
            {
                generaMapCells[i].SetActive(true);
            }
        }
    }

    private void ActivePlayerOne()
    {
        playerOneActive = true;
        playerTwoActive = false;

        playerOne.SetActive(false);
        playerTwo.SetActive(true);
        
        
        mapPlayerOne.SetActive(false);
        maoPlayerTwo.SetActive(true);
        
        /*for (int i = 0; i < cellsPlayerOne.Length; i++)
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
            
        }*/
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

        mapPlayerOne.SetActive(true);
        maoPlayerTwo.SetActive(false);

        /*for (int i = 0; i < cellsPlayerOne.Length; i++)
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

        }*/
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

    /*private void ShellingPhase()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].InactiveCollider();
            }
        }
    }*/
    private void ShellingPhase()
    {
        //Отключение колайдера пелены
        for (int i = 0; i < veil.Length; i++)
        {
            if (veil[i] != null)
            {
                veil[i].InactiveCollider();
            }
        }
        ActiveGeneralMap();
    }


    IEnumerator GamePlay()
    {
        while (true)
        {
            ActivePlayerOne();
            yield return new WaitForSeconds(timeToMove);

            ActivePlayerTwo();
            yield return new WaitForSeconds(timeToMove);

            ShellingPhase();
        }
    }
}
