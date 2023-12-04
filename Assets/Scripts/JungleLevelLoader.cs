using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JungleLevelLoader : MonoBehaviour
{
    public AudioController audioController;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            audioController.playLevelComplete();
            SceneManager.LoadScene("JungleScene");
        }
    }
}
