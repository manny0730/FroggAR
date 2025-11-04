using UnityEngine;
using Vuforia;

public class MidAirGroundSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GroundPointController GroundPointControllerScript;
    [SerializeField] private GroundSpawner GroundSpawnerScript;
    [SerializeField] private UI_GameManager UI_GameManagerScript;
    [SerializeField] private SoundManager SoundManagerScript;
    [SerializeField] private GameObject MidAirLogistics;

    private int nameCount = 0;
    private bool midAirTurn = false;
    private float scaleSpeed = 0.5f;    
    private GameObject currentGround;
    private Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MidAirLogistics.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!midAirTurn || spawnPoint == null)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (currentGround == null && nameCount < 5)
                    {
                        Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 0.5f;
                        currentGround = Instantiate(groundPrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
                        PlatformManager platformScript = currentGround.GetComponent<PlatformManager>();
                        GameManager.Instance.RegisterPlatform();
                    }
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if (currentGround != null)
                    {
                        float growthAmount = scaleSpeed * Time.deltaTime;
                        currentGround.transform.localScale += new Vector3(growthAmount, growthAmount, growthAmount);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (currentGround != null)
                    {
                        AnchorBehaviour myAnchor = currentGround.AddComponent<AnchorBehaviour>();
                        myAnchor.ConfigureAnchor("Anchor" + nameCount.ToString(), currentGround.transform.position, Quaternion.Euler(0, 0, 0));
                        SoundManagerScript.playGroundPlacementSFX();
                        nameCount += 1;

                        if (nameCount >= 5)
                        {
                            SoundManagerScript.playGroundPlacementSFX();
                            GroundSpawnerScript.switchTracker();
                            UI_GameManagerScript.disableSecondStep();
                            MidAirLogistics.SetActive(false);
                            midAirTurn = false;
                        }

                        currentGround = null;
                    }
                    break;
            }
        }
    }

    public void AnchorCreator(Transform worldPositioning)
    {
        spawnPoint = worldPositioning;
        GroundPointControllerScript.addGroundPoints(spawnPoint.position);


    }

    public void switchSpawner()
    {
        midAirTurn = true;
        MidAirLogistics.SetActive(true);
        UI_GameManagerScript.enableSecondStep();
    }
}
