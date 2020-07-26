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
    
    [Header("Player1 and Player2")]
    [SerializeField] Player playerOne;
    [SerializeField] Player playerTwo;

    [SerializeField] float playerPosition = 0.1f;
    [SerializeField] float timeToMove = 5f;
    [SerializeField] int damage = 20; //урон наносится одинаковый. В будущем, возможно, можно сделать что и наносимый урон можно прокачивать
    [SerializeField] int bonusHealth;


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

    public int BonusHealth
    {
        get
        {
            return bonusHealth;
        }
    }

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
        SCOUTING,
        FIRE,
        SKILL
    }
    

    // Start is called before the first frame update
    void Start()
    {
        cells = FindObjectsOfType<Cell>();
        veil = FindObjectsOfType<Veil>();
        GeneratePosition();

        invisiblePosition = positionPlayers.transform.position;


        ActivePlayerOne();
        ChangePhase(PlayerPhases.SCOUTING);

        playerOne.onLose += WinnerPLayerTwo;
        playerTwo.onLose += WinnerPlayerOne;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePhase();
    }


    //public void CheckDetection(Vector3 target)
    //{
    //    Debug.Log("BLABLABLA");
    //    if (playerOneActive && target == positionPlayerTwo)
    //    {
    //        playerTwo.Detection();
    //        Debug.Log("playertwo detection!");
    //    }
    //    else if (playerTwoActive && target == positionPlayerOne)
    //    {
    //        playerOne.Detection();
    //        Debug.Log("player 1 detection");
    //    }

    //}
    public void DamagePosition(Vector3 target)
    {
        if (playerOneActive)
        {
            if (target == positionPlayerTwo)
            {
                Debug.Log("Попадание в игрока 2");
                playerTwo.DoDamage();
            }
            else
            {
                Debug.Log("Мимо!");
            }
        }
        else if (playerTwoActive)
        {
            if (target == positionPlayerOne)
            {
                Debug.Log("Попадание в игрока 1");
                playerOne.DoDamage();

            }
            else
            {
                Debug.Log("Мимо!");
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

                playerOne.transform.position = newPositionOne;
                playerTwo.transform.position = newPositionTwo;    
            }
        }
    }


    //Активный игрок
    private void ActivePlayerOne()
    {
        playerOneActive = true;
        playerTwoActive = false;

        mapPlayerOne.SetActive(false);
        mapPlayerTwo.SetActive(true);

        playerOne.ResetScoutingPoints();

        playerOne.transform.position = invisiblePosition;
        playerTwo.transform.position = newPositionTwo;

        UIManager.Instance.ShowPanelPlayerOne();
    }
    private void ActivePlayerTwo()
    {
        playerOneActive = false;
        playerTwoActive = true;

        mapPlayerOne.SetActive(true);
        mapPlayerTwo.SetActive(false);

        playerTwo.ResetScoutingPoints();

        playerOne.transform.position = newPositionOne;
        playerTwo.transform.position = invisiblePosition;

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
            case PlayerPhases.SCOUTING:
                if (isClickNextPhase)
                {
                    ChangePhase(PlayerPhases.FIRE);
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
                    ChangePhase(PlayerPhases.SCOUTING);
                    isClickNextPlayer = false;
                }
                break;
        }
    }

    public void NextPhase()
    {
        Debug.Log("NEXT PHASE!!!");
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
            buttonPhase.gameObject.SetActive(true);
            changePlayerPanel.SetActive(false);
            ActivePlayerTwo();
        }
        else if (playerTwoActive)
        {
            //yield return new WaitForSeconds(timeToMove);
            changePlayerPanel.SetActive(true);
            buttonPhase.gameObject.SetActive(false);
            yield return new WaitForSeconds(timeToMove);
            buttonPhase.gameObject.SetActive(true);
            changePlayerPanel.SetActive(false);
            ActivePlayerOne();
        }
    }
}
