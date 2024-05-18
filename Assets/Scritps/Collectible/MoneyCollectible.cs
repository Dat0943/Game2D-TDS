using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectible : Collectible
{
	public override void Trigger()
	{
		Prefs.coins += bonus;
		GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
		AudioController.Ins.PlaySound(AudioController.Ins.coinPickup);
	}
}
