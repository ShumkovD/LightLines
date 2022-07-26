using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
//�Z�[�u�f�[�^
public class SaveData
{
    [SerializeField]
    private int _clearStageCount; //�N���A�̃X�e�[�W��
    [SerializeField]
    private List<StageInfo> _stageInfoList = new List<StageInfo>(); //�e�X�e�[�W�̋L�^

    public int ClearStageCount { get => _clearStageCount; set => _clearStageCount = value; }
    public List<StageInfo> StageInfoList
    {
        get => _stageInfoList;
    }

    public SaveData()
    {
        this.ClearStageCount = 0;
    }
    public SaveData(int clearStage, List<StageInfo> stageInfo)
    {
        this.ClearStageCount = clearStage;
    }
}

[System.Serializable]
//�X�e�[�W�L�^
public struct StageInfo
{
    [SerializeField]
    private string _stageName;
    [SerializeField]
    private float _clearTime;
    [SerializeField]
    private int _secretItemCount;
    [SerializeField]
    private int _secretItemMaxCount;

    public string StageName { get => _stageName; set => _stageName = value; }
    public float ClearTime
    {
        get => _clearTime;
        set
        {
            if (value < 0) { return; }

            if (this._clearTime > value || this._clearTime == 0)
            {
                this._clearTime = value;
            }
        }
    }
    public int SecretItemCount
    {
        get => _secretItemCount;
        set
        {
            if (value > 3) { return; }

            if (this._secretItemCount < value)
            {
                this._secretItemCount = value;
            }
        }
    }

    public int SecretItemMaxCount { get => this._secretItemMaxCount; set => this._secretItemMaxCount = value; }
}

/// <summary>
/// �f�[�^�Ǘ��A����p�N���X
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField] SaveData _saveData;

    public static DataManager _Instance;

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _saveData = LoadFromJson<SaveData>("savedata.json"); //���L�f�[�^�����[�h����
    }

    /// <summary>
    /// �N���A�L�^��ۑ��A�܂��͍X�V����
    /// </summary>
    /// <param name="stageInfo"></param>
    public void SaveStageClearData(StageInfo stageInfo)
    {
        bool checkdata = false;

        for(int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            //�f�[�^�����݂��Ă���Ƃ��A�f�[�^���X�V����
            if(stageInfo.StageName == _saveData.StageInfoList[i].StageName)
            {
                checkdata = true;
                StageInfo temp = _saveData.StageInfoList[i];
                //�N���A����
                temp.ClearTime = stageInfo.ClearTime;
                //�B���A�C�e��
                temp.SecretItemCount = stageInfo.SecretItemCount;
                _saveData.StageInfoList[i] = temp;
            }
        }

        //�f�[�^�����݂��Ă��Ȃ��Ƃ��A�f�[�^��ǉ�����
        if (!checkdata)
        {
            _saveData.StageInfoList.Add(stageInfo);
            _saveData.ClearStageCount++;
        }

        SaveByJson("savedata.json" ,_saveData);
    }

    /// <summary>
    /// �L�^������X�e�[�W�L�^���擾����
    /// </summary>
    /// <param name="stageName"></param>
    /// <returns></returns>
    public StageInfo GetStageInfo(string stageName)
    {
        if (_saveData == null) return new StageInfo();

        for (int i = 0; i < _saveData.StageInfoList.Count; ++i)
        {
            if(_saveData.StageInfoList[i].StageName == stageName)
            {
                return _saveData.StageInfoList[i];
            }
        }

        return new StageInfo();
    }

    /// <summary>
    /// �f�[�^�ۑ�
    /// </summary>
    /// <param name="fileName">�t�@�C����</param>
    /// <param name="data">�Z�[�u�f�[�^</param>
    public static void SaveByJson(string fileName, object data)
    {
        DeleteSaveFile(fileName);
        var json = JsonUtility.ToJson(data);
        //�ۑ��ʒu�A�f�o�C�X�ɂ���ĕύX����
        var path = Path.Combine(Application.persistentDataPath, fileName);
        
        try
        {
            File.WriteAllText(path, json);
            Debug.Log("save success");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// �t�@�C������̓ǂݍ���
    /// </summary>
    /// <typeparam name="T">�f�[�^�^�C�v</typeparam>
    /// <param name="fileName">�t�@�C����</param>
    /// <returns>�f�[�^</returns>
    public static T LoadFromJson<T>(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                Debug.Log("load success");
                return data;
            }
            else
            {
                Debug.Log("create new data");
                return System.Activator.CreateInstance<T>();
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return default;
        }
    }

    /// <summary>
    /// �Z�[�u�f�[�^�폜
    /// </summary>
    /// <param name="fileName">�t�@�C����</param>
    public static void DeleteSaveFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.Delete(path);
            Debug.Log("delete success");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
}
