using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float GetUpgradeFomula(int level)
    {
        return (level / 2 - 0.5f) * 0.5f;
    }
}
