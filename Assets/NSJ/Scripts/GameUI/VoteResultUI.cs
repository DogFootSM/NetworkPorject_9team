using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class VoteResultUI : BaseUI
    {
        [SerializeField] private float _duration;

        private Image _playerImage => GetUI<Image>("Goose");
        private TMP_Text _nameText => GetUI<TMP_Text>("NameText");
        private TMP_Text _jobText => GetUI<TMP_Text>("JobText");
        private void Awake()
        {
            Bind();
        }

        public void SetActive(bool value)
        {
            GetUI("VoteResultUI").SetActive(value);
            if (value)
            {
                StartCoroutine(DurationRoutine());
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
        IEnumerator DurationRoutine()
        {
            yield return _duration.GetDelay();
            GetUI("VoteResultUI").SetActive(false);
        }
    }
}

