using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "TDS/Create Player Stats")]
public class PlayerStats : ActorStats
{
    [Header("Level Up Base")]
    public int level;
    public int maxLevel;
    public float xp;
    public float levelUpXpRequired; // Cần bao nhiêu để lên lv tiếp

    [Header("Level Up")]
    public float levelUpXpRequiredUp;
    public float hpUp; // Level càng cao hp càng cao

	public override bool IsMaxLevel()
	{
		return level >= maxLevel;
	}

	public override void Load()
	{
		// Lấy dữ liệu dưới máy người dùng ghi đè lên những thông số của ScriptableObject
		if(!string.IsNullOrEmpty(Prefs.playerData))
		{
			JsonUtility.FromJsonOverwrite(Prefs.playerData, this);
		}
	}

	public override void Save()
	{
		// Chuyển dữ liệu của script này sang dạng json xuống máy người dùng
		Prefs.playerData = JsonUtility.ToJson(this);
	}

	public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
	{	
		while(xp >= levelUpXpRequired && !IsMaxLevel())
		{
			level++; // Tăng level
			xp -= levelUpXpRequired;

			hp += hpUp * Helper.GetUpgradeFomula(level); // Tăng máu
			levelUpXpRequired += levelUpXpRequiredUp * Helper.GetUpgradeFomula(level);

			Save();

			OnSuccess?.Invoke();
		}

		if(xp < levelUpXpRequired || IsMaxLevel())
		{
			OnFailed?.Invoke();
		}
	}
}
