using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public enum PlayerMode
    {
        OnFoot,
        OnPlane,
        OnRocket
    }

    public PlayerMode playerMode;

    [SerializeField] GameObject[] mobileControlsPanel;


    int coinValue;
     int fruitsvalue;
    int bombValue;

    [SerializeField]TMP_Text coinValueDisplay;
    [SerializeField] TMP_Text lifeValueDisplay;
    [SerializeField] TMP_Text fruitsValueDisplay;
    [SerializeField] TMP_Text levelValueDisplay;
    [SerializeField]  GameObject fruitDisplay;
  


   [SerializeField] GameObject levelSelectPanel;
    [SerializeField] GameObject gameOverPanel;
   

    [Space(2)]
    [Header("Background")]
    [SerializeField]GameObject level2Background;
    [SerializeField]GameObject level3Background;


    public UnityEvent OnScaleBee;
    private void Start()
    {
        coinValue = 0;
        coinValueDisplay.text = coinValue.ToString();

        fruitsvalue = 0;
        fruitsValueDisplay.text = fruitsvalue.ToString();

        levelValueDisplay.text = GameManager.Instance.GetActiveScene().ToString();

        gameOverPanel.SetActive(false);
        levelSelectPanel.SetActive(false);

       

        switch (playerMode) 
        {
            case PlayerMode.OnFoot:
                MobileControlsActive(0);

                    break;
            case PlayerMode.OnPlane:
                if (level2Background != null)
                {
                    level2Background.SetActive(false);
                    fruitDisplay.SetActive(false);
                }
                MobileControlsActive(1); 
                ; break;
            case PlayerMode.OnRocket:
                MobileControlsActive(2);


                if (level3Background != null) 
                {
                    level3Background.SetActive(true);
                    fruitDisplay.SetActive(false);
                }


                ; break;
        }
       
    }

    public void UpdateCoinValue(int value) 
    {
        coinValue += value;
        coinValueDisplay.text = coinValue.ToString();
    }

    public void UpdateLifeValue(short value) 
    {
      lifeValueDisplay.text = value.ToString();
    }


    public void IncreaseFruitsValue() 
    {
        fruitsvalue++;
        fruitsValueDisplay.text = fruitsvalue.ToString();
    }

    public void DicreaseFruitsValue() 
    {
        fruitsvalue--;

        if (fruitsvalue < 1) 
        {
            fruitsvalue = 0;
        }


        fruitsValueDisplay.text = fruitsvalue.ToString();
    }

    public void MobileControlsActive(int index) 
    {
        foreach (GameObject m_cpanels in mobileControlsPanel) 
        {
            m_cpanels.SetActive(false);

        }

        mobileControlsPanel[index].SetActive(true);
       // mobileControlsPanel[index].transform.parent.gameObject.SetActive(true);

    }

    public void UpscaleBee() 
    {
        OnScaleBee?.Invoke();
    }

    public void SettingsButton() 
    {
        GameManager.Instance.OpenSettingsPanel();
    }

    public void LevelComplete() 
    {
       levelSelectPanel.SetActive(true);
    
    }

    public void GameOver() 
    {
        gameOverPanel.SetActive(true);  
    }


    public void RestartGame() 
    {
        GameManager.Instance.RestartGame();
    }

    public void LoadNext() 
    {
        if (GameManager.Instance.GetActiveScene() == 2)
        {
            GameManager.Instance.EndGame();
        }
        else 
        {
            GameManager.Instance.LoadNextGame();
        }

        
    }

}
