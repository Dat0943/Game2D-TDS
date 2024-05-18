using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunUpgradeDialog : Dialog
{
	[SerializeField] private GunStatUI bulletStatUI;
	[SerializeField] private GunStatUI damageStatUI;
	[SerializeField] private GunStatUI firerateStatUI;
	[SerializeField] private GunStatUI reloadStatUI;
	[SerializeField] private TMP_Text upgradeBtnText;

	Weapon weapon;
	WeaponStats weaponStats;

	public override void Show(bool isShow)
	{
		base.Show(isShow);
		Time.timeScale = 0f;

		weapon = GameManager.Ins.Player.weapon;
		weaponStats = weapon.statData;

		UpdateUI();
	}

	void UpdateUI()
	{
		if (weapon == null || weaponStats == null) return;

		if (titleText)
			titleText.text = "LEVEL " + weaponStats.level.ToString("00");

		if (upgradeBtnText)
			upgradeBtnText.text = "UP [" + weaponStats.upgradePrice.ToString("n0") + " $]";

		if(bulletStatUI)
		{
			bulletStatUI.UpdateStat
				("Bullets : ",
				weaponStats.bullets.ToString("n0"),
				" ( +" + weaponStats.BulletsUpInfo.ToString("n0") + " )"
				);
		}

		if (damageStatUI)
		{
			damageStatUI.UpdateStat
				("Damage : ",
				weaponStats.damage.ToString("F2"),
				" ( +" + weaponStats.DamageUpInfo.ToString("F3") + " )"
				);
		}

		if (firerateStatUI)
		{
			firerateStatUI.UpdateStat
				("Firerate : ",
				weaponStats.fireRate.ToString("F2"),
				" ( +" + weaponStats.FireRateUpInfo.ToString("F3") + " )"
				);
		}

		if (reloadStatUI)
		{
			reloadStatUI.UpdateStat
				("Reload : ",
				weaponStats.reloadTime.ToString("F2"),
				" ( +" + weaponStats.ReloadTimeUpInfo.ToString("F3") + " )"
				);
		}
	}

	public void UpradeGun()
	{
		if (weaponStats == null) return;

		weaponStats.Upgrade(OnUpgradeSuccess, OnUpradeFailed);
	}

	void OnUpgradeSuccess()
	{
		UpdateUI();

		GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);

		AudioController.Ins.PlaySound(AudioController.Ins.upgradeSuccess);
	}

	void OnUpradeFailed()
	{
		


	}

	public override void Close()
	{
		base.Close();
		Time.timeScale = 1f;
	}
}
