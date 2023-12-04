using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lives
{
    private static int lives = 3;
    private static float lasthit = -10f;

    public static int GetLives ()
    {
        return lives;
    }

    public static bool LoseLife()
    {
        if(Time.time > lasthit + 4f)
        {
            lives -= 1;
            lasthit = Time.time;
            return true;
        }
        else
            return false;
    }

    public static void Reset()
    {
        lives = 3;
    }
}
