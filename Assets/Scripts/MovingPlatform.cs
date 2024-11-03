using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public Transform desPos;
    public float speed;
    public bool isPlayerConPlatform;    //플레이어와 접촉해야 작동하는 오브젝트인가?
    public bool isPlayerConnect;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        desPos = endPos;    
    }

    private void FixedUpdate()
    {
        //플레이어와 접촉해야 작동하는 플랫폼인 경우
        if(isPlayerConPlatform == true && isPlayerConnect == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.fixedDeltaTime * speed);
            //DesPos에 도착하면 DesPos값을 StartPos로 초기화 거리가 0.05f 이하일 때 목적지를 바꿔준다.
            if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
            {
                if (desPos == endPos)
                    desPos = startPos;
                else
                    desPos = endPos;
            }
        }
        else if(isPlayerConPlatform == false && isPlayerConnect == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.fixedDeltaTime * speed);
            //DesPos에 도착하면 DesPos값을 StartPos로 초기화 거리가 0.05f 이하일 때 목적지를 바꿔준다.
            if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
            {
                if (desPos == endPos)
                    desPos = startPos;
                else
                    desPos = endPos;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            isPlayerConnect = true;
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
