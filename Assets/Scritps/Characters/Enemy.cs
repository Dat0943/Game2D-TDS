using UnityEngine;

public class Enemy : Actor
{
	Player player;
	EnemyStats enemyStats;

    float curDamage;
	float xpBonus;

	public float CurDamage { get => curDamage; private set => curDamage = value; }

	public override void Init()
	{
		player = GameManager.Ins.Player;

		if (statData == null || player == null) return;

		enemyStats = (EnemyStats)statData;
		enemyStats.Load();

		StatsCaculate();

		// Khi mà enemy chết thì sẽ gọi sự kiện ra
		OnDead.AddListener(OnSpawnCollectable);
		OnDead.AddListener(OnAddXpToPLayer);
	}

	// Tính toán lại lại dữ liệu của enemy khi sinh ra
	void StatsCaculate()
	{
		var playerStats = player.PlayerStats;

		if (playerStats == null) return;

		float hpUpgrade = enemyStats.hpUp * Helper.GetUpgradeFomula(playerStats.level + 1);
		float damageUpgrade = enemyStats.damageUp * Helper.GetUpgradeFomula(playerStats.level + 1);
		float randomXpbonus = Random.Range(enemyStats.minXpBonus, enemyStats.maxXpBonus);

		// Chỉ số của enemy sẽ tăng theo level của nhân vật + 1
		CurHp = enemyStats.hp + hpUpgrade;
		CurDamage = enemyStats.damage + damageUpgrade;
		xpBonus = randomXpbonus * Helper.GetUpgradeFomula(playerStats.level + 1);
	}

	protected override void Die()
	{
		base.Die();

		anim.SetTrigger(AnimConsts.ENEMY_DEAD_PARAM);
	}

	void OnSpawnCollectable()
	{
		CollectibleManager.Ins.Spawn(transform.position);
	}

	void OnAddXpToPLayer()
	{
		GameManager.Ins.Player.AddXp(xpBonus);
	}

	private void FixedUpdate()
	{
		Move();
	}

	protected override void Move()
	{
		if (IsDead || player == null) return;

		Vector2 playerDirection = player.transform.position - transform.position;
		playerDirection.Normalize();

		if (!isKnockback)
		{
			// Xoay ve huong player
			Flip(playerDirection);
			rb.velocity = playerDirection * enemyStats.moveSpeed * Time.deltaTime;
			return;
		}

		rb.velocity = playerDirection * -enemyStats.knockbackForce * Time.deltaTime;
	}

	void Flip(Vector2 playerDirection)
	{
		if(playerDirection.x > 0)
		{
			if (transform.localScale.x > 0) return;

			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
		else if(playerDirection.x < 0)
		{
			if (transform.localScale.x < 0) return;

			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
	}

	private void OnDisable()
	{
		OnDead?.RemoveListener(OnSpawnCollectable);
		OnDead?.RemoveListener(OnAddXpToPLayer);
	}
}
