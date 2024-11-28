using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadyUI :BaseUI
{
    public enum Image { Master , Ready, UnReady}

    private GameObject _master => GetUI("MasterImage");
    private GameObject _ready => GetUI("ReadyImage");
    private GameObject _unReady => GetUI("UnReadyImage");

    private void Awake()
    {
        Bind();
    }

    /// <summary>
    /// ���� �̹��� ��ü
    /// </summary>
    public void ChangeImage(Image image)
    {
        _master.SetActive(image == Image.Master);
        _ready.SetActive(image == Image.Ready);
        _unReady.SetActive(image == Image.UnReady);
    }
}
