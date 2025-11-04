using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<HealthUI> allHealths = new List<HealthUI>();
    [SerializeField] List<Image> stepsImages = new List<Image>();
    [SerializeField] List<Image> endScreen = new List<Image>();
    [SerializeField] private Image HealthContainer;

    public bool finalStepActive;
    public void restartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }
    public void loadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void decreaseHealth()
    {
        for (int i = allHealths.Count-1; i >= 0; i--)
        {
            if (allHealths[i].isActive)
            {
                allHealths[i].DisableHealth();
                break;
            }
        }
    }
    public void enableHealthContainer()
    {
        HealthContainer.gameObject.SetActive(true);
    }
    public void disableHealthContainer()
    {
        HealthContainer.gameObject.SetActive(false);
    }
    public void enableFirstStep()
    {
        stepsImages[0].gameObject.SetActive(true);
    }
    public void disableFirstStep()
    {
        stepsImages[0].gameObject.SetActive(false);
    }
    public void enableSecondStep()
    {
        stepsImages[1].gameObject.SetActive(true);
    }
    public void disableSecondStep()
    {
        stepsImages[1].gameObject.SetActive(false);
    }
    public void enableThirdStep()
    {
        stepsImages[2].gameObject.SetActive(true);
    }
    public void disableThirdStep()
    {
        stepsImages[2].gameObject.SetActive(false);
    }
    public void enableFinalStep()
    {
        stepsImages[3].gameObject.SetActive(true);
        finalStepActive = true;
    }
    public void disableFinalStep()
    {
        stepsImages[3].gameObject.SetActive(false);
        finalStepActive = false;
    }
    public void enableWarning()
    {
        stepsImages[4].gameObject.SetActive(true);
    }
    public void disableWarning()
    {
        stepsImages[4].gameObject.SetActive(false);
    }
    public void enableWinScreen()
    {
        endScreen[0].gameObject.SetActive(true);
    }
    public void enableLoseScreen()
    {
        endScreen[1].gameObject.SetActive(true);
    }
}
