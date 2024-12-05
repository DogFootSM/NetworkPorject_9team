using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIs
{
    public class GameStartUI : BaseUI
    {
        [SerializeField] public float Duration;

        private GameObject _gooseUI => GetUI("GooseBackGround");
        private GameObject _duckUI => GetUI("DuckBackGround");

        private Image _playerImage => GetUI<Image>("PlayerBody");

        private void Awake()
        {
            Bind();
        }
        private void Start()
        {
            SetActive(false);
        }

        public void SetActive(bool value)
        {
            GetUI("GameStartUI").SetActive(value);
            if (value == true)
            {
                StartCoroutine(DurationRoutine());
            }
        }

        /// <summary>
        /// ���� ȭ�� �Ǵ� ���� ȭ�� ����
        /// �÷��̾� ���� ���� ����
        /// </summary>
        public void SetUI(PlayerType type, Color color)
        {
            SoundManager.SFXPlay(type == PlayerType.Goose ? SoundManager.Data.GooseIntro : SoundManager.Data.DuckIntro);

            _gooseUI.SetActive(type == PlayerType.Goose);
            _duckUI.SetActive(type == PlayerType.Duck);

            _playerImage.color = color;
        }

        /// <summary>
        /// ���ӽð����ȸ� ��Ÿ��
        /// </summary>
        IEnumerator DurationRoutine()
        {
            yield return Duration.GetDelay();
            GetUI("GameStartUI").SetActive(false);
        }
    }
}

