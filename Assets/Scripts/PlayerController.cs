using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region player_components
    Rigidbody2D playerRB;
    #endregion
    // Start is called before the first frame update

    #region animation_components
    Animator anim;
    #endregion





    #region attackVariables
    public float damage;
    public float attackSpeed;
    float attackTimer;
    public float hitboxTiming;
    public float endAnimationTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region helth_variables
    public float maxHealth;
    float currHealth;
    public Slider hpSlider;
    #endregion



    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        hpSlider.value = currHealth / maxHealth;
 

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        }
        else attackTimer -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.L)){
            Interact();

        }

    }

    private void Move() {

        anim.SetBool("Moving", true);
        if (x_input > 0) {
            playerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        }
        else if (x_input < 0) {
            playerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }

        else if (y_input > 0) {
            playerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
        else if (y_input < 0) {
            playerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else
        {
            playerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        anim.SetFloat("Dirx", currDirection.x);
        anim.SetFloat("Diry", currDirection.y);
    }

    #region attack_function
    private void Attack()
    {
        print("ij");

        StartCoroutine(AttackRoutine());
        attackTimer = attackSpeed;

    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        playerRB.velocity = Vector2.zero;
        anim.SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("PlayerAttack");
        yield return new WaitForSeconds(hitboxTiming);

        Debug.Log("cast me now senpai");
   
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, Vector2.one, 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
                print("damagge");
            }

        }

        yield return new WaitForSeconds(endAnimationTiming);
        isAttacking = false;
      
    }
    #endregion

    #region helth_functions
    public void TakeDamage(float value)
    {
        currHealth = currHealth - value;
        FindObjectOfType<AudioManager>().Play("PlayerHurt");
        print(currHealth);
        hpSlider.value = currHealth / maxHealth;


        if (currHealth <= 0)
        {
            Die();
        }

    }
    public void Heal(float value)

    {
        currHealth = currHealth + value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        hpSlider.value = currHealth / maxHealth;
        print(currHealth);

    }
    private void Die()
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LossGame();
    }
    #endregion

    #region interact_functions
    void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                print("benis");
                hit.transform.GetComponent<Chest>().Interact();
            }

        }
    }
    #endregion
}
