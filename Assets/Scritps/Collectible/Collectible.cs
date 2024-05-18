using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int minBonus;
    [SerializeField] private int maxBonus;
    [SerializeField] private int lifeTime; // Thời gian tồn tại
    [SerializeField] private int spawnForce; // Khi sinh ra bắn ra các hướng

    int lifeTimeCounting;
    protected int bonus;

    protected Player player;
    Rigidbody2D rb;
    FlashVfx flashvfx;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();  
        flashvfx = GetComponent<FlashVfx>();
    }

    private void Start()
    {
        player = GameManager.Ins.Player; // Lấy dữ liệu của Player bên GameManager
        lifeTimeCounting = lifeTime;
        bonus = Random.Range(minBonus, maxBonus) * player.PlayerStats.level; // Bonus tăng theo level của người chơi

        Init();
        Explode();
        FlashVfxCompleted();
        StartCoroutine(DisappearFlashNoticeCountDown());
    }

    // Báo hiệu cho người chơi Collectible sắp hết rồi
	IEnumerator DisappearFlashNoticeCountDown()
	{
        while(lifeTimeCounting > 0)
        {
			// Thời gian tồn tại còn lại của những collectible
			float timeLifeLeftRate = Mathf.Round((float)lifeTimeCounting / lifeTime);
			yield return new WaitForSeconds(1f);
            lifeTimeCounting--;
            if(timeLifeLeftRate <= 0.3f && flashvfx != null)
            {
                flashvfx.Flash(lifeTimeCounting);
            }    
        }
	}

	void FlashVfxCompleted()
	{
        if (flashvfx == null) return;

        // Khi hoàn thành việc Flash thì sẽ xóa hết các sự kiện
        flashvfx.OnCompleted.RemoveAllListeners();
        flashvfx.OnCompleted.AddListener(OnDestroyCollectible);
	}

    void OnDestroyCollectible()
    {
        Destroy(gameObject);
    }    

    // Collectible sẽ rơi ra ở các hướng ngẫu nhiên
	void Explode()
	{
        if (rb == null) return;

        float randomForceX = Random.Range(-spawnForce, spawnForce);
		float randomForceY = Random.Range(-spawnForce, spawnForce);

        rb.velocity = new Vector2(randomForceX, randomForceY) * Time.deltaTime;
        StartCoroutine(StopMoving()); // Sau 1 khoảng thời gian sẽ dừng di chuyển lại
	}

	IEnumerator StopMoving()
	{
        yield return new WaitForSeconds(0.8f);
        if(rb != null)
        {
            rb.velocity = Vector2.zero;
        }
	}

	public virtual void Init()
	{
		
	}

	public virtual void Trigger()
    {

    }
}
