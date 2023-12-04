using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lives
{
    private static int lives = 3;

    public static int GetLives ()
    {
        return lives;
    }

    public static void LoseLife()
    {
        lives -= 1;
    }

    public static void Reset()
    {
        lives = 3;
    }
}
