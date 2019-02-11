using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VoiceInteractable : MonoBehaviour {
    [SerializeField]
    private float durationToActivate;
    private float currentDuration;

    private bool isActivating;
    private bool isActivated;

    private Sequence shakeSequence;
    private Sequence destructionSequence;

    private Vector3 originalPosition;
    
    private void Awake() {
        originalPosition = transform.localPosition;

        shakeSequence = DOTween.Sequence();
        shakeSequence.Append(transform.DOShakeRotation(durationToActivate, new Vector3(0, 0, 10), 10, 90, false));
        shakeSequence.Join(transform.DOShakePosition(durationToActivate, 0.05f, 20, 90, false, false));
        shakeSequence.OnComplete(() => {
            shakeSequence.Rewind();
        });
        shakeSequence.Pause();

        ResetChecks();
    }

    // Start is called before the first frame update
    void Start() {

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
        if (isActivating) {
            return;
        }

        isActivating = true;
        currentDuration = 0;

        shakeSequence.Play();
    }

    private void StopActivating() {
        isActivating = false;
        currentDuration = 0;

        shakeSequence.Restart();
        shakeSequence.Pause();
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
            Activate();
        }
    }

    private void ResetChecks() {
        currentDuration = 0;
        isActivated = false;
        isActivating = false;

        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        
    }
}
