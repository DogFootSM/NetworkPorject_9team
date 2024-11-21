using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] float moveSpeed;
    [SerializeField] SpriteRenderer body;

    [SerializeField] Color[] colors;
    private int count;


    private void Start()
    {
        count = PhotonNetwork.ViewCount - 1;
        body.color = colors[count];
    }
    private void Update()
    {
        if (photonView.IsMine == false)  // �������� ����
            return;

         Move();
         body.color = colors[count];
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");


        transform.Translate(Vector2.right * moveSpeed * x * Time.deltaTime);
        transform.Translate(Vector2.up * moveSpeed * y * Time.deltaTime);

        
        if (x <= 0) // �������� �̵� ��
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else // ���������� �̵� ��
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
