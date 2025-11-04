using System.Collections.Generic;
using UnityEngine;

public class GroundPointController : MonoBehaviour
{

    [SerializeField] private Transform killZone;
    [SerializeField] private GroundSpawner GroundSpawnerScript;

    private List<Vector3> groundPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundPoints = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GroundSpawnerScript.spawnKillZone)
        {
            SpawnAtLowestPoint();
            killZone.gameObject.SetActive(true);
        }
    }
    public void SpawnAtLowestPoint()
    {
        Vector3 lowestPoint = groundPoints[0];

        for (int i = 1; i < groundPoints.Count; i++)
        {
            if (groundPoints[i].y < lowestPoint.y)
            {
                lowestPoint = groundPoints[i];
            }
        }

        Vector3 spawnPosition = new Vector3(lowestPoint.x, lowestPoint.y - 20f, lowestPoint.z);
        killZone.position = spawnPosition;
    }

    public void addGroundPoints(Vector3 newGroundPoints)
    {
        groundPoints.Add(newGroundPoints);
    }
}
