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
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip nextPhaseClip;


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
    private bool shot = false;
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

    public bool PlayerOneActive
    {
        get
        {
            return playerOneActive;
        }
    }
    public bool PlayerTwoActive
    {
        get
        {
            return playerTwoActive;
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        cells = FindObjectsOfType<Cell>();
        veil = FindObjectsOfType<Veil>();

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
            if (playerOneActive && (playerOne.BuildingPoints >= pointsBuilding))
            {
                GameObject newObject = Instantiate(castlePlayerOne, target - new Vector3(0, playerPosition,0), Quaternion.identity);
                castlesPlayerOne.Add(newObject);
                playerOne.ZeroBuildingPoints();
                playerOne.AddCastle();
            }
            else if (playerTwoActive && (playerTwo.BuildingPoints >= pointsBuilding))
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
            if (playerOneActive)
            {
                if(playerOne.BuildingPoints >= pointsBuilding)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (playerTwoActive)
            {
                if (playerTwo.BuildingPoints >= pointsBuilding)
                {
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
        else
        {
            return false;
        }
    }
    public bool CheckFirePhase()
    {
        if (activePhase == PlayerPhases.FIRE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckScoutingPhase()
    {
        if (activePhase == PlayerPhases.SCOUTING)
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
        if (activePhase == PlayerPhases.FIRE)
        {
            if (playerOneActive)
            {
                if (playerOne.Bullets > 0 && shot)
                {
                    playerOne.Shot();
                    shot = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (playerTwoActive)
            {
                if (playerTwo.Bullets > 0 && shot)
                {
                    playerTwo.Shot();
                    shot = false;
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
        else
        {
            return false;
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

        ActiveGeneralMap();

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
                //AudioManager.Instance.PlaySound(nextPhaseClip);
                activePhaseText.text = $"Выберите место для строительства замка";
                BuildingPhase();
                break;
            case PlayerPhases.SCOUTING:
                //AudioManager.Instance.PlaySound(nextPhaseClip);
                activePhaseText.text = $"Выберите область исследования!";
                ScoutingPhase();
                break;
            case PlayerPhases.FIRE:
                //AudioManager.Instance.PlaySound(nextPhaseClip);
                activePhaseText.text = $"Выберите область для обстрела!";
                shot = true;
                ShellingPhase();
                break;
            case PlayerPhases.SKILL:
                //AudioManager.Instance.PlaySound(nextPhaseClip);
                activePhaseText.text = $"Что вы хотите улучшить?";
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
                    if (playerOneActive)
                    {
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

                    UIManager.Instance.HidePanelSkill();
                    
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
    }

    private void WinnerPlayerOne()
    {
        winner = true;
        winnerPanel.SetActive(true);
        winnerText.text = $"Игрок 1";
        AudioManager.Instance.PlaySound(winClip);
    }
    private void WinnerPLayerTwo()
    {
        winner = true;
        winnerPanel.SetActive(true);
        winnerText.text = $"Игрок 2";
        AudioManager.Instance.PlaySound(winClip);
    }
    
    public void ChangePlayer()
    {
        changePlayerPanel.SetActive(true);
        buttonPhase.gameObject.SetActive(false);
    }

    IEnumerator NextPlayerCoroutine()
    {
        if (playerOneActive)
        {
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
