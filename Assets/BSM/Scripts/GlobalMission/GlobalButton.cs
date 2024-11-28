using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalButton : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _powerClips = new List<AudioClip>();

    private bool ButtonActive;
    private bool LightActive;
    public bool ButtonCheck { get { return ButtonActive; } }

    private int _completeHash;
    private int _powerCount;
    public int PowerCount
    {
        get
        {
            return _powerCount;
        }
        set
        {
            //PowerCount�� ��ȭ�� ���� ������ ȣ��
            _powerCount = value;
             
            if(_powerCount < 0)
            {
                LightActive = ButtonActive;
                _lightAnimator.SetBool(_completeHash, LightActive); 
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

    private void OnDisable()
    {
        ButtonActive = false;
        LightActive = false;
    }

    /// <summary>
    /// ����ġ On, Off ����, �ִϸ��̼� ��� ���
    /// </summary>
    public void PlayAnimation()
    {
        ButtonActive = !ButtonActive;

        if (ButtonActive)
        {
            SoundManager.Instance.SFXPlay(_powerClips[1]);
             
        }
        else
        {
            SoundManager.Instance.SFXPlay(_powerClips[0]); 
        }

        _buttonAnimator.SetBool(_completeHash, ButtonActive);
         
    }
     
}
