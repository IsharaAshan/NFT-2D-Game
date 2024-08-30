using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] Transform player;
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

    [SerializeField] TMP_Text levelValueDisplay;
    [SerializeField] TMP_Text[] endCoinValue;

    byte tempBeeValue =0;

    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject gameOverPanel;

    //Level 1
    public bool IsPlayerHasMeat { get; set; }
    public float MainSpeed { get; set; } = 5; 

    public UnityEvent OnGameStop;
    public UnityEvent<float> OnMoveSpeedChange;

    [Header("MobieControlsUI")]
    [SerializeField] GameObject[] levelsUiControls;

    //Level 2
    public GameObject level2Sky;

    //Level 3
    [SerializeField]GameObject exitPortal;
    [SerializeField] GameObject objectPack;
    [SerializeField]byte teleportMode;

    


    private void Start()
    {
        gameManager = GameManager.Instance;

        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);

        gameManager.StopSound("JetEngine");
        gameManager.StopSound("planeEngine");


        lifeValue = gameManager.MainLifeValue;

        coinValue = gameManager.MainCoinValue;

        coinValueDisplay.text =coinValue.ToString();
        deadValueDisplay.text = deadBeeValue.ToString();    
        fruiteDisplay.text = fruiteValue.ToString();
        levelValueDisplay.text = $"Level {gameManager.GetActiveScene()}";

        LifeUpdate();
        UIControlsActive();

        
       
    }


    public void UIControlsActive() 
    {
        foreach (var level in levelsUiControls) 
        {
            level.SetActive(false);
        }

        if (playerMode == PlayerMode.Ground)
        {
            levelsUiControls[0].SetActive(true);
            fruiteDisplay.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        else if (playerMode == PlayerMode.Sky)
        {
            levelsUiControls[1].SetActive(true);
            gameManager.PlaySfx("planeEngine");
            if (level2Sky != null)
            {

                level2Sky.SetActive(true);

            }
        }
        else 
        {
            levelsUiControls[2].SetActive(true);
            gameManager.PlaySfx("JetEngine");
            exitPortal.SetActive(false);
        }



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
            MainSpeed = 0;
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
        GameManager.Instance.UpdateCoinValue(coinValue);
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

    public void MoveSpeedChange(float value)
    {
        MainSpeed = value;
    }

    public void TeleportCheck() 
    {
        MainSpeed = 0;
        GameManager.Instance.PlaySfx("teleportEnter");
       
        if (teleportMode == 0) 
        {
            teleportMode = 1;
        }
        else  
        {
            teleportMode = 0;
        }

        if(player != null)
        {
            if (objectPack != null)
            {
                foreach (Transform i in objectPack.transform) 
                {
                    BeeEnemy b;
                    Planets p;

                    // Try to get the BeeEnemy component
                    if (i.TryGetComponent<BeeEnemy>(out b))
                    {
                        // Assign start position for BeeEnemy
                        b.AssignStartPostion();

                        // Adjust the scale based on teleportMode
                        if (teleportMode == 0)
                        {
                            b.transform.localScale = new Vector3(.8f, .8f, 1f);
                        }
                        else
                        {
                            b.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                    }

                    // Try to get the Planets component
                    if (i.TryGetComponent<Planets>(out p))
                    {
                        // Assign start position for Planets
                        p.AssignStartPostion();
                    }


                }

              
             
                objectPack.SetActive(false);

              



            }

            StartCoroutine(Teleporting());

    }

    IEnumerator Teleporting() 
    {
        
           

            FindObjectOfType<ObjectsMover>().transform.position = new Vector2(15, 0);
            player.GetComponent<PlayerControl>().isCanControl = false;
            player.transform.GetChild(2).gameObject.SetActive(false);
            player.position = new Vector2(0, 0);
            yield return new WaitForSeconds(1);
            GameManager.Instance.PlaySfx("teleportExit");
            exitPortal.SetActive(true);
            yield return new WaitForSeconds(.8f);
            player.transform.GetChild(2).gameObject.SetActive(true);
            exitPortal.SetActive(false);
            player.GetComponent<PlayerControl>().isCanControl = true;
            MainSpeed = 5;

            if (objectPack != null) {

              
                objectPack.SetActive(true);
            }

        }
    }

    public void LevelWinCheck()
    {
        if (deadBeeValue >= targetBeeValue && coinValue >= targetCoinValue) 
        {
            MoveSpeedChange(0); 
            
            foreach (var tmpText in endCoinValue) 
            {
                tmpText.text =  $"{coinValue}";
            }


            gameManager.PlaySfx("LevelComplete");
            levelCompletePanel.SetActive(true);
            OnGameStop?.Invoke();
            GameManager.Instance.MainCoinValue += coinValue;
            gameManager.StopSound("JetEngine");
            gameManager.StopSound("planeEngine");
            gameManager.MainCoinValue += coinValue;
           
        }
    }

    public void GameOver() 
    {
        OnGameStop?.Invoke();
        MoveSpeedChange(0);

        foreach (var tmpText in endCoinValue)
        {
            tmpText.text = $"{coinValue}";
        }

        gameManager.PlaySfx("GameOver");
        gameManager.StopSound("JetEngine");
        gameManager.StopSound("planeEngine");
        gameOverPanel.SetActive(true);
    }

    public void PauseButton() 
    {
       
        gameManager.PausePanelOpen();

    }

    public void NextButton() 
    {
        gameManager.LoadNextScene();
    
    }

    public void RestartButtton() 
    {
       gameManager.RestartScene();
        Destroy(Pooler.Instance);
    }

    public void Home() 
    {
        gameManager.StopSound("JetEngine");
        gameManager.StopSound("planeEngine");
        gameManager.HomeMenu();
    }

   
}
