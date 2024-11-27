using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalButton : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _powerClips = new List<AudioClip>();

    private bool isPlay;
    private int _completeHash;
    private int _lightHash;


    private int _powerCount;
    public int PowerCount
    {
        get
        {
            return _powerCount;
        }
        set
        {
            _powerCount = value;

            if(_powerCount < 1)
            {
                //���� ų �� �ְ� Bool ���� ����
            }


        }
    }

    private Animator _buttonAnimator;
    private Animator _lightAnimator;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _buttonAnimator = GetComponent<Animator>();
        _lightAnimator = transform.GetChild(0).GetComponent<Animator>();

        _completeHash = Animator.StringToHash("Complete");

    }

    private void OnEnable()
    {
        _powerCount = Random.Range(1, 15); 
    }

    public void PlayAnimation()
    {
        isPlay = !isPlay;

        if (isPlay)
        {
            SoundManager.Instance.SFXPlay(_powerClips[1]);
        }
        else
        {
            SoundManager.Instance.SFXPlay(_powerClips[0]);
        }

        _buttonAnimator.SetBool(_completeHash, isPlay);
        _lightAnimator.SetBool(_completeHash, isPlay);

        


    }


    //�� ��ư �� Ŭ���ؾ� �� Ƚ�� > Random.Range�� �ο�
    //�� ��ư �� ��ũ��Ʈ�� �Ѱ��ִ°� ������
}
