using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : ActorVisual
{
    [SerializeField] private GameObject deathVfxPrefab;

	Player player;
	PlayerStats playerStats;

	private void Start()
	{
		player = (Player)actor;
		playerStats = player.PlayerStats;
	}

	public override void OnTakeDamage()
	{
		base.OnTakeDamage();

		GUIManager.Ins.UpdateHpInfo((int)actor.CurHp, (int)actor.statData.hp);
	}

	// Khi bị mất một mạng
	public void OnLostLife()
	{
		if (player == null || playerStats == null) return;

		AudioController.Ins.PlaySound(AudioController.Ins.lostLife);

		GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);

		GUIManager.Ins.UpdateHpInfo((int)player.CurHp, (int)playerStats.hp);
	}

	public void OnDead()
	{
		if (deathVfxPrefab)
			Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);

		AudioController.Ins.PlaySound(AudioController.Ins.playerDeath);

		GUIManager.Ins.ShowGameoverDialog();
	}

	public void OnAddXp()
	{
		if (playerStats == null) return;

		GUIManager.Ins.UpdateLevelInfo(playerStats.level, playerStats.xp, playerStats.levelUpXpRequired);
	}

	public void OnLevelUp()
	{
		AudioController.Ins.PlaySound(AudioController.Ins.levelUp);
	}
}
