using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private bool canFlip = true;

    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private SpriteRenderer warning;

    private CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        canFlip = true;

        warning.DOFade(0.0f, 0.001f);

        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MicInput.isThresholdBroken && canFlip) {
            Flip();
        }
    }

    public void Flip() {
        canFlip = false;
        circleCollider.radius = 0.6f;
        transform.DOLocalRotate(new Vector3(0, 0, 360), duration, RotateMode.LocalAxisAdd).OnComplete(()=> {
            canFlip = true;
            circleCollider.radius = 0.4f;
        });
    }

    public void ShowWarning() {
        warning.DOFade(1.0f, 1.0f).SetEase(Ease.Flash, 6).OnComplete(() => {
            warning.DOFade(0.0f, 0.001f);
        });
    }

    public bool IsFlipping() {
        return !canFlip;
    }

    public void DamagePlayer() {
        Debug.Log("PLAYER DAMAGED");
        //GameController.gameControllerInstance.PauseGame();
    }
}
