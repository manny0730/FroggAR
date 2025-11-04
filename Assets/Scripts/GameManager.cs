using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private UI_GameManager UI_GameManagerScript;

    private int totalPlatformsToVisit = 0;
    private int visitedPlatformsCount = 0;    

    private bool allNormalPlatformsVisited = false;
    private PlatformManager finishLinePlatform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1;

    }
    public void RegisterPlatform()
    {
        totalPlatformsToVisit++;
    }
    public void RegisterFinishLine(PlatformManager platform)
    {
        finishLinePlatform = platform;
        finishLinePlatform.setAsFinishLine();
    }

    public void RegisterStartLine(PlatformManager platform)
    {
        finishLinePlatform = platform;
        finishLinePlatform.setAsStartLine();
    }
    public void MarkPlatformAsVisited()
    {
        if (allNormalPlatformsVisited) return;

        visitedPlatformsCount++;
        UI_GameManagerScript.disableWarning();

        if (totalPlatformsToVisit > 0 && visitedPlatformsCount >= totalPlatformsToVisit)
        {
            allNormalPlatformsVisited = true;
        }
    }
    public void ReportFinishLineLanding()
    {
        if (allNormalPlatformsVisited)
        {
            Time.timeScale = 0;
            UI_GameManagerScript.enableWinScreen();
        }
        else
        {
            Debug.Log("This is the finish line, but you haven't visited all other platforms yet!");
            UI_GameManagerScript.enableWarning();
        }      
    }
}
