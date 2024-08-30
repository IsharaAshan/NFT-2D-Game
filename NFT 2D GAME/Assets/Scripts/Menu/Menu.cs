using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]TMP_Text mainCoinDisplay;

    [SerializeField]GameObject profilePanel;
    [SerializeField]GameObject shopPanel;

    [SerializeField]Button walletConnectButton;

    bool isWallConnect;
    [SerializeField] Sprite connectButtonSprite;
    [SerializeField] Sprite disConnectButtonSprite;




    private void Start()
    {
        mainCoinDisplay.text = $"{GameManager.Instance.MainCoinValue}";
        GameManager.Instance.PlaySfx("BG");
      

        profilePanel.SetActive(false);
        shopPanel.SetActive(false);

        if (isWallConnect)
        {
            walletConnectButton.image.sprite = disConnectButtonSprite;
        }
        else 
        {
            walletConnectButton.image.sprite = connectButtonSprite;
        }

    }


    public void OpenProfileButotn(bool isActive) 
    {
        if (isActive)
        {
           profilePanel.SetActive(true);
        }

        else 
        {
            profilePanel.SetActive(false);
        }

        GameManager.Instance.PlaySfx("click");
    }

    public void OpenShopPanel(bool isActive) 
    {
        if (isActive)
        {
            shopPanel.SetActive(true);
        }
        else 
        {
           shopPanel.SetActive(false);
        }
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
