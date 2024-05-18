using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("Projectile info: ")]
    [SerializeField] private float speed;
    float damage;
    float curSpeed;

	[Header("HitPS: ")]
	[SerializeField] private GameObject bodyHitPrefab;

	Vector2 lastPosition;
	RaycastHit2D raycastHit;

	public float Damage { get => damage; set => damage = value; }

	private void Start()
	{
		curSpeed = speed;
		RefreshLastPos();
	}

	private void Update()
	{
		transform.Translate(transform.right * curSpeed * Time.deltaTime, Space.World);

		DealDamage();
		RefreshLastPos();
	}

	void DealDamage()
	{
		Vector2 rayDirection = (Vector2)transform.position - lastPosition;
		raycastHit = Physics2D.Raycast(lastPosition, rayDirection, rayDirection.magnitude);
		var collider = raycastHit.collider;

		if (!raycastHit || collider == null) return;

		// Khi raycast chạm phải enemy
		if(collider.CompareTag(TagConsts.ENEMY_TAG))
		{
			DealDamageToEnemy(collider);
		}
	}

	void DealDamageToEnemy(Collider2D collider)
	{
		Actor actorComp = collider.GetComponent<Actor>();
		
		// Nếu mà nhân vật khác rỗng thì gây sát thương
		actorComp?.TakeDamage(damage);

		if(bodyHitPrefab)
		{
			// raycastHit.point: Vị trí va chạm của raycast với mọi thứ
			Instantiate(bodyHitPrefab, (Vector3)raycastHit.point, Quaternion.identity);
		}

		Destroy(gameObject);
	}

	void RefreshLastPos()
	{
		lastPosition = (Vector2)transform.position;
	}

	private void OnDisable() // Khi viên đạn bị disable
	{
		// reset lại raycastHit
		raycastHit = new RaycastHit2D();
	}
}
