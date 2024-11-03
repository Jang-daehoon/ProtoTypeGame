using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public Transform desPos;
    public float speed;
    public bool isPlayerConPlatform;    //�÷��̾�� �����ؾ� �۵��ϴ� ������Ʈ�ΰ�?
    public bool isPlayerConnect;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        desPos = endPos;    
    }

    private void FixedUpdate()
    {
        //�÷��̾�� �����ؾ� �۵��ϴ� �÷����� ���
        if(isPlayerConPlatform == true && isPlayerConnect == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.fixedDeltaTime * speed);
            //DesPos�� �����ϸ� DesPos���� StartPos�� �ʱ�ȭ �Ÿ��� 0.05f ������ �� �������� �ٲ��ش�.
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
            //DesPos�� �����ϸ� DesPos���� StartPos�� �ʱ�ȭ �Ÿ��� 0.05f ������ �� �������� �ٲ��ش�.
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
