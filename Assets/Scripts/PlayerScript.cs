using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    public float runSpeed;
    [HideInInspector] public bool jumpCheck;
    ColliderScript down, right;
    public List<GameObject> bulletList, usedBulletList;
    Animator playerAnim;
    bool canJump, isMelee = false, isMovementEnabled = true, cantMove, isgrounded;
    float x, xScale, jumpTimer, shootTimer, infectionLevel;
	void Start ()
    {
        usedBulletList = new List<GameObject>();
        bulletList = new List<GameObject>();
        playerAnim = transform.FindChild("Player").GetComponent<Animator>();
        down = transform.FindChild("down collider").GetComponent<ColliderScript>();
        right = transform.FindChild("right collider").GetComponent<ColliderScript>();
        x = transform.position.x;
        xScale = transform.localScale.x;
    }
	
	void Update ()
    {
        if(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Shoot") ||
           playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Syringe Attack")||
           playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            cantMove = true;
        }
        else if(cantMove)
        {
            cantMove = false;
            isMovementEnabled = true;
        }
        if(isMovementEnabled)
            Movement();

        RaycastHit2D[] hit = null;
        if(transform.localScale.x < 0)
            hit = Physics2D.BoxCastAll(transform.position, new Vector2(0.5f,0.5f), 0, Vector2.left, 0.5f);
        else if(transform.localScale.x > 0)
            hit = Physics2D.BoxCastAll(transform.position, new Vector2(0.5f, 0.5f), 0, Vector2.right, 0.5f);

        if(Input.GetKeyDown(KeyCode.E))
        {
            isMelee = !isMelee;
        }
        if (isMelee)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isMovementEnabled = false;
                playerAnim.Play("Syringe Attack");
                foreach (RaycastHit2D h in hit)
                {
                    if (h.collider.tag == "Enemy")
                    {
                        if(h.collider.GetComponent<EnemyScript>().isCured)
                            h.collider.GetComponent<EnemyScript>().isVaccinated = true;
                    }
                }
            }
        }
        else
        {
            if(bulletList.Count > 0)
            {
                for(int i = 0; i < bulletList.Count; i++)
                {
                    if(!bulletList[i].activeSelf)
                    {
                        usedBulletList.Add(bulletList[i]);
                        bulletList.Remove(bulletList[i]);
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                isMovementEnabled = false;
                playerAnim.Play("Shoot");
                if (usedBulletList.Count == 0)
                    Invoke("MakeBullet", 0.2f);
                else
                    Invoke("CullBullet", 0.2f);

            }
        }
        
    }
    void MakeBullet()
    {
        GameObject g = Instantiate(Resources.Load("Bullet"), transform.FindChild("Gun").GetChild(0).position, Quaternion.identity) as GameObject;
        g.name = "Bullet";
        if (transform.localScale.x > 0)
            g.GetComponent<BulletScript>().isRightDirection = true;
        else if (transform.localScale.x < 0)
            g.GetComponent<BulletScript>().isRightDirection = false;
        bulletList.Add(g);
    }
    void CullBullet()
    {
        GameObject g = usedBulletList[0];
        g.transform.position = transform.FindChild("Gun").GetChild(0).position;
        if (transform.localScale.x > 0)
            g.GetComponent<BulletScript>().isRightDirection = true;
        else if (transform.localScale.x < 0)
            g.GetComponent<BulletScript>().isRightDirection = false;
        bulletList.Add(usedBulletList[0]);
        usedBulletList.Remove(usedBulletList[0]);
        g.SetActive(true);
    }
    void Movement()
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        if (/*Input.GetAxisRaw("Horizontal") < 0*/ Input.GetKey(KeyCode.A))
        {
            //playerAnim.SetBool("Moving", true);
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                playerAnim.Play("Run");
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
            if (!right.check)
            {
                x += -1 * Time.deltaTime * runSpeed;
            }
        }
        else if (/*Input.GetAxisRaw("Horizontal") > 0*/ Input.GetKey(KeyCode.D))
        {
            //playerAnim.SetBool("Moving", true);
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                playerAnim.Play("Run");
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            if (!right.check)
            {
                x += 1 * Time.deltaTime * runSpeed;
            }
        }
        else if(!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall") &&
                !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            playerAnim.Play("Idle");
            //playerAnim.SetBool("Moving", false);
        }
        if (down.check)
        {
            if(!isgrounded)
            {
                isgrounded = true;
                playerAnim.Play("Grounded");
                //isMovementEnabled = false;
            }
            jumpCheck = false;
            if (jumpTimer >= 0.5f)
            {
                canJump = true;
                jumpTimer = 0;
            }
            else
                jumpTimer += Time.deltaTime;
        }
        else
        {
            isgrounded = false;
            playerAnim.Play("Fall");
        }
        if (canJump && (Input.GetAxisRaw("Vertical") > 0 || Input.GetButtonDown("Jump")))
        {
            playerAnim.Play("Jump");
            canJump = false;
            jumpCheck = true;
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
    }
}
