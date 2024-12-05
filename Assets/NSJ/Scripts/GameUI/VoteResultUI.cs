using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class VoteResultUI : BaseUI
    {
        public enum Result { Kick, Skip }

        [SerializeField] public float _duration;

        private Image _playerImage => GetUI<Image>("Goose");
        private TMP_Text _nameText => GetUI<TMP_Text>("NameText");
        private TMP_Text _jobText => GetUI<TMP_Text>("JobText");
        private void Awake()
        {
            Bind();

        }
        private void Start()
        {
            SetActiveKick(false) ;
            SetActiveSkip(false);
        }

        public void SetActiveKick(bool value)
        {
            GetUI("VoteKickUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationKickRoutine());
            }
        }

        public void SetActiveSkip(bool value)
        {
            GetUI("VoteSkipUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationSkipRoutine());
            }
        }
        public void SetUI(Color playerColor, string name, PlayerType type)
        {
            _playerImage.color = playerColor;
            _nameText.SetText($"{name}�� �� �Ƹ��ٿ� �������� �������ϴ�.");

            string jobText = type == PlayerType.Goose ? "������ �ƴϾ�" : "������";
            _jobText.SetText($"�״� {jobText}���ϴ�.");
        }

        /// <summary>
        /// ���ӽð����ȸ� ��Ÿ��
        /// </summary>
        IEnumerator DurationKickRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteKickUI").SetActive(false);
        }

        /// <summary>
        /// ���ӽð����ȸ� ��Ÿ��
        /// </summary>
        IEnumerator DurationSkipRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteSkipUI").SetActive(false);
        }
    }
}

