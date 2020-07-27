using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

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
    
    [Header("Player 1")]
    [SerializeField] Player playerOne;
    [SerializeField] GameObject castlePlayerOne;

    [Header("Player 2")]
    [SerializeField] Player playerTwo;
    [SerializeField] GameObject castlePlayerTwo;

    [SerializeField] float playerPosition = 0.1f;
    [SerializeField] float timeToMove = 5f;
    [SerializeField] GameObject buildFX;
    [SerializeField] int pointsBuilding = 3;


    [Header("Map of Player 1")]
    [SerializeField] GameObject[] cellsPlayerOne;
    [SerializeField] GameObject mapPlayerOne;

    [Header("Map of player 2")]
    [SerializeField] GameObject[] cellsPlayerTwo;
    [SerializeField] GameObject mapPlayerTwo;

    [Header("General map")]
    [SerializeField] GameObject[] generaMapCells;
    [SerializeField] GameObject positionPlayers;

    [Header("Active player info")]
    [SerializeField] Text activePlayerText;
    [SerializeField] Text activePhaseText;
    [SerializeField] Button buttonPhase;
    [SerializeField] Button buttonNextPlayer;
    [SerializeField] GameObject changePlayerPanel;

    [Header("Winner")]
    [SerializeField] GameObject winnerPanel;
    [SerializeField] Text winnerText;




    private List<GameObject> castlesPlayerOne = new List<GameObject>();
    private List<GameObject> castlesPlayerTwo = new List<GameObject>();

    private Map[] mapCells;
    private Cell[] cells;
    private Veil[] veil;


    private bool playerOneActive;
    private bool playerTwoActive;
    private bool isClickNextPhase = false;
    private bool isClickNextPlayer = false;
    private bool isClickSkill = false;
    private bool winner = false;

    private int randomCellPlayerOne;
    private int randomCellPlayerTwo;

    private Vector3 positionPlayerOne;
    private Vector3 newPositionOne;

    private Vector3 positionPlayerTwo;
    private Vector3 newPositionTwo;
    private Vector3 invisiblePosition;

    private PlayerPhases activePhase;

    private enum PlayerPhases
    {
        BUILD,
        SCOUTING,
        FIRE,
        SKILL
    }
    

    // Start is called before the first frame update
    void Start()
    {
        cells = FindObjectsOfType<Cell>();
        veil = FindObjectsOfType<Veil>();
        //GeneratePosition();

        invisiblePosition = positionPlayers.transform.position;


        ActivePlayerOne();
        ChangePhase(PlayerPhases.BUILD);

        playerOne.onLose += WinnerPLayerTwo;
        playerTwo.onLose += WinnerPlayerOne;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePhase();
    }


    public void DamagePosition(Vector3 target)
    {
        if (playerOneActive)
        {
            for (int i = 0; i < castlesPlayerTwo.Count; i++)
            {
                if (target == (castlesPlayerTwo[i].transform.position + new Vector3(0, playerPosition,0)))
                {
                    playerTwo.DoDamage();
                    Destroy(castlesPlayerTwo[i]);
                    castlesPlayerTwo.RemoveAt(i);
                }
            }

        }
        else if (playerTwoActive)
        {
            for (int i = 0; i < castlesPlayerOne.Count; i++)
            {
                if (target == (castlesPlayerOne[i].transform.position + new Vector3(0, playerPosition, 0)))
                {
                    playerOne.DoDamage();
                    Destroy(castlesPlayerOne[i]);
                    castlesPlayerOne.RemoveAt(i);
                }
            }
        }

    }


    public void BuildCastle(Vector3 target)
    {
        if (activePhase == PlayerPhases.BUILD)
        {
            if (playerOneActive)
            {
                GameObject newObject = Instantiate(castlePlayerOne, target - new Vector3(0, playerPosition,0), Quaternion.identity);
                castlesPlayerOne.Add(newObject);
                playerOne.ZeroBuildingPoints();
                playerOne.AddCastle();
            }
            else if (playerTwoActive)
            {
                GameObject newObject = Instantiate(castlePlayerTwo, target - new Vector3(0, playerPosition, 0), Quaternion.identity);
                castlesPlayerTwo.Add(newObject);
                playerTwo.ZeroBuildingPoints();
                playerTwo.AddCastle();
            }
            
        }
    }
    public void CheckScoutingPoint()
    {
        if (playerOneActive)
        {
            if (playerOne.ScoutingPoints <= 1)
            {
                NextPhase();
                playerOne.ScoutinPointCount();
            }
            else
            {
                playerOne.ScoutinPointCount();
            }
        }
        else if (playerTwoActive)
        {
            if (playerTwo.ScoutingPoints <= 1)
            {
                NextPhase();
                playerTwo.ScoutinPointCount();
            }
            else
            {
                playerTwo.ScoutinPointCount();
            }
        }
    }
    public bool CheckBuildPhase()
    {
        if (activePhase == PlayerPhases.BUILD)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckBulletPlayers()
    {

        if (playerOneActive)
        {
            if (playerOne.Bullets > 0)
            {
                playerOne.Shot();
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (playerTwoActive)
        {
            if (playerTwo.Bullets > 0)
            {
                playerTwo.Shot();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
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

                positionPlayerOne = generaMapCells[randomCellPlayerOne].transform.position;
                positionPlayerTwo = generaMapCells[randomCellPlayerTwo].transform.position;

                newPositionOne = positionPlayerOne - new Vector3(0, playerPosition, 0);
                newPositionTwo = positionPlayerTwo - new Vector3(0, playerPosition, 0);

                //playerOne.transform.position = newPositionOne;
                //playerTwo.transform.position = newPositionTwo;    
            }
        }
    }


    //Активный игрок
    private void ActivePlayerOne()
    {
        playerOneActive = true;
        playerTwoActive = false;

        playerOne.ResetScoutingPoints();

        UIManager.Instance.ShowPanelPlayerOne();
    }
    private void ActivePlayerTwo()
    {
        playerOneActive = false;
        playerTwoActive = true;

        playerTwo.ResetScoutingPoints();


        UIManager.Instance.ShowPanelPlayerTwo();
    }

    
    private void InactiveGeneralMap() //Отключение коллайдера клеток по которым ведется обстрел
    {
        for (int i = 0; i < generaMapCells.Length; i++)
        {
            if (generaMapCells[i] != null)
            {
                generaMapCells[i].SetActive(false);
            }
        }
    }
    private void ActiveGeneralMap() //Включение коллайдера клеток по которым ведется обстрел
    {
        for (int i = 0; i < generaMapCells.Length; i++)
        {
            if (generaMapCells[i] != null)
            {
                generaMapCells[i].SetActive(true);
            }
        }
    }

    //ФАЗЫ:

    private void BuildingPhase()
    {
        mapPlayerOne.SetActive(false);
        mapPlayerTwo.SetActive(false);   

        if (playerOneActive)
        {
            foreach (var castle in castlesPlayerOne)
            {
                castle.SetActive(true);
            }

            foreach (var castle in castlesPlayerTwo)
            {
                castle.SetActive(false);
            }            
        }
        else if (playerTwoActive)
        {
            foreach (var castle in castlesPlayerOne)
            {
                castle.SetActive(false);
            }

            foreach (var castle in castlesPlayerTwo)
            {
                castle.SetActive(true);
            }
        }
    }
    private void ShellingPhase() //Отключение колайдера пелены во время фазы обстрела
    {
        for (int i = 0; i < veil.Length; i++)
        {
            if (veil[i] != null)
            {
                veil[i].InactiveCollider();
            }
        }
        ActiveGeneralMap();
    }

    private void ScoutingPhase() //Включение коллайдера пелены во время фазы разведки
    {
        if (playerOneActive)
        {
            mapPlayerOne.SetActive(false);
            mapPlayerTwo.SetActive(true);

            foreach (var castle in castlesPlayerOne)
            {
                castle.SetActive(false);
            }

            foreach (var castle in castlesPlayerTwo)
            {
                castle.SetActive(true);
            }
        }
        else if (playerTwoActive)
        {
            mapPlayerOne.SetActive(true);
            mapPlayerTwo.SetActive(false);

            foreach (var castle in castlesPlayerOne)
            {
                castle.SetActive(true);
            }

            foreach (var castle in castlesPlayerTwo)
            {
                castle.SetActive(false);
            }
        }
        for (int i = 0; i < veil.Length; i++)
        {
            if (veil[i] != null)
            {
                veil[i].ActiveCollider();
            }
        }
        InactiveGeneralMap();
    }


    private void SkillPhase()
    {
        if (playerOneActive)
        {
            UIManager.Instance.ShowSkillPanelOne();
        }
        else if (playerTwoActive)
        {
            UIManager.Instance.ShowSkillPanelTwo();
        }
    }


    private void ChangePhase(PlayerPhases newPhase)
    {
        activePhase = newPhase;
        switch (activePhase)
        {
            case PlayerPhases.BUILD:
                activePhaseText.text = $"Выберите место для строительства замка";
                BuildingPhase();
                break;
            case PlayerPhases.SCOUTING:
                activePhaseText.text = $"Выберите область для разведки!";
                ScoutingPhase();
                break;
            case PlayerPhases.FIRE:
                activePhaseText.text = $"Выберите область для обстрела!";
                ShellingPhase();
                break;
            case PlayerPhases.SKILL:
                activePhaseText.text = $"Что вы хотите улучшить?";
                SkillPhase();
                buttonPhase.gameObject.SetActive(false);
                break;
        }
    }

    private void UpdatePhase()
    {
        switch (activePhase)
        {
            case PlayerPhases.BUILD:
                if (isClickNextPhase)
                {
                    ChangePhase(PlayerPhases.SCOUTING);
                    isClickNextPhase = false;
                }
                break;
            case PlayerPhases.SCOUTING:
                if (isClickNextPhase)
                {
                    AfterScoutingPhase();
                    //ChangePhase(PlayerPhases.FIRE);
                    isClickNextPhase = false;
                }
                break;
            case PlayerPhases.FIRE:
                if (isClickNextPhase && !winner)
                {
                    ChangePhase(PlayerPhases.SKILL);
                    isClickNextPhase = false;
                }
                break;
            case PlayerPhases.SKILL:
                if (isClickNextPlayer)
                {
                    
                    UIManager.Instance.HidePanelSkill();
                    //ChangePhase(PlayerPhases.BUILD);
                    
                    isClickNextPlayer = false;
                }
                break;
        }
    }

    private void AfterScoutingPhase()
    {
        if (playerOneActive)
        {
            if (playerOne.Bullets >= 1)
            {
                ChangePhase(PlayerPhases.FIRE);
            }
            else
            {
                ChangePhase(PlayerPhases.SKILL);
            }
        }
        else if (playerTwoActive)
        {
            if (playerTwo.Bullets >= 1)
            {
                ChangePhase(PlayerPhases.FIRE);
            }
            else
            {
                ChangePhase(PlayerPhases.SKILL);
            }
        }
    }

    public void NextPhase()
    {
        isClickNextPhase = true;
    }

    public void NextPlayer()
    {
        if (activePhase == PlayerPhases.SCOUTING)
        {
            return;
        }

        isClickNextPlayer = true;

        StartCoroutine(NextPlayerCoroutine());
    }


    private void WinnerPlayerOne()
    {
        winner = true;
        winnerPanel.SetActive(true);
        winnerText.text = $"Игрок 1";
    }
    private void WinnerPLayerTwo()
    {
        winner = true;
        winnerPanel.SetActive(true);
        winnerText.text = $"Игрок 2";
    }

    IEnumerator NextPlayerCoroutine()
    {
        if (playerOneActive)
        {
            //yield return new WaitForSeconds(timeToMove);
            changePlayerPanel.SetActive(true);
            buttonPhase.gameObject.SetActive(false);
            yield return new WaitForSeconds(timeToMove);

            ActivePlayerTwo();
            if (playerTwo.BuildingPoints >= pointsBuilding)
            {
                ChangePhase(PlayerPhases.BUILD);
            }
            else
            {
                ChangePhase(PlayerPhases.SCOUTING);
            }
            buttonPhase.gameObject.SetActive(true);
            changePlayerPanel.SetActive(false);
        }
        else if (playerTwoActive)
        {
            //yield return new WaitForSeconds(timeToMove);
            changePlayerPanel.SetActive(true);
            buttonPhase.gameObject.SetActive(false);
            yield return new WaitForSeconds(timeToMove);

            ActivePlayerOne();
            if (playerOne.BuildingPoints >= pointsBuilding)
            {
                ChangePhase(PlayerPhases.BUILD);
            }
            else
            {
                ChangePhase(PlayerPhases.SCOUTING);
            }
            buttonPhase.gameObject.SetActive(true);
            changePlayerPanel.SetActive(false);
        }
    }
}
