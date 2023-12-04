using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }


    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y+1, -10);
    }
}
