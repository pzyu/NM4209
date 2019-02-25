using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameController.gameControllerInstance.isGamePaused) {
            return;
        }

        if (collision.tag == "Player") {
            transform.parent.GetComponent<Obstacle>().TriggerObstacle();
            collision.GetComponent<Player>().ShowWarning();
        }
    }
}
