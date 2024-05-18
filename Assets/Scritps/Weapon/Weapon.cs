using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Common: ")]
    public WeaponStats statData;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;

    float curFireRate;
    int curBullets;
    float curReloadTime;

    bool isShooted;
    bool isReloading;

    [Header("Events: ")]
    public UnityEvent OnShoot;
    public UnityEvent OnReload;
    public UnityEvent OnReloadDone;

	private void Start()
	{
		LoadStats();
	}

    // Lấy dữ liệu lên
	void LoadStats()
	{
        if (statData == null) return;

        statData.Load(); // Lấy dữ liệu dưới máy người dùng
        curFireRate = statData.fireRate;
        curBullets = statData.bullets;
        curReloadTime = statData.reloadTime;
	}

	private void Update()
	{
        ReduceFireRate();
        ReduceReloadTime();
	}

	void ReduceReloadTime()
	{
        if (!isReloading) return;
        curReloadTime -= Time.deltaTime;

        if (curReloadTime > 0) return;

        LoadStats();

        isReloading = false;
        OnReloadDone?.Invoke();
	}

    // Giảm thời gian sau mỗi lần bắn
	void ReduceFireRate()
	{
        if (!isShooted) return; // Nếu súng chưa bắn
        curFireRate -= Time.deltaTime;

        if (curFireRate > 0) return;
        curFireRate = statData.fireRate;
        isShooted = false;
	}

    public void Shoot(Vector3 targetDirection)
    {
        if (isShooted || shootingPoint == null || curBullets <= 0) return;

        if(muzzleFlashPrefab)
        {
            var muzzleFlashClone = Instantiate(muzzleFlashPrefab, shootingPoint.position, transform.rotation);
            muzzleFlashClone.transform.SetParent(shootingPoint); // Sinh ra ở trong Object shootingPoint
        }

        if(bulletPrefab)
        {
            var bulletClone = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            var projectileComp = bulletClone.GetComponent<Projectile>();

            if(projectileComp != null)
            {
                projectileComp.Damage = statData.damage; // Load lại damage của súng
            }
        }

        curBullets--;
        isShooted = true;   
        if(curBullets <= 0)
        {
            Reload(); // Nếu hết đạn thì nạp đạn
        }

        OnShoot?.Invoke();
    }

    public void Reload()
    {
        isReloading = true;

        OnReload?.Invoke();
    }
}
