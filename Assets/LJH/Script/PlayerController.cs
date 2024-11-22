using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviourPun
{
    // �÷��̾� Ÿ�� ����(�ù�) ����(��������) 
    [SerializeField] PlayerType playerType;
    [SerializeField] float moveSpeed;


    [SerializeField] SpriteRenderer body;

    [SerializeField] Color[] colors;

    [SerializeField] Animator eyeAnim;
    [SerializeField] Animator bodyAnim;
    [SerializeField] Animator feetAnim;
    private int count;

    private Vector3 privPos;
    private Vector3 privDir;
    [SerializeField] float threshold = 0.001f;
    bool isOnMove = false;


    private void Start()
    {
        playerType = PlayerType.Goose;   // �������� ���� �����ϴ� ����� �ʿ� (���� ���忡�� �ʿ���� ���� ����� �ʿ�)

        count = PhotonNetwork.ViewCount - 1;  // ���� ������� �� ���� 
        body.color = colors[count];
        Debug.Log($"�÷��̾� �ѹ�{count}");

    }
    private void Update()
    {
        if (photonView.IsMine == false)  // �������� ����
            return;

        Move();
        MoveCheck(); 
    }


    // ������ ���Ǿ�� �ֺ� ������Ʈ Ž��(�̴ϰ��� , �纸Ÿ�� , ��ü , ���ȸ�� , �ٸ� �÷��̾�) Ž���� ������Ʈ�� ���� �ٸ� �ൿ�� �����ϰ�

    private void FindNearObject() 
    {

    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y).normalized;

        if (moveDir == Vector2.zero)
            return;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        if (x < 0) // �������� �̵� ��
        {   
            privDir = new Vector3(1, 1, 1);
            transform.localScale = privDir;
        }
        else if (x > 0) // ���������� �̵� ��
        {
            privDir = new Vector3(-1, 1, 1);
            transform.localScale = privDir;
        }
        else  // ���� ������ �����ϰ� 
        {
            transform.localScale =privDir;  
        }
    }

    private void MoveCheck() 
    {
       
        float distance = Vector3.Distance(transform.position, privPos);

        if (distance > threshold)
        {
            if (!isOnMove) 
            {
                isOnMove = true;
                eyeAnim.SetBool("Running", true);
                bodyAnim.SetBool("Running", true);
                feetAnim.SetBool("Running", true);

            }
        }
        else
        {
            if (isOnMove) // ���°� ����� ��쿡�� �ִϸ��̼� ������Ʈ
            {
                isOnMove = false;
                eyeAnim.SetBool("Running", false);
                bodyAnim.SetBool("Running", false);
                feetAnim.SetBool("Running", false);
            }
        }
        privPos = transform.position;
    }
}
