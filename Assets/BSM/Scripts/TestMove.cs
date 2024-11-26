using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float _movSpeed;
    [SerializeField] private GameObject _tempMission;
    

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
     
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.right * 20f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 3f);

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);

            if(hit.collider.gameObject.name == "TempMissionObject")
            {
                ActiveMission mission = hit.collider.GetComponent<ActiveMission>();
                
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //�̼� Ȱ��ȭ
                    mission.GetMission();
                }

            }
        }
         

        PlayerInput();


    }

    private void FixedUpdate()
    {
        MoveState();
    }

    private Vector2 _pos;
    private void PlayerInput()
    {
        _pos.x = Input.GetAxisRaw("Horizontal");
        _pos.y = Input.GetAxisRaw("Vertical");
       
    }

    private void MoveState()
    {
        _rb.MovePosition(_rb.position + _pos.normalized * _movSpeed *Time.fixedDeltaTime);
    }


}
