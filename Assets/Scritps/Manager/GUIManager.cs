using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIManager : Singleton<GUIManager>
{
	[SerializeField] private GameObject homeGUI;
	[SerializeField] private GameObject gameGUI;

	[SerializeField] private Transform lifeGrid;
	[SerializeField] private GameObject lifeIconPrefab;

	[SerializeField] private ImageFilled levelProgressBar;
	[SerializeField] private ImageFilled hpProgressBar;

	[SerializeField] private TMP_Text levelCountingText;
	[SerializeField] private TMP_Text hpCountingText;
	[SerializeField] private TMP_Text xpCountingText;
	[SerializeField] private TMP_Text coinCountingText;
	[SerializeField] private TMP_Text reloadStateText;

	[SerializeField] private Dialog gunUpgradeDialog;
	[SerializeField] private Dialog gameoverDialog;
	Dialog activeDialog;
	public Dialog ActiveDialog { get => activeDialog; private set => activeDialog = value; }

	protected override void Awake()
	{
		MakeSingleton(false);
	}

	#region: Show Panel
	public void ShowGameGUI(bool isShow)
	{
		if (gameGUI != null)
			gameGUI.SetActive(isShow);

		if (homeGUI != null)
			homeGUI.SetActive(!isShow);
	}

	void ShowDialog(Dialog dialog)
	{
		if (dialog == null) return;

		activeDialog = dialog;
		activeDialog.Show(true);
	}

	public void ShowGunUpradeDialog()
	{
		ShowDialog(gunUpgradeDialog);
	}

	public void ShowGameoverDialog()
	{
		ShowDialog(gameoverDialog);
	}
	#endregion

	#region: LifeIcon
	public void UpdateLifeInfo(int life)
	{
		if (lifeGrid == null) return;

		ClearLifeGrid();
		DrawLifeGrid(life);
	}

	void DrawLifeGrid(int life)
	{
		if (lifeGrid == null || lifeIconPrefab == null) return;

		for(int i = 0; i < life; i++)
		{
			var lifeIconClone = Instantiate(lifeIconPrefab, Vector3.zero, Quaternion.identity);

			lifeIconClone.transform.SetParent(lifeGrid);
			lifeIconClone.transform.localPosition = Vector3.zero;
			lifeIconClone.transform.localScale = Vector3.one;
		}
	}

	void ClearLifeGrid()
	{
		if(lifeGrid == null) return;

		int lifeItemCounting = lifeGrid.childCount;
		for(int i = 0; i < lifeItemCounting; i++)
		{
			var lifeItem = lifeGrid.GetChild(i);
			if (lifeItem == null) continue;
			Destroy(lifeItem.gameObject);
		}
	}
	#endregion

	#region: UpdateText
	public void UpdateLevelInfo(int curLevel, float curXp, float levelUpXpRequired)
	{
		levelProgressBar?.UpdateValue(curXp, levelUpXpRequired);

		if(levelCountingText != null)
			levelCountingText.text = "LEVEL " + curLevel.ToString("00");

		if(xpCountingText != null)
			xpCountingText.text = curLevel.ToString("00") + " / " + levelUpXpRequired.ToString("00");
	}

	public void UpdateHpInfo(int curHp, int maxHp)
	{
		hpProgressBar?.UpdateValue(curHp, maxHp);

		if (hpCountingText != null)
			hpCountingText.text = curHp.ToString("00") + " / " + maxHp.ToString("00");
	}

	public void UpdateCoinsCounting(int coins)
	{
		if (coinCountingText != null)
			coinCountingText.text = coins.ToString("n0");
	}

	public void ShowReloadText(bool isShow)
	{
		if (reloadStateText != null)
			reloadStateText.gameObject.SetActive(isShow);
	}
	#endregion
}
