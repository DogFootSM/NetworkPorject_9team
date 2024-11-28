using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCol : MonoBehaviour
{
    [SerializeField] BoxCollider2D col;
    public float rayLength = 10f; // ���� ����
    public LayerMask collisionLayer;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2[] hitPoints = new Vector2[4]; // �浹 ������ ������ �迭
    private void Start()
    {
        
    }

    private void LateUpdate()
    { 
       // CreateRectangleCollider();
       CircleRayCast(); 
    }

    private void CircleRayCast() 
    {
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, 0.1f, Vector2.up, rayLength);
        if (ray.transform.gameObject != this.gameObject) 
        {
            if (ray.transform.gameObject.layer == 7)
            {
            Debug.Log("�÷��̾� ��");

            }
            else 
            {
            Debug.Log("�÷��̾� ����");
            }

        }
        
    }

    private void CreateRectangleCollider()
    {
        // ����ĳ��Ʈ ����
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], rayLength, collisionLayer);
            Debug.DrawRay(transform.position, directions[i], Color.red);
            if (hit.collider != null)
            {
                hitPoints[i] = hit.point; // �浹 ���� ����
            }
            else
            {
                if (i == 0)
                {
                    hitPoints[i] = new Vector2(transform.position.x, transform.position.y+rayLength);
                }
                else if (i == 1) 
                {
                    hitPoints[i] = new Vector2(transform.position.x, transform.position.y-rayLength);
                }
                else if (i == 2)
                {
                    hitPoints[i] = new Vector2(transform.position.x-rayLength, transform.position.y);
                }
                else if (i == 3)
                {
                    hitPoints[i] = new Vector2(transform.position.x+rayLength, transform.position.y);
                }

            }
        }

        // �簢�� �߽ɰ� ũ�� ���
        Vector2 center = new Vector2(
            (hitPoints[2].x + hitPoints[3].x) / 2, // ���ʰ� �������� �߰� x��
            (hitPoints[0].y + hitPoints[1].y) / 2  // ���ʰ� �Ʒ����� �߰� y��
        );

        Vector2 size = new Vector2(
            Mathf.Abs(hitPoints[3].x - hitPoints[2].x), // ������ - ����
            Mathf.Abs(hitPoints[0].y - hitPoints[1].y)  // �� - �Ʒ�
        );

        // ���� BoxCollider2D�� �ִٸ� ����
        
        
        col.offset = center - (Vector2)transform.position; // ���� ��ġ�� ������ ����
        col.size = size;

        Debug.Log($"Rectangle Collider created at Center: {center}, Size: {size}");
    }
}
