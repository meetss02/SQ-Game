using UnityEngine;
using System.Collections;

public class ColliderScript : MonoBehaviour
{
    /*[HideInInspector]*/ public bool check;
    void OnTriggerStay2D(Collider2D col)
    {
        if (((col.tag == "Ground" || col.tag == "Enemy" || col.tag == "Border") && transform.parent.tag == "Player") ||
        ((col.tag == "Ground" || col.tag == "Enemy") && transform.parent.tag == "Enemy"))
            check = true;
        else if (transform.parent.tag == "MainCamera")
        {
            if (col.tag == "Border")
            {
                if ((name.StartsWith("down") && col.name.StartsWith("down")) || ((name.StartsWith("left") && col.name.StartsWith("left")))|| 
                   ((name.StartsWith("right") && col.name.StartsWith("right"))) || ((name.StartsWith("up") && col.name.StartsWith("up"))))
                {
                    check = true;
                }
            }
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (((col.tag == "Ground" || col.tag == "Enemy" || col.tag == "Border") && transform.parent.tag == "Player") ||
        ((col.tag == "Ground" || col.tag == "Enemy") && transform.parent.tag == "Enemy"))
            check = false;
        else if (transform.parent.tag == "MainCamera")
        {
            if (col.tag == "Border")
            {
                if ((name.StartsWith("down") && col.name.StartsWith("down")) || ((name.StartsWith("left") && col.name.StartsWith("left")))|| 
                   ((name.StartsWith("right") && col.name.StartsWith("right"))) || ((name.StartsWith("up") && col.name.StartsWith("up"))))
                check = false;
            }
        }
    }
}
