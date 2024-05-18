using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "TDS/Create Weapon Stats")]
public class WeaponStats : Stats
{
	[Header("Base Stats: ")]
	public int bullets;
	public float fireRate;
	public float reloadTime;
	public float damage;

	[Header("Upgrade: ")]
	public int level;
	public int maxLevel;
	public int bulletsUp;
	public float fireRateUp;
	public float reloadTimeUp;
	public float damageUp;
	public int upgradePrice;
	public int upgradePriceUp;

	[Header("Limit: ")]
	public float minFireRate = 0.1f;
	public float minReloadTime = 0.01f;

	public int BulletsUpInfo { get => bulletsUp * (level + 1); }
	public float FireRateUpInfo { get => fireRateUp * Helper.GetUpgradeFomula(level + 1); }
	public float ReloadTimeUpInfo { get => reloadTimeUp * Helper.GetUpgradeFomula(level + 1); }
	public float DamageUpInfo { get => damageUp * Helper.GetUpgradeFomula(level + 1); }

	public override bool IsMaxLevel()
	{
		return level >= maxLevel;
	}

	public override void Load()
	{
		// Lấy dữ liệu dưới máy người dùng ghi đè lên những thông số của ScriptableObject
		if (!string.IsNullOrEmpty(Prefs.playerWeaponData))
		{
			JsonUtility.FromJsonOverwrite(Prefs.playerWeaponData, this);
		}
	}

	public override void Save()
	{
		// Chuyển dữ liệu của script này sang dạng json xuống máy người dùng
		Prefs.playerWeaponData = JsonUtility.ToJson(this);
	}

	public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
	{
		if(Prefs.IsEnoughCoins(upgradePrice) && !IsMaxLevel())
		{
			Prefs.coins -= upgradePrice;
			level++;
			bullets += bulletsUp * level;
			fireRate -= fireRateUp * Helper.GetUpgradeFomula(level);
			fireRate = Mathf.Clamp(fireRate, minFireRate, fireRate);

			reloadTime -= reloadTimeUp * Helper.GetUpgradeFomula(level);
			reloadTime = Mathf.Clamp(reloadTime, minReloadTime, reloadTime);

			damage += damageUp * Helper.GetUpgradeFomula(level);
			upgradePrice += upgradePriceUp * level;

			Save();
			OnSuccess?.Invoke();

			return;
		}

		OnFailed?.Invoke();
	}
}
