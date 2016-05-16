using UnityEngine;
using System.Collections;

public class InfectedScript : MonoBehaviour
{
    public int life;
    SpriteRenderer sr;
    ColliderScript right;
    Vector3 playerTransform;
    //bool attackPlayer, getOncePerHit = true;
    float colorTimer, anticipateTimer, attackTimer;
    void Start()
    {
        life = 3;
        sr = GetComponent<SpriteRenderer>();
        right = transform.FindChild("right collider").GetComponent<ColliderScript>();
    }
    void Update()
    {
        Movement();
        RaycastHit2D[] playerHit = null;
        if (transform.localScale.x > 0)
            playerHit = Physics2D.RaycastAll(new Vector2(transform.position.x + transform.localScale.x / 1.5f, transform.position.y + transform.localScale.y / 4), Vector2.right, 2);
        else if (transform.localScale.x < 0)
            playerHit = Physics2D.RaycastAll(new Vector2(transform.position.x - transform.localScale.x / 1.5f, transform.position.y - transform.localScale.y / 4), Vector2.left, 2);
        foreach (RaycastHit2D hit in playerHit)
        {
            
        }
        if (life <= 0)
        {
            Destroy(gameObject);
        }
        if (sr.color != Color.white)
        {
            if (colorTimer <= 0.1f)
            {
                colorTimer += Time.deltaTime;
            }
            else
            {
                sr.color = Color.white;
                colorTimer = 0;
            }
        }
    }
    void Movement()
    {
        if (right.check)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            anticipateTimer = 0;
        }
        if (transform.localScale.x > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, 0.05f);
        }
        else if (transform.localScale.x < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, 0.05f);
        }
    }
}
