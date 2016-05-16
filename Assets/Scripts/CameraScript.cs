using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    PlayerScript player;
    ColliderScript up, down, left, right;
    Transform playerTrans, upTrans, downTrans, leftTrans, rightTrans;
    float tempY, x, y;
    bool getOnce = true, getScreenCollidersOnce = true;
    void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTrans.GetComponent<PlayerScript>();
        upTrans = transform.FindChild("up collider");
        downTrans = transform.FindChild("down collider");
        leftTrans = transform.FindChild("left collider");
        rightTrans = transform.FindChild("right collider");
        up = upTrans.GetComponent<ColliderScript>();
        down = downTrans.GetComponent<ColliderScript>();
        left = leftTrans.GetComponent<ColliderScript>();    
        right = rightTrans.GetComponent<ColliderScript>();
        x = playerTrans.position.x;
        y = playerTrans.position.y;
    }

    void Update()
    {
        if (getScreenCollidersOnce)
        {
            getScreenCollidersOnce = false;
            leftTrans.localScale = Camera.main.WorldToViewportPoint(new Vector3(6, Screen.height / 3.85f, 0));
            leftTrans.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)) + new Vector3(transform.localScale.y / 2, 0, 0);
            rightTrans.localScale = Camera.main.WorldToViewportPoint(new Vector3(6, Screen.height / 3.85f, 0));
            rightTrans.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)) - new Vector3(transform.localScale.y / 2, 0, 0);
            downTrans.localScale = Camera.main.WorldToViewportPoint(new Vector3(Screen.width / 2.4f, 6, 0));
            downTrans.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)) + new Vector3(0, transform.localScale.y / 2, 0);
            upTrans.localScale = Camera.main.WorldToViewportPoint(new Vector3(Screen.width / 2.4f, 6, 0));
            upTrans.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)) - new Vector3(0, transform.localScale.y / 2, 0);
        }
        if ((left.check && playerTrans.position.x >= transform.position.x) || (right.check && playerTrans.position.x <= transform.position.x) || (!left.check && !right.check))
        {
            x = Mathf.MoveTowards(transform.position.x, playerTrans.position.x, Time.deltaTime * 9);
        }
        if ((down.check && playerTrans.position.y >= transform.position.y) || (up.check && playerTrans.position.y <= transform.position.y) || (!down.check && !up.check))
        {
            if (player.jumpCheck)
                y = transform.position.y;
            else
                y = Mathf.MoveTowards(transform.position.y, playerTrans.position.y, Time.deltaTime * 9);
        }
        transform.position = new Vector3(x, y, -10);
    }
}
