using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    private Animator anime;
    private Playermovement playerMovement;
    private float coolDownTimer = Mathf.Infinity;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        playerMovement = GetComponent<Playermovement>();
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) && coolDownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        coolDownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anime.SetTrigger("attack");
        coolDownTimer = 0;

        int fireballIndex = FindFireBall();
        fireBalls[fireballIndex].transform.position = firePoint.position;
        fireBalls[fireballIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireBall()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
