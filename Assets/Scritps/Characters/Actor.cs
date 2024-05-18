using System;
using System.Collections;
using System.Collections.Generic;
using TDS;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Actor : MonoBehaviour
{
    [Header("Common: ")]
    public ActorStats statData;

    [LayerList]
    [SerializeField] private int invincibleLayer;
    [LayerList]
    [SerializeField] private int normalLayer;

    public Weapon weapon;

    protected Rigidbody2D rb;
    protected Animator anim;

	protected Coroutine stopKnockbackCo;
	protected Coroutine invincibleCo;

	protected bool isKnockback;
	protected bool isInvincible;
    bool isDead;
    float curHp;

    [Header("Events: ")]
    public UnityEvent OnInit;
	public UnityEvent OnTakeDamage;
	public UnityEvent OnDead;

	public bool IsDead { get => isDead; set => isDead = value; }
	public float CurHp 
	{
		get => curHp;
		set => curHp = value;
	}

	protected virtual void Awake() // Ghi đè được
	{
		rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
	}

	protected virtual void Start()
	{
		Init();
		OnInit?.Invoke(); // Nếu mà OnInit khác null thì chạy sự kiện
	}

	public virtual void Init()
	{
		
	}

	public virtual void TakeDamage(float damage)
	{
		if (damage < 0 || isInvincible) return; // Nếu mà không có damage đồng thời đồng bất tử

		curHp -= damage;
		Knockback();
		if(curHp <= 0)
		{
			curHp = 0;
			Die();
		}

		OnTakeDamage?.Invoke(); // Chạy sự kiện
	}

	protected virtual void Die()
	{
		isDead = true;
		rb.velocity = Vector3.zero;

		OnDead?.Invoke();

		Destroy(gameObject, 0.5f);
	}

	protected void Knockback()
	{
		if (isInvincible || isDead || isKnockback) return;

		isKnockback = true;

		stopKnockbackCo = StartCoroutine(StopKnockback());
	}

	protected void Invincible(float invincibleTime)
	{
		// Khi mà bị hất ra thì chuyển sang trạng thái bất bại
		isKnockback = false;
		isInvincible = true;

		// Đổi layer sang bất bại
		gameObject.layer = invincibleLayer;

		invincibleCo = StartCoroutine(StopInvincible(invincibleTime));
	}

	IEnumerator StopKnockback()
	{
		yield return new WaitForSeconds(statData.knockbackTime);

		Invincible(statData.invincibleTime);
	}

	IEnumerator StopInvincible(float invincibleTime)
	{
		yield return new WaitForSeconds(invincibleTime);

		isInvincible = false;

		// Đổi layer sang normal
		gameObject.layer = normalLayer;
	}

	protected virtual void Move()
	{

	}
}
