using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsOn: MonoBehaviour
{
    public void UpdateArrowUp(int _level)
    {
        gameObject.SetActive(_level > 0);
    }
    public void UpdateArrowDownOn()
    {
        gameObject.SetActive(true);
    }
    public void UpdateArrowDownMax()
    {
        gameObject.SetActive(false);
    }
}
