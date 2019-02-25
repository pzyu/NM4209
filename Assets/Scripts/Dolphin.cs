using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dolphin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (collision.gameObject.GetComponent<Player>().IsFlipping()) {
                spriteRenderer.DOFade(0.0f, 0.5f);
                Debug.Log("DOLPHIN KILLED");
                GameController.gameControllerInstance.boostSpeed = 4.0f;
            } else {
                collision.gameObject.GetComponent<Player>().DamagePlayer();
            }
        }
    }
}
