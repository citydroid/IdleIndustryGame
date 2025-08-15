using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("������, ������� ����� ��������")]
    [SerializeField] private GameObject objectToActivate;

    public void Execute()
    {
        Debug.Log("AvtoActivator: Execute ������");

        if (objectToActivate == null)    return;

        objectToActivate.SetActive(true);
    }
}
