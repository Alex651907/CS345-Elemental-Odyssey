using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public AudioController audioController;
    public Sprite flagDown;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<SpriteRenderer>().sprite = flagDown;
            audioController.playLevelComplete();
            StartCoroutine(wait(3.0f));
            SceneManager.LoadScene("VictoryScreen");
        }
    }
    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
