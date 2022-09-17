using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AmbulaneCameraManager : MonoBehaviour
{
    public GameObject initialCam, gamePlayCam, ambuDriver;
    public Vector3 movePos;
    public GameObject HUD;
    public List<GameObject> thingsToActivate;
    
    void Start()
    {
        StartCoroutine(ChangeToGameplayView());
    }

    IEnumerator ChangeToGameplayView()
    {
        yield return new WaitForSeconds(1.5f);
        //ambuDriver.transform.DOMove(movePos, 0.5f).OnComplete(() =>
        //{
        //    ambuDriver.SetActive(false);
        //});
        yield return new WaitForSeconds(.5f);
        gamePlayCam.SetActive(true);
        initialCam.SetActive(false);
        //for (int i = 0; i < thingsToActivate.Count; i++)
        //{
        //    thingsToActivate[i].SetActive(true);
        //}
        yield return new WaitForSeconds(1.5f);
        HUD.SetActive(true);
        
    }
}
