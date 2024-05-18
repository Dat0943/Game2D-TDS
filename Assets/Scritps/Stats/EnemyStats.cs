using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "TDS/Create Enemy Stats")]
public class EnemyStats : ActorStats
{
    [Header("Xp Bonus: ")]
    public float minXpBonus;
	public float maxXpBonus;

    [Header("Level Up: ")]
    public float hpUp;
    public float damageUp;
}
