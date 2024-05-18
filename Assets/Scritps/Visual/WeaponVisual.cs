using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    Weapon weapon;

	private void Awake()
	{
		weapon = GetComponent<Weapon>();
	}

	public void OnShoot()
	{
		AudioController.Ins.PlaySound(AudioController.Ins.shooting);

		CineController.Ins.ShakeTrigger();
	}

	public void OnReload()
	{
		GUIManager.Ins.ShowReloadText(true);
	}

	public void OnReloadDone()
	{
		AudioController.Ins.PlaySound(AudioController.Ins.reload);

		GUIManager.Ins.ShowReloadText(false);
	}
}
