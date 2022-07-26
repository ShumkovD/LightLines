using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// �I�������X�e�[�W�̋L�^�\��
/// </summary>
public class StageSelect : MonoBehaviour
{
    [SerializeField] string _stageName = default;
    [SerializeField] GameObject _stageInfoPanel;
    [SerializeField] Sprite _sourceSprite;
    [SerializeField] Image _stageImage;
    [SerializeField] Text _nameText;
    [SerializeField] Text _timeText;
    [SerializeField] Text _itemText;

    StageInfo _stageInfo;
    bool _active = false;

    void Start()
    {
        _stageName = this.name;
    }

    private void Update()
    {
        if(FadeInOut._OnSceneChange) { return; }

        //���͌��m�A�w��X�e�[�W�Ɉڍs����
        if (_active)
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.buttonWest.wasPressedThisFrame)
                {
                    StageManager._Instance.MoveToSelectScene(_stageName);
                    StageManager._Instance.IsStageClear = true;
                    _stageInfoPanel.SetActive(false);
                    Camera.main.GetComponent<OrbitCamera>().StageSelectPeriod(false);

                    AudioManager.PlayClickAudio();
                }
            }

            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                StageManager._Instance.MoveToSelectScene(_stageName);
                StageManager._Instance.IsStageClear = true;
                _stageInfoPanel.SetActive(false);
                Camera.main.GetComponent<OrbitCamera>().StageSelectPeriod(false);

                AudioManager.PlayClickAudio();
            }
        }
    }

    /// <summary>
    ///  �X�e�[�W�C���t�H���[�V�����p�l���̃e�L�X�g�ݒ�
    /// </summary>
    void SetStageInfoPanel()
    {
        _stageInfo = DataManager._Instance.GetStageInfo(_stageName);
        _stageImage.sprite = _sourceSprite;
        _nameText.text = _stageName;
        string s = String.Format("TIme: {0:00}:{1:00}:{2:00}", 
            (int)_stageInfo.ClearTime / 60, 
            (int)_stageInfo.ClearTime % 60, 
            (_stageInfo.ClearTime - (int)_stageInfo.ClearTime) * 100);
        _timeText.text = s;
        _itemText.text = "Item: " + _stageInfo.SecretItemCount.ToString() + " / " + _stageInfo.SecretItemMaxCount.ToString();
        _stageInfoPanel.SetActive(true);
    }

    //�v���C���[���m�A�X�e�[�W�C���t�H���[�V�����p�l���W��
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<OrbitCamera>().StageSelectPeriod(true);
            SetStageInfoPanel();
            _active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<OrbitCamera>().StageSelectPeriod(false);
            _stageInfoPanel.SetActive(false);
            _active = false;
        }
    }
}
