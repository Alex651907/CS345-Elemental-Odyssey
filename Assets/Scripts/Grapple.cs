using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint2D;
    public PlayerController playerController;
    public CanGrapple grappaplable;
    // Start is called before the first frame update
    void Start()
    {
        distanceJoint2D.enabled = false;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && CanGrapple.status == true)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, mousePosition);
            lineRenderer.SetPosition(1, transform.position);
            distanceJoint2D.connectedAnchor = mousePosition;
            distanceJoint2D.enabled = true;
            lineRenderer.enabled = true;
            playerController.enabled = false;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
            playerController.enabled = true;
        }

        if(distanceJoint2D.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
