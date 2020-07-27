using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region SingleTon

    public static UIManager Instance { get; private set; }



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
    [SerializeField] GameObject playerOnePanel;
    [SerializeField] GameObject skillPanelPlayerOne;
    [SerializeField] Text scoutingPlayerOne;
    [SerializeField] Text bulletPlayerOne;
    [SerializeField] Text hpInfoPlayerOne;
    [SerializeField] Slider healthPlayerOne;

    [Header("Player 2")]
    [SerializeField] Player playerTwo;
    [SerializeField] GameObject playerTwoPanel;
    [SerializeField] GameObject skillPanelPlayerTwo;
    [SerializeField] Text scoutingPlayerTwo;
    [SerializeField] Text bulletPlayerTwo;
    [SerializeField] Text hpInfoPlayerTwo;
    [SerializeField] Slider healthPlayerTwo;



    
    private void Start()
    {
        StartHealth();
        playerOne.onScoutingPoint += ShowScoutingPlayerOne;
        playerOne.onBulletChanged += ShowBulletPLayerOne;
        playerOne.onCastleChanged += SliderPlayerOne;

        playerTwo.onScoutingPoint += ShowScoutingPlayerTwo;
        playerTwo.onBulletChanged += ShowBulletPlayerTwo;
        playerTwo.onCastleChanged += SliderPlayerTwo;

        ShowBulletPLayerOne();
        ShowBulletPlayerTwo();
    }

    private void StartHealth()
    {
        healthPlayerOne.maxValue = playerOne.MaxCastles;
        healthPlayerOne.value = playerOne.Castles;
        hpInfoPlayerOne.text = $"{playerOne.Castles}/{playerOne.MaxCastles}";

        healthPlayerTwo.maxValue = playerTwo.MaxCastles;
        healthPlayerTwo.value = playerTwo.Castles;
        hpInfoPlayerTwo.text = $"{playerTwo.Castles}/{playerTwo.MaxCastles}";
    }

    public void ShowPanelPlayerOne()
    {
        playerOnePanel.SetActive(true);
        playerTwoPanel.SetActive(false);
    }
    public void ShowPanelPlayerTwo()
    {
        playerOnePanel.SetActive(false);
        playerTwoPanel.SetActive(true);
    }


    public void ShowSkillPanelOne()
    {
        skillPanelPlayerOne.SetActive(true);
        skillPanelPlayerTwo.SetActive(false);
    }
    public void ShowSkillPanelTwo()
    {
        skillPanelPlayerTwo.SetActive(true);
        skillPanelPlayerOne.SetActive(false);
    }

    public void HidePanelSkill()
    {
        skillPanelPlayerOne.SetActive(false);
        skillPanelPlayerTwo.SetActive(false);
    }


    private void ShowScoutingPlayerOne()
    {
        scoutingPlayerOne.text = $"{playerOne.ScoutingPoints}";
    }
    private void ShowScoutingPlayerTwo()
    {
        scoutingPlayerTwo.text = $"{playerTwo.ScoutingPoints}";
    }
    
    private void ShowBulletPLayerOne()
    {
        bulletPlayerOne.text = $"{playerOne.Bullets}";
    }
    private void ShowBulletPlayerTwo()
    {
        bulletPlayerTwo.text = $"{playerTwo.Bullets}";
    }

    private void SliderPlayerOne()
    {
        healthPlayerOne.maxValue = playerOne.MaxCastles;
        healthPlayerOne.value = playerOne.Castles;
        hpInfoPlayerOne.text = $"{playerOne.Castles}/{playerOne.MaxCastles}";
    }
    private void SliderPlayerTwo()
    {
        healthPlayerTwo.maxValue = playerTwo.MaxCastles;
        healthPlayerTwo.value = playerTwo.Castles;
        hpInfoPlayerTwo.text = $"{playerTwo.Castles}/{playerTwo.MaxCastles}";
    }
}
