using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionState : MonoBehaviour
{
    [field: SerializeField] public string MissionName {  get; set; }
    [field: HideInInspector] public int ObjectCount { get; set; }
    [field: HideInInspector] public bool IsDetect { get; set; }
    [field: HideInInspector] public Vector2 MousePos { get; set; } 
    [field: HideInInspector] public PlayerType MyPlayerType { get; set; }
    [field: HideInInspector] public bool IsPerform { get; set; }
    [field: HideInInspector] public bool IsAssign { get; set; }

    [field: HideInInspector] public int TextIndex { get; set; } = -1;

    //������ �÷� ��
    public static Color PlayerColor { get; set; }

    [Header("�̼� ��� ����Ʈ")]
    [Tooltip
        ("[0] �̼� ��ȣ�ۿ� ����\n" +
        "[1] �̼� Ŭ���� ����\n" +
        "[2] X ��ư Ŭ�� ����")]
    [SerializeField] public List<AudioClip> _clips = new List<AudioClip>();
    [HideInInspector] public Animator _anim;

    [HideInInspector] public int _openHash;
    [HideInInspector] public int _closeHash;

    private bool isAnimCheck;

    private void Awake()
    {
        //_anim = GetComponent<Animator>();
        isAnimCheck = TryGetComponent<Animator>(out _anim);

        _openHash = Animator.StringToHash("OpenPopup");
        _closeHash = Animator.StringToHash("ClosePopup"); 
    }

    private void OnEnable()
    {
        if(isAnimCheck)
            _anim.Play(_openHash);
        Debug.Log($"���� �̼� ���� : {MissionName}");
    }

    public void ClosePopAnim()
    {
        if(isAnimCheck)
            _anim.Play(_closeHash);
    }

}
