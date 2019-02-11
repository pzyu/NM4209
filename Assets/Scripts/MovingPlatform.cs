using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform startAnchor;
    [SerializeField]
    private Transform endAnchor;

    [SerializeField]
    private float durationToActivate;
    private float currentDuration;

    private bool isActivating;
    private bool isActivated;

    private Sequence forwardSequence;
    private Sequence backwardsSequence;

    // Start is called before the first frame update
    void Start()
    {
        forwardSequence = DOTween.Sequence();
        forwardSequence.Append(transform.DOMove(endAnchor.transform.position, durationToActivate).SetEase(Ease.Linear));
        forwardSequence.SetAutoKill(false);
        forwardSequence.Pause();

        backwardsSequence = DOTween.Sequence();
        backwardsSequence.Append(transform.DOMove(startAnchor.transform.position, durationToActivate).SetEase(Ease.Linear));
        backwardsSequence.Pause();
    }

    // Update is called once per frame
    void Update() {
        CheckActivationDuration();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Voice") {
            StartActivating();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Voice") {
            StopActivating();
        }
    }

    private void StartActivating() {
        Debug.Log("Start");

        forwardSequence.Pause();
        forwardSequence.PlayForward();
    }

    private void StopActivating() {
        Debug.Log("Stop");
        //forwardSequence.Rewind();
        
        forwardSequence.Pause();
        if (forwardSequence.IsComplete()) {
            forwardSequence.SmoothRewind();
        } else {
            forwardSequence.PlayBackwards();
        }
    }

    private void Activate() {
        isActivated = true;


        Vector3 jumpPosition = new Vector3(transform.localPosition.x + Random.Range(-1.0f, 1.0f), transform.localPosition.y + Random.Range(0.5f, 1.0f));
        transform.DOLocalJump(jumpPosition, 1, 1, 0.5f);
        transform.DOLocalRotate(new Vector3(0, 0, Random.Range(-360, 360)), 0.5f, RotateMode.LocalAxisAdd);
        transform.DOScale(0.0f, 0.5f).OnComplete(ResetChecks);
    }

    private void CheckActivationDuration() {
        // If it is activated or is not activating
        if (isActivated || !isActivating) {
            return;
        }

        currentDuration += Time.deltaTime;

        if (currentDuration >= durationToActivate) {
            //Activate();
        }
    }

    private void ResetChecks() {
        currentDuration = 0;
        isActivated = false;
        isActivating = false;
        
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

    }
}
