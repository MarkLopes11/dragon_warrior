using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activetime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; //when the trap is triggered
    private bool active; //when the trap is active and can hurt the player

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ("Player"))
        {
            if(!triggered)
            {
                StartCoroutine(ActivateFireTrap());
            }
            if(active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }

    }

    private IEnumerator ActivateFireTrap()
    {
        triggered = true;
        spriteRend.color = Color.red; // to notify the player that the trap is triggering

        //wait for the delay , activate trap, turn on animation, return color to normal
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white; // to notify the player that the trap is turning off
        active = true;
        anim.SetBool("activated", true);

        // wait until X seconds, deactivate trap and reset all variables and the animator
        yield return new WaitForSeconds(activetime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);

    }
}
