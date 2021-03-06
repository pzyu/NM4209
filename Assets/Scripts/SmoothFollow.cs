﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField]
    private bool isFollowingYAxis;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private bool randomizeSmoothTime = false;
    [SerializeField]
    private float randomizeInterval = 1.0f;

    [SerializeField]
    private bool isSmooth = true;

    // Start is called before the first frame update
    void Start() {
        if (randomizeSmoothTime) {
            StartCoroutine(RandomizeSmoothTime());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
        Vector2 targetPosition = new Vector2(target.position.x, (isFollowingYAxis ? target.position.y : transform.position.y));

        if (offset != Vector2.zero) {
            targetPosition += offset;
        }


        if (isSmooth) {
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        } else {
            transform.position = targetPosition;
        }
    }

    private IEnumerator RandomizeSmoothTime() {
        yield return new WaitForSeconds(Random.Range(2, 5));
        smoothTime = Random.Range(0.2f, 0.7f);
        StartCoroutine(RandomizeSmoothTime());
    }
}
