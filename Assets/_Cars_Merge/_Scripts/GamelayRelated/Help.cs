using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Help : MonoBehaviour
{
    private bool _canDisableNow;
    private void Start()
    {
        DOVirtual.DelayedCall(2, () =>
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _canDisableNow = true;
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canDisableNow)
        {
            gameObject.SetActive(false);
        }
    }
}
