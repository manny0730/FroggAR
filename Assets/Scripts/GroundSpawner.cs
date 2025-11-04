using UnityEngine;
using Vuforia;

public class GroundSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform frogLocation;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GroundPointController GroundPointControllerScript;
    [SerializeField] private MidAirGroundSpawner MidAirGroundSpawnerScript;
    [SerializeField] private UI_GameManager UI_GameManagerScript;
    [SerializeField] private SoundManager SoundManagerScript;
    [SerializeField] private GameObject planeLogistics;    

    public bool spawnKillZone = false;

    private bool firstTime = true;
    private bool lastTime = false;
    private HitTestResult previousHit;
    private int spawnCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planeLogistics.SetActive(true);
        UI_GameManagerScript.disableHealthContainer();
        UI_GameManagerScript.enableFirstStep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void intersectionLocation(HitTestResult intersection)
    {
        if (intersection != null)
        {
            previousHit = intersection;
        }
    }
    
    public void createGround()
    {

        if (firstTime && spawnCount < 2)
        {
            firstTime = false;

            GameObject newGround = Instantiate(groundPrefab, previousHit.Position, previousHit.Rotation);

            PlatformManager platformScript = newGround.GetComponent<PlatformManager>();

            GameManager.Instance.RegisterPlatform();
            GameManager.Instance.RegisterStartLine(platformScript);

            float groundHeight = groundPrefab.GetComponent<Renderer>().bounds.size.y;

            Vector3 frogPosition = previousHit.Position + new Vector3(0, groundHeight + 1f, 0);
            frogLocation.position = frogPosition;

            spawnCount += 1;

            GroundPointControllerScript.addGroundPoints(previousHit.Position);

            MidAirGroundSpawnerScript.switchSpawner();
            SoundManagerScript.playGroundPlacementSFX();

            planeLogistics.SetActive(false);
            UI_GameManagerScript.disableFirstStep();
            

        }

        if (lastTime && spawnCount < 2)
        {            

            GameObject newGround = Instantiate(groundPrefab, previousHit.Position, previousHit.Rotation);
            PlatformManager platformScript = newGround.GetComponent<PlatformManager>();

            GameManager.Instance.RegisterFinishLine(platformScript);
            SoundManagerScript.playGroundPlacementSFX();

            spawnCount += 1;

            GroundPointControllerScript.addGroundPoints(previousHit.Position);

            if (lastTime && spawnCount == 2)
            {
                startGame();
                spawnKillZone = true;
            }
        }
    }

    public void startGame()
    {
        if (spawnCount == 2)
        {
            planeLogistics.SetActive(false);
            frogLocation.gameObject.SetActive(true);
            UI_GameManagerScript.disableThirdStep();
            UI_GameManagerScript.enableFinalStep();
            UI_GameManagerScript.enableHealthContainer();
        }
        
    }

    public void switchTracker()
    {
        lastTime = true;
        planeLogistics.SetActive(true);
        UI_GameManagerScript.enableThirdStep();
    }
}
