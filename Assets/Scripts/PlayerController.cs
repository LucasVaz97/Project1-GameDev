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
    SpriteRenderer playerSprite;
    #endregion
    // Start is called before the first frame update

    #region animation_components
    Animator anim;
    #endregion


    public GameObject arrow;
    public Transform firepoint;
    public float arrowforce = 20f;
    public int numberOffArrows;
    public Text arrowText;

    public bool isDashing;
    public float dashTime=0.1f;





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
    public Slider StaminaSlider;
    #endregion

    public float maxStamina;
    float currStamina;



    void Awake()
    {

        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        currHealth = maxHealth/2;
        currStamina = maxStamina / 2;
        numberOffArrows = 10;
        hpSlider.value = currHealth / maxHealth;
        StaminaSlider.value = currStamina / maxStamina;
        arrowText.text = "Arrows:" + numberOffArrows.ToString();
        InvokeRepeating("RegenStamina", 0.05f, 0.05f);


    }

    // Update is called once per frame
    void Update()
    {
        Dash();


        if (isAttacking | isDashing)
        {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();
        ShootArrow();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0 && isDashing==false)
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
            firepoint.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (x_input < 0) {
            playerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
            firepoint.transform.eulerAngles = new Vector3(0, 0, 90);
        }

        else if (y_input > 0) {
            playerRB.velocity = Vector2.up * movespeed;
            firepoint.transform.eulerAngles = new Vector3(0, 0, 0);
            currDirection = Vector2.up;

        }
        else if (y_input < 0) {
            playerRB.velocity = Vector2.down * movespeed;
            firepoint.transform.eulerAngles = new Vector3(0, 0, 180);
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



   
        Collider2D[] hits = Physics2D.OverlapBoxAll(playerRB.position + currDirection,Vector2.one,0f);
        foreach (Collider2D hit in hits)
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

    public void ShootArrow()
    {
  
        if (Input.GetKeyDown(KeyCode.K) && numberOffArrows>0)
        {

            GameObject arrowP = Instantiate(arrow, playerRB.position + currDirection, firepoint.rotation);
            Rigidbody2D rb = arrowP.GetComponent<Rigidbody2D>();
            rb.AddForce(currDirection * arrowforce, ForceMode2D.Impulse);
            numberOffArrows = numberOffArrows - 1;
            arrowText.text = "Arrows:" + numberOffArrows.ToString();
    

        }

    }

    public void pickArrow()
    {
        numberOffArrows = numberOffArrows + 5;
        arrowText.text = "Arrows:" + numberOffArrows.ToString();
    }


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


    }
    public void RegenStamina()
    {
        if (currStamina < 3)
        {
            StaminaSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            StaminaSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;

        }
        currStamina = currStamina + 0.05f;
        currStamina = Mathf.Min(currStamina, maxStamina);
        StaminaSlider.value = currStamina / maxStamina;
      
    }
    private void Die()
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LossGame();
    }
    #endregion

    IEnumerator DoubleDamageRoutine()
    {
        damage = damage * 2;
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(5f);
        damage = damage / 2;
        playerSprite.color = Color.white;


    }

    public void DoubleDamage()
    {
        StartCoroutine(DoubleDamageRoutine());

    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currStamina>3)
        {
            isDashing = true;
            currStamina = currStamina - 3;

            playerRB.velocity = currDirection * 40;
        }
        if (isDashing)
        {
            dashTime = dashTime - Time.deltaTime;

        }
        if (dashTime <= 0)
        {
       
            isDashing = false;
            dashTime = 0.06f;
        }
    }


    #region interact_functions
    void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
           
                hit.transform.GetComponent<Chest>().Interact();
            }

        }
    }
    #endregion
}
