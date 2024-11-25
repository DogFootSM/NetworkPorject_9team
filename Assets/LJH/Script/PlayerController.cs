using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPun
{
    // �÷��̾� Ÿ�� ����(�ù�) ����(��������) 
    [SerializeField] PlayerType playerType;
    [SerializeField] float moveSpeed;  // �̵��ӵ� 
    [SerializeField] float Detectradius;  // Ž�� ����



    [SerializeField] GameObject IdleBody;
    [SerializeField] GameObject Ghost;
    [SerializeField] GameObject Corpse;



    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer GhostRender;
    [SerializeField] SpriteRenderer CorpRender;

    [SerializeField] Animator eyeAnim;
    [SerializeField] Animator bodyAnim;
    [SerializeField] Animator feetAnim;



    private Vector3 privPos;
    private Vector3 privDir;
    [SerializeField] float threshold = 0.001f;

    bool isOnMove = false;
    public bool isGhost = false;


    Coroutine coroutine;
    private void Start()
    {

        // �������� ���� �����ϴ� ����� �ʿ� (���� ���忡�� �ʿ���� ���� ����� �ʿ�)
        if (photonView.IsMine == false)  // �������� ����
            return;
        playerType = PlayerType.Duck;


        // ���� �� ���� , ���Ŀ� �� ���� ����� ������ ���� ��� ���� ���ڸ� ������ �ɵ�   
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);
        photonView.RPC("RpcSetColors", RpcTarget.AllBuffered, randomColor.r, randomColor.g, randomColor.b);



    }
    private void Update()
    {

        if (photonView.IsMine == false)  // �������� ����
            return;

        Move();
        MoveCheck();
        FindNearObject();
    }

    // r �Ű� , e ��ȣ�ۿ� , space ���� 
    // �ֺ� ������Ʈ Ž��(�̴ϰ��� , �纸Ÿ�� , ��ü , ���ȸ�� , �ٸ� �÷��̾�) Ž���� ������Ʈ�� ���� �ٸ� �ൿ�� �����ϰ�
    // �Ű� �Ǹ� ��ü�� ������� �� 
    private void FindNearObject()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, Detectradius);
        Collider2D nearCol = null;
        float minDistance = 1000f;
        foreach (Collider2D col in colliders)
        {
            if (col.transform.position != this.transform.position) // �ڽ� �ƴϰ� 
            {
                if (col.gameObject.layer != 9) // ���� �ƴϰ� 
                {
                    if (col != null)
                    {
                        float distance = Vector2.Distance(this.transform.position, col.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;  // ���� ����� ��ü ã��()
                            nearCol = col;
                           
                        }
                    }

                }

            }

        }
        if (nearCol.tag == "Test")
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("�Ű���!");

                // ��ǥ ������ ��� �߰� �ؾ��� 

                nearCol.gameObject.GetComponent<ReportingObject>().Reporting(); //�Ű�� ��ü ����

            }
        }
        else if (nearCol.gameObject.layer == gameObject.layer)
        {
            //Killing(col);
            coroutine = StartCoroutine(Kill(nearCol));
        }
        else if (nearCol.gameObject.layer == 8)  // �̼� 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                coroutine = StartCoroutine(PlayMission());
            }
        }
        else if (nearCol.gameObject.layer == 10) // �纸Ÿ��(�������͸� ���� )
        {
            if (playerType == PlayerType.Duck)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    coroutine = StartCoroutine(PlaySabotage());
                }
            }
        }
    }
    IEnumerator Kill(Collider2D col)
    {

        if (col.gameObject != this.gameObject)
        {
            if (playerType == PlayerType.Duck)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    Debug.Log("����!"); // ��Ÿ�� �߰��ؾ��� 

                    col.gameObject.GetComponent<PlayerController>().Die();
                }

            }

        }
        yield return new WaitForSeconds(1f);

    }
    IEnumerator PlayMission()
    {

        Debug.Log("�̼�!");

        yield return new WaitForSeconds(1f);

    }
    IEnumerator PlaySabotage()
    {

        Debug.Log("�纸Ÿ��!");

        yield return new WaitForSeconds(1f);

    }



    public void Die()
    {
        StartCoroutine(switchGhost());
    }
    IEnumerator switchGhost()
    {

        Debug.Log(photonView.ViewID);
        Debug.Log(isGhost);

        photonView.RPC("RpcChildActive", RpcTarget.All, "GooseIdel", false);
        photonView.RPC("RpcChildActive", RpcTarget.All, "Goosecorpse", true);
        yield return new WaitForSeconds(1f);
        photonView.RPC("RpcChildActive", RpcTarget.All, "GoosePolter", true);
    }

   
    private void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 5f,8);
        Debug.DrawRay(transform.position, transform.right * 5f, Color.yellow);
        if (hit.collider != null)
        {          
                print(hit.transform.name);
        }
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.right, 5f, 10);
        Debug.DrawRay(transform.position, transform.right * 5f, Color.yellow);
        if (hit1.collider != null)
        {
            print(hit1.transform.name);
        }


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
            transform.localScale = privDir;
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

    [PunRPC]
    private void RpcChildActive(string name, bool isActive)
    {

        if (name == "GooseIdel")
        {
            IdleBody.SetActive(isActive);
        }
        else if (name == "Goosecorpse")
        {
            Corpse.SetActive(isActive);
            Corpse.transform.SetParent(null);
        }
        else if (name == "GoosePolter")
        {
            isGhost = true;
            Ghost.SetActive(isActive);

        }
    }

    [PunRPC]

    private void RpcSetColors(float r, float g, float b)
    {
        Color color = new Color(r, g, b);
        body.color = color;
        GhostRender.color = color;
        CorpRender.color = color;
    }
}
