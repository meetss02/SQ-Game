using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    int type;
    [HideInInspector] public bool isRightDirection = true;
    EnemyScript enemy;
	void Start ()
    {
	    
	}
	void Update ()
    {
	    if(isRightDirection)
            transform.position = new Vector2(transform.position.x + Time.deltaTime * 25, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - Time.deltaTime * 25, transform.position.y);

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Ground" || col.tag == "Enemy" || col.tag == "Border")
        {
            if(col.tag =="Enemy")
            {
                enemy = col.GetComponent<EnemyScript>();
                col.GetComponent<SpriteRenderer>().color = new Color(0, 0.73f, 0);
                enemy.life--;
            }
            gameObject.SetActive(false);
        }
            
    }
}
