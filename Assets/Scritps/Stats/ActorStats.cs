using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStats : Stats
{
	[Header("Base Stats: ")]
	public float hp;
	public float damage;
	public float moveSpeed;
	public float knockbackForce;
	public float knockbackTime;
	public float invincibleTime;

	public override bool IsMaxLevel()
	{
		return false;
	}

	public override void Load()
	{
		
	}

	public override void Save()
	{
		
	}

	public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
	{
		
	}
}
