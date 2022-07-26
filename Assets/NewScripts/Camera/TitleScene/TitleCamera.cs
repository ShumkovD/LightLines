using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C�g���J�����̈ړ�
/// </summary>
public class TitleCamera : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(0, _rotateSpeed * Time.deltaTime, 0));
    }
}
