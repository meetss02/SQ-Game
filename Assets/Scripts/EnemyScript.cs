using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public int life;
    public bool isCured, isVaccinated;
    SpriteRenderer sr;
    ColliderScript right;
    Vector3 playerTransform;
    bool attackPlayer, getOncePerHit = true;
    float colorTimer, anticipateTimer, attackTimer, curedTimer;
	void Start ()
    {
        life = 3;
        sr = GetComponent<SpriteRenderer>();
        right = transform.FindChild("right collider").GetComponent<ColliderScript>(); 
	}
    void Update()
    {

        if (!isCured)
        {
            if (attackTimer <= 0.5f && !attackPlayer)
                attackTimer += Time.deltaTime;
            if (anticipateTimer >= 0.75f && attackPlayer)
                attackTimer += Time.deltaTime;
            if (right.check)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                attackPlayer = false;
                anticipateTimer = 0;
            }
            if (attackPlayer)
            {
                if (anticipateTimer <= 0.75f)
                {
                    anticipateTimer += Time.deltaTime;
                }
                else if (transform.localScale.x > 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.x + 1, transform.position.y), 0.25f);
                    if (playerTransform.x + 1 == transform.position.x)
                    {
                        attackPlayer = false;
                        anticipateTimer = 0;
                        getOncePerHit = true;
                        attackTimer = 0;
                    }
                }
                else if (transform.localScale.x < 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.x - 1, transform.position.y), 0.25f);
                    if (playerTransform.x - 1 == transform.position.x)
                    {
                        attackPlayer = false;
                        anticipateTimer = 0;
                        getOncePerHit = true;
                        attackTimer = 0;
                    }
                    else if (attackTimer >= 1f)
                    {

                    }
                }
            }

            else if (attackTimer >= 0.5f)
            {
                if (transform.localScale.x > 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, 0.05f);
                }
                else if (transform.localScale.x < 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, 0.05f);
                }
            }
            //Debug.DrawRay(new Vector2(transform.position.x+transform.localScale.x/1.5f, transform.position.y + transform.localScale.y/4), Vector2.right);
            RaycastHit2D[] playerHit = null;
            if (transform.localScale.x > 0)
                playerHit = Physics2D.RaycastAll(new Vector2(transform.position.x + transform.localScale.x / 1.5f, transform.position.y + transform.localScale.y / 4), Vector2.right, 2);
            else if (transform.localScale.x < 0)
                playerHit = Physics2D.RaycastAll(new Vector2(transform.position.x - transform.localScale.x / 1.5f, transform.position.y - transform.localScale.y / 4), Vector2.left, 2);
            foreach (RaycastHit2D hit in playerHit)
            {
                if (hit.collider != null && hit.collider.tag == "Player" && getOncePerHit && attackTimer >= 0.5f)
                {
                    attackPlayer = true;
                    playerTransform = hit.transform.position;
                    getOncePerHit = false;
                    //Debug.Log(playerHit.collider.name);
                }
            }
        }
        if (life <= 0)
        {
            isCured = true;
        }
        if(isCured && !isVaccinated)
        {
            sr.color = new Color(0, 0.73f, 0);
            curedTimer += Time.deltaTime;
            if (curedTimer >= 3)
            {
                ResetInfection();
                isCured = false;
                curedTimer = 0;
            }
        }
        else if(isVaccinated)
        {
            sr.color = Color.white;
        }
        if (sr.color != new Color(0.73f, 0, 0) && !isCured)
        {
            if (colorTimer <= 0.1f)
            {
                colorTimer += Time.deltaTime;
            }
            else
            {
                sr.color = new Color(0.73f, 0, 0);
                colorTimer = 0;
            }
        }
    }
    void ResetInfection()
    {
        sr.color = new Color(0.73f, 0, 0);
        attackPlayer = false;
        getOncePerHit = true;
        isCured = false;
        isVaccinated = false;
        life = 3;
        colorTimer = 0;
        anticipateTimer = 0;
        attackTimer = 0;
        curedTimer = 0;
    }
}
