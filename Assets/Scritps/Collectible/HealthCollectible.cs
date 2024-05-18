using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
	public override void Trigger()
	{
		if (player == null) return;

		player.CurHp += bonus;
		player.CurHp = Mathf.Clamp(player.CurHp, 0, player.PlayerStats.hp);

		GUIManager.Ins.UpdateHpInfo((int)player.CurHp, (int)player.PlayerStats.hp);
		AudioController.Ins.PlaySound(AudioController.Ins.healthPickup);
	}
}
