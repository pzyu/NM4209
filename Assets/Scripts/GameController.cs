using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private GameObject ground;

    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursor.transform.position += transform.right * Time.deltaTime * speed;
        ground.transform.position -= transform.right * Time.deltaTime * speed / 2;
    }
}
