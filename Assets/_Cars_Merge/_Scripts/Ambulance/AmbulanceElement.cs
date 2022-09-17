using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using UnityEngine;


public class AmbulanceElement : MonoBehaviour
{
    public bool canMove;
    public float speed;

    //public int i=1;
    private void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.forward * 10, Color.black);
        if (!Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.forward, out hit, Mathf.Infinity))
        {
            //i = 1;
            StartCoroutine(MoveTheAmbulance());
        }
        //if (!Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), -transform.forward, out hit, Mathf.Infinity))
        //{
        //    i = -1;
        //    StartCoroutine(MoveTheAmbulance());
           
        //   }
        //else
        //    StartCoroutine( CheckDistance(hit));

        if (canMove) transform.position +=transform.forward * speed * Time.deltaTime;
    }

    private bool _moveCalled;
    IEnumerator MoveTheAmbulance()
    {
        if (_moveCalled) yield break;
        _moveCalled = true;
        yield return new WaitForSeconds(1.2f);
        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            AmbulanceController.instance.AmbulanceCounter();
        }
    }

    IEnumerator CheckDistance(RaycastHit hit)//optional
    {
        print(hit.distance);
        do
        {
            var distance = (1 / hit.distance) * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, hit.point, distance);
            yield return null;
        }
        while (hit.distance > 4f && !canMove);
    }
    

}

