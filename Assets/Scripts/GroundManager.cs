using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;

    [SerializeField]
    private int amountToSpawn;

    [SerializeField]
    private float groundSize;

    [SerializeField]
    private List<GameObject> groundList;

    private float newXOffset;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        newXOffset =  groundSize * (amountToSpawn - 1);
        canMove = true;

        SpawnGround();
    }

    private void SpawnGround() {
        float groundOffset = 0;

        for (int i = 0; i < amountToSpawn; i++) {
            GameObject ground = Instantiate(groundPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

            ground.transform.localPosition = new Vector3(groundOffset, 0, 0);

            groundOffset += groundSize;

            groundList.Add(ground);
        }
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement() {
        foreach (GameObject ground in groundList) {
            ground.transform.localPosition -= transform.right * Time.deltaTime * GameController.gameControllerInstance.speed;

            if (ground.transform.localPosition.x <= -groundSize && canMove) {
                canMove = false;
                ground.transform.localPosition = new Vector2(newXOffset, 0);
                ground.GetComponent<Ground>().SetObstaclesPositions();
                StartCoroutine(ResetCanMove());
            }
        }
    }

    private IEnumerator ResetCanMove() {
        yield return new WaitForSeconds(0.25f);
        canMove = true;
    }

    private void MoveToNewPosition(GameObject gameObject) {
        gameObject.transform.localPosition = new Vector2(newXOffset, 0);
        canMove = true;
    }

    private void LoopGround() {

    }
}
