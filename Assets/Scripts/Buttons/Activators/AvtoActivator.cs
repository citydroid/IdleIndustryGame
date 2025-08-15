using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvtoActivator : MonoBehaviour, IButtonAction
{
    [Header("Объект, который нужно включить")]
    [SerializeField] private GameObject objectToActivate;

    public void Execute()
    {
        Debug.Log("AvtoActivator: Execute вызван");

        if (objectToActivate == null)    return;

        objectToActivate.SetActive(true);
    }
}
