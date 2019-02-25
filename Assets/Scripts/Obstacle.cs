using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Transform trigger;

    [SerializeField]
    private Transform spriteAnchor;
    
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private SpriteRenderer warning;

    [SerializeField]
    private float minDistance = 1.5f;

    [SerializeField]
    private float maxDistance = 3.0f;

    private float distance;

    // Start is called before the first frame update
    void Awake()
    {
        InitObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitObstacle() {
        distance = Random.Range(minDistance, maxDistance);

        trigger.localPosition = new Vector3(-distance * 4, 0, transform.localPosition.z);

        sprite.transform.localPosition = new Vector3(distance, distance, transform.localPosition.z);

        spriteAnchor.transform.localEulerAngles = new Vector3(0, 0, -135);

        sprite.GetComponent<SpriteRenderer>().DOFade(1.0f, 0.1f);

        warning.DOFade(0.0f, 0.001f);
    }

    public void TriggerObstacle() {
        spriteAnchor.DOLocalRotate(new Vector3(0, 0, 360), distance * 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(InitObstacle);
    }
}
