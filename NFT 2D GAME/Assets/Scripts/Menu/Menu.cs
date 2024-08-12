using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]TMP_Text mainCoinDisplay;



    private void Start()
    {
        mainCoinDisplay.text = $"{GameManager.Instance.MainCoinValue}";
        GameManager.Instance.PlaySfx("BG");
    }

    public void PlayButton() 
    {
        GameManager.Instance.LoadNextScene();
        GameManager.Instance.PlaySfx("click");
    }

    public void ExitGame() 
    {
        Application.Quit();
        GameManager.Instance.PlaySfx("click");
    }

   
}
