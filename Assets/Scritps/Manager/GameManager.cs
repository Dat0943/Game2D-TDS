using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	STARTNG,
	PLAYING,
	PAUSED,
	GAMEOVER
}

public class GameManager : Singleton<GameManager>   
{
	public static GameState state;

	[Header("Prefabs: ")]
	[SerializeField] private Map mapPrefab;
	[SerializeField] private Player playerPrefab;
	[SerializeField] private Enemy[] enemyPrefabs;

	[Header("Spawn: ")]
	[SerializeField] private GameObject enemySpawnVfx; // Hiệu ai khi sinh ra
	[SerializeField] private float enemySpawnTime;

	[Header("Life: ")]
	[SerializeField] private int playerMaxLife;
	[SerializeField] private int playerStartingLife;
	int curLife;

	Player player;
	PlayerStats playerStats;
	Map map;

	public Player Player { get => player; private set => player = value; }
	public int CurLife 
	{ 
		get => curLife;
		set
		{
			curLife = value;
			curLife = Mathf.Clamp(curLife, 0, playerMaxLife);
		}
	}

	protected override void Awake()
	{
		MakeSingleton(false);
	}

	private void Start()
	{
		Init();
	}

	#region: Khởi tạo Game
	void Init() // Khởi tạo
	{
		state = GameState.STARTNG;
		curLife = playerStartingLife;
		SpawnMap_Player();

		GUIManager.Ins.ShowGameGUI(false);
	}

	void SpawnMap_Player()
	{
		if (mapPrefab == null || playerPrefab == null) return;

		map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
		player = Instantiate(playerPrefab, map.playerSpawnPoint.position, Quaternion.identity);
	}

	public void PlayGame()
	{
		state = GameState.PLAYING;
		playerStats = player.PlayerStats;

		SpawnEnemy();

		if (player == null || playerStats == null) return; 

		GUIManager.Ins.ShowGameGUI(true);
		GUIManager.Ins.UpdateLifeInfo(curLife);
		GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
		GUIManager.Ins.UpdateHpInfo((int)player.CurHp, (int)playerStats.hp);
		GUIManager.Ins.UpdateLevelInfo(playerStats.level, playerStats.xp, playerStats.levelUpXpRequired);
	}

	void SpawnEnemy()
	{
		var randomEnemy = GetRandomEnemy();

		if (randomEnemy == null || map == null) return;

		StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
	}

	Enemy GetRandomEnemy()
	{
		if (enemyPrefabs == null || enemyPrefabs.Length <= 0) return null;

		int randomIdx = UnityEngine.Random.Range(0, enemyPrefabs.Length);
		return enemyPrefabs[randomIdx];
	}

	IEnumerator SpawnEnemy_Coroutine(Enemy randomEnemy)
	{
		yield return new WaitForSeconds(3f);

		while(state == GameState.PLAYING)
		{
			if (map.RandomAISpawnPoint == null) break;

			Vector3 spawnPoint = map.RandomAISpawnPoint.position;
			if(enemySpawnVfx)
			{
				Instantiate(enemySpawnVfx, spawnPoint, Quaternion.identity);
			}
			yield return new WaitForSeconds(0.2f);
			Instantiate(randomEnemy, spawnPoint, Quaternion.identity);
			yield return new WaitForSeconds(enemySpawnTime);
		}
		yield return null;
	}
	#endregion

	#region: Kết thúc Game
	public void GameOverChecking(Action OnLostLife = null, Action OnDead = null)
	{
		if (curLife <= 0) return;

		curLife--;
		OnLostLife?.Invoke();

		if(curLife <= 0)
		{
			state = GameState.GAMEOVER;
			OnDead?.Invoke();
		}
	}
	#endregion

}
