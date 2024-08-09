using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    public enum PlayerMode {Ground,Sky,Space}
    public PlayerMode playerMode = PlayerMode.Ground;

    public int targetCoinValue;
    public int targetBeeValue;

    int coinValue;
    [SerializeField]TMP_Text coinValueDisplay;

    int lifeValue;
    [SerializeField] GameObject[] lifeImages;

    int deadBeeValue;
    [SerializeField] TMP_Text deadValueDisplay;

    public  int fruiteValue;
    [SerializeField] TMP_Text fruiteDisplay;

    byte tempBeeValue =0;

    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject gameOverPanel;

    //Level 1
    public bool isPlayerHasMeat;


    public UnityEvent OnGameStop;

    private void Start()
    {
        gameManager = GameManager.Instance;

        lifeValue = gameManager.MainLifeValue;
        coinValueDisplay.text = coinValue.ToString();
        deadValueDisplay.text = deadBeeValue.ToString();    
        fruiteDisplay.text = fruiteValue.ToString();

        LifeUpdate();
    }


    public void LifeUpdate() 
    {
        foreach (GameObject lifeImage in lifeImages)
        {
            lifeImage.SetActive(false);
        }
        for (int i = 0; i < lifeValue; i++)
        {
            lifeImages[i].SetActive(true);
        }
    }

    public void LifeDiscrease() 
    {
        lifeValue--;
        tempBeeValue++;

        if (lifeValue <= 0) 
        {
            lifeValue = 0;
            GameOver();
            
        }

        if (tempBeeValue >= 5 ) 
        {
            tempBeeValue = 0;
            UpdateCoinValue();
        }

        LifeUpdate();
    }

    public void UpdateCoinValue() 
    {
        coinValue++;
        coinValueDisplay.text = coinValue.ToString();
        LevelWinCheck();
    }

    public void UpdateDeadBees() 
    {
        deadBeeValue++;
        deadValueDisplay.text = deadBeeValue.ToString();
        LevelWinCheck();
    }

    public void UpdateFruitValue(bool isPlus) 
    {
        if (isPlus)
        {
            fruiteValue++;
            
        }
        else 
        {
            fruiteValue--;

            if (fruiteValue <= 0) 
            {
                fruiteValue = 0;
            }
           
        }

        fruiteDisplay.text = fruiteValue.ToString();
    }

    public void LevelWinCheck()
    {
        if (deadBeeValue >= targetBeeValue && coinValue >= targetCoinValue) 
        {
            gameManager.MoveSpeedChange(0); 
            levelCompletePanel.SetActive(true);
            OnGameStop?.Invoke();
        }
    }

    public void GameOver() 
    {
        OnGameStop?.Invoke();
        gameManager.MoveSpeedChange(0);
        gameOverPanel.SetActive(true);
    }

    public void PauseButton() 
    {
        gameManager.PlaySfx("click");
        GameManager.Instance.PausePanelOpen();
    }

   
}
