using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CAR : MonoBehaviour
{
    public float speed;
    private float tempSpeedHolder;

    private bool moving;
    int direction;
    Vector3 startPos;
    [SerializeField]private Transform TargetPosition;

    private Vector3 restPos;
    private bool Rest;

    // Start is called before the first frame update
    void Start()
    {
        Rest = false;
        moving = false;
        direction = -1;
        tempSpeedHolder = speed;
        startPos = transform.position;
        restPos = startPos;
    }

    private void OnMouseDown()
    {
        moving = true;
        direction *= -1;
        StartCoroutine(MoveCar());
    }

  
    IEnumerator MoveCar()
    {
        do
        {
            Move();
            yield return null;
        }
        while (moving);
    }

    void Move()
    {
        transform.position += direction * transform.forward * tempSpeedHolder * Time.deltaTime;
        var temp = transform.position - new Vector3(TargetPosition.position.x, transform.position.y, TargetPosition.position.z);
        float distance = temp.magnitude;
        if (distance < .3f && !Rest)
        {
            RRaycast();
            moving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("car"))
        {
            RRaycast();
            moving = false;
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            moving = false;
            Rest = false;
        }
    }

    void RRaycast()
    {
        if (Physics.Raycast(transform.position+new Vector3(0,1f,0), -transform.up, out RaycastHit hit, Mathf.Infinity))
        {
            restPos =new Vector3(hit.transform.position.x,transform.position.y,hit.transform.position.z);
            Rest = true;
            transform.DOMove(restPos,.3f);
        }     
    }

    
}
