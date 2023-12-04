using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGrapple : MonoBehaviour
{
    public static bool status = false;
    void OnMouseOver()
    {
        status = true;
    }

    void OnMouseExit()
    {
        status = false;
    }

}
