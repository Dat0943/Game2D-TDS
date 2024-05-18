using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCollectible : Collectible
{
	public override void Trigger()
	{
		GameManager.Ins.CurLife += bonus;
		GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
		AudioController.Ins.PlaySound(AudioController.Ins.lifePickup);
	}
}
