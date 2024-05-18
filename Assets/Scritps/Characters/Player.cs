using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
	[Header("Player Settings: ")]
	[SerializeField] private float accelerationSpeed;
	[SerializeField] private float maxMousePosDistance;
	[SerializeField] private Vector2 velocityLimit;

	[SerializeField] private float enemyDectectionRadius; // Bán kính để tìm thấy enemy gần nhất
	[SerializeField] private LayerMask enemyDetectionLayer;

	float curSpeed;
	PlayerStats playerStats;
	Actor enemyTargeted;
	Vector2 enemyTargetedDirection;

	[Header("Player Events: ")]
	public UnityEvent OnAddXp;
	public UnityEvent OnLevelUp;
	public UnityEvent OnLostLife;

	public PlayerStats PlayerStats { get => playerStats; private set => playerStats = value; }

	public override void Init()
	{
		LoadStats();
	}

	private void LoadStats()
	{
		if (statData == null) return;

		playerStats = (PlayerStats)statData;
		playerStats.Load();
		CurHp = playerStats.hp;
	}

	private void Update()
	{
		Move();
	}

	private void FixedUpdate()
	{
		DetectEnemy();
	}

	#region Tìm thấy enemy gần nhất và bắn súng
	private void DetectEnemy()
	{
		var enemyFindeds = Physics2D.OverlapCircleAll(transform.position, enemyDectectionRadius, enemyDetectionLayer);
		var finalEnemy = FindNearestEnemy(enemyFindeds);

		if (finalEnemy == null) return;

		enemyTargeted = finalEnemy;

		WeaponHandle();
	}

	void WeaponHandle()
	{
		if (enemyTargeted == null || weapon == null) return;

		enemyTargetedDirection = enemyTargeted.transform.position - weapon.transform.position;
		enemyTargetedDirection.Normalize();

		// Xoay khẩu súng theo hướng
		float angle = Mathf.Atan2(enemyTargetedDirection.y, enemyTargetedDirection.x) * Mathf.Rad2Deg; // Chuyển từ radian sang độ
		weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

		if (isKnockback) return;

		weapon.Shoot(enemyTargetedDirection);
	}

	Actor FindNearestEnemy(Collider2D[] enemyFindeds)
	{
		float minDistance = 0;
		Actor finalEnemy = null;

		if (enemyFindeds == null || enemyFindeds.Length <= 0) return null;

		for (int i = 0; i < enemyFindeds.Length; i++)
		{
			var enemyFinded = enemyFindeds[i];
			if (enemyFinded == null) continue;
			if (finalEnemy == null)
			{
				minDistance = Vector2.Distance(transform.position, enemyFinded.transform.position);
			}
			else
			{
				float distanceTemp = Vector2.Distance(transform.position, enemyFinded.transform.position);
				if (distanceTemp > minDistance) continue;
				minDistance = distanceTemp;
			}
			finalEnemy = enemyFinded.GetComponent<Actor>();
		}

		return finalEnemy;
	}
	#endregion

	#region Di chuyển Player bằng cách nhấn chuột
	protected override void Move()
	{
		if (IsDead) return;

		// Chuyển vị trí con trỏ chuột vào trong unity
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 movingDirection = mousePos - (Vector2)transform.position;
		movingDirection.Normalize();

		if (!isKnockback)
		{
			if (Input.GetMouseButton(0))
			{
				Run(mousePos, movingDirection);
			}
			else
			{
				BackToIdle();
			}

			return;
		}

		rb.velocity = enemyTargetedDirection * -statData.knockbackForce * Time.deltaTime;
		anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
	}

	void BackToIdle()
	{
		curSpeed -= accelerationSpeed * Time.deltaTime; // Giảm tốc độ hiên tại xuống
		curSpeed = Mathf.Clamp(curSpeed, 0, curSpeed);

		rb.velocity = Vector2.zero;

		anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
	}

	void Run(Vector2 mousePos, Vector2 movingDirection)
	{
		curSpeed += accelerationSpeed * Time.deltaTime;
		curSpeed = Mathf.Clamp(curSpeed, 0, playerStats.moveSpeed);

		float delta = curSpeed * Time.deltaTime;
		float distanceToMousePos = Vector2.Distance(transform.position, mousePos);
		distanceToMousePos = Mathf.Clamp(distanceToMousePos, 0, maxMousePosDistance / 3);

		delta *= distanceToMousePos;

		rb.velocity = movingDirection * delta;
		float velocityLimitX = Mathf.Clamp(rb.velocity.x, -velocityLimit.x, velocityLimit.x);
		float velocityLimitY = Mathf.Clamp(rb.velocity.y, -velocityLimit.y, velocityLimit.y);

		rb.velocity = new Vector2(velocityLimitX, velocityLimitY);

		anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, true);
	}
	#endregion

	#region Player nhận xp
	public void AddXp(float xpBonus)
	{
		if (playerStats == null) return;

		playerStats.xp += xpBonus;
		playerStats.Upgrade(OnUpgradeStats); // Khi levelup lên thì sẽ upgrade bằng cách sử dụng sự kiện

		OnAddXp?.Invoke();
		playerStats.Save();
	}	

	void OnUpgradeStats()
	{
		OnLevelUp?.Invoke();
	}
	#endregion

	#region Player bị gây damage
	public override void TakeDamage(float damage)
	{
		if (isInvincible) return;

		Debug.Log("Va cham");

		CurHp -= damage;
		CurHp = Mathf.Clamp(CurHp, 0, PlayerStats.hp);
		Knockback();

		OnTakeDamage?.Invoke();

		if (CurHp > 0) return;

		// Giam so mang cua player dang co
		GameManager.Ins.GameOverChecking(OnLostLifeDelegate, OnDeadDelegate);
	}

	void OnLostLifeDelegate()
	{
		CurHp = playerStats.hp;

		if (stopKnockbackCo != null)
			StopCoroutine(stopKnockbackCo);

		if (invincibleCo != null)
			StopCoroutine(invincibleCo);

		OnLostLife?.Invoke();
	}

	void OnDeadDelegate()
	{
		CurHp = 0;
		Die();
	}
	#endregion

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
		{
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();

			if(enemy != null)
			{
				TakeDamage(enemy.CurDamage);
			}
		}
		else if(collision.gameObject.CompareTag(TagConsts.COLLECTIBLE_TAG))
		{
			Collectible collectible = collision.gameObject.GetComponent<Collectible>();
			collectible?.Trigger(); // Bắt va chạm của player ở trong player, giảm bớt hiệu năng đi
			Destroy(collision.gameObject);
		}
	}
	
}
