 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    [SerializeField]
    private Sprite[] livesSprites;
    [SerializeField]
    private Image livesImage;
    public void updateLives(int currentLives)
    {
        livesImage.sprite = livesSprites[currentLives];
    }
}
