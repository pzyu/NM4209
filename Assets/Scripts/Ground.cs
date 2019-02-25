using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private GameObject palmPrefab;

    [SerializeField]
    private float minRange;
    [SerializeField]
    private float maxRange;

    [SerializeField]
    private Transform frontObjects;
    [SerializeField]
    private Transform backObjects;

    private List<Transform> frontObjectList;
    private List<Transform> backObjectList;

    [SerializeField]
    private int minAmountToSpawn;
    [SerializeField]
    private int maxAmountToSpawn;

    [SerializeField]
    private int minDistanceBetweenObjects;
    [SerializeField]
    private int maxDistanceBetweenObjects;

    [SerializeField]
    private GameObject obstaclePrefab;

    private List<GameObject> obstacleList;

    [SerializeField]
    private int minObstaclesToSpawn;
    [SerializeField]
    private int maxObstaclesToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnvironmentObjects();
        SpawnObstacles();
    }

    private void SpawnEnvironmentObjects() {
        frontObjectList = new List<Transform>();
        backObjectList = new List<Transform>();

        int amountToSpawn = Random.Range(minAmountToSpawn, maxAmountToSpawn);

        float currentSpawnDistance = Random.Range(minRange, minRange + minDistanceBetweenObjects);
        
        for (int i = 0; i < amountToSpawn; i++) {
            bool isSpawningFront = Random.Range(0, 2) == 0;

            if (i > 0) {
                currentSpawnDistance += Random.Range(minDistanceBetweenObjects, maxDistanceBetweenObjects);
            }
            Debug.Log("Current Spawn distance: " + currentSpawnDistance);
            
            if (isSpawningFront) {
                // Front objects
                GameObject palm = Instantiate(palmPrefab, new Vector3(transform.localPosition.x + currentSpawnDistance, 0, frontObjects.transform.localPosition.z), Quaternion.identity, frontObjects);
                float palmScale = Random.Range(0.4f, 0.45f);
                palm.transform.localScale = Vector3.one * palmScale;
                palm.GetComponent<SpriteRenderer>().sortingLayerName = "FrontLayer";
                //palm.GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 0;

                palm.GetComponent<cakeslice.Outline>().eraseRenderer = true;

                frontObjectList.Add(palm.transform);
            } else {
                // Back objects
                GameObject palm = Instantiate(palmPrefab, new Vector3(transform.localPosition.x + currentSpawnDistance, 0, backObjects.transform.localPosition.z), Quaternion.identity, backObjects);
                float palmScale = Random.Range(0.3f, 0.35f);
                palm.transform.localScale = Vector3.one * palmScale;
                palm.GetComponent<SpriteRenderer>().sortingLayerName = "BackLayer";
                //palm.GetComponent<SpriteRenderer>().flipX = true;

                palm.GetComponent<cakeslice.Outline>().enabled = false;

                frontObjectList.Add(palm.transform);
            }
        }
    }

    public void SetEnvironmentPositions() {
        float currentSpawnDistance = Random.Range(minRange, minRange + minDistanceBetweenObjects);

        for (int i = 0; i < frontObjectList.Count; i++) {
            if (i > 0) {
                currentSpawnDistance += Random.Range(minDistanceBetweenObjects, maxDistanceBetweenObjects);
            }

            frontObjectList[i].transform.localPosition = new Vector3(currentSpawnDistance, 0, 0);
        }
    }

    private void SpawnObstacles() {
        int amountToSpawn = Random.Range(minObstaclesToSpawn, maxObstaclesToSpawn);
        obstacleList = new List<GameObject>();

        for (int i = 0; i < amountToSpawn; i++) {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacleList.Add(obstacle);
        }

        SetObstaclesPositions();
    }

    public void SetObstaclesPositions() {
        float currentSpawnDistance = Random.Range(minRange * 2, minRange * 2 + minDistanceBetweenObjects);

        for (int i = 0; i < obstacleList.Count; i++) {
            if (i > 0) {
                currentSpawnDistance += Random.Range(minDistanceBetweenObjects, maxDistanceBetweenObjects);
            }

            obstacleList[i].transform.localPosition = new Vector3(currentSpawnDistance, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
