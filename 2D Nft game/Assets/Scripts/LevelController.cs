using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    int coinValue;
     int fruitsvalue;
    int bombValue;

    [SerializeField]TMP_Text coinValueDisplay;
    [SerializeField] TMP_Text lifeValueDisplay;
    [SerializeField] TMP_Text fruitsValueDisplay;
    [SerializeField] TMP_Text levelValueDisplay;


    [SerializeField] GameObject levelSelectPanel;
    [SerializeField] GameObject gameOverPanel;

    private void Start()
    {
        coinValue = 0;
        coinValueDisplay.text = coinValue.ToString();

        fruitsvalue = 0;
        fruitsValueDisplay.text = fruitsvalue.ToString();

        levelValueDisplay.text = GameManager.Instance.GetActiveScene().ToString();

        gameOverPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
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
        SceneManager.LoadScene(0);
    }


}
