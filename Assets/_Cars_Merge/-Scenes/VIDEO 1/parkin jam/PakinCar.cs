using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PakinCar : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    private bool navemesh;

    public float speed;
    private float tempSpeedHolder;

    private Vector3 startPos, restPos;
    private Vector3 touchStartPos,touchEndPos;

    [SerializeField] private bool straight;
    //public Camera camera;
    float Yoffset;
    // Start is called before the first frame update
    void Start()
    {
        navemesh = false;
        agent = GetComponent<NavMeshAgent>();

        tempSpeedHolder = speed;
        startPos = transform.position;
        restPos = startPos;
    }

    // Update is called once per frame
    void Update()
    {      
        if (navemesh)
            agent.SetDestination(target.position);
    }

    private void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (!navemesh)
        {
            var threshold = (touchStartPos - Input.mousePosition).magnitude;
            if (threshold > 50)
            {
                print("can move");
                touchEndPos = Input.mousePosition;
                if (straight)
                {
                    var dir = touchStartPos.y - touchEndPos.y;

                    dir = Mathf.Sign(dir) * -1;

                    var tempdir = Vector3.Dot(transform.forward, Vector3.forward);
                    if (tempdir < 0)
                        dir = dir * -1;
                    if (tempdir > 0)
                        dir = dir * 1;

                    transform.position += dir * transform.forward * 10f * Time.deltaTime;
                }
                else
                {
                    var dir = touchStartPos.x - touchEndPos.x;
                    print(dir);
                    dir = Mathf.Sign(dir) * -1;

                    var tempdir = Vector3.Dot(transform.forward, Vector3.right);
                    if (tempdir < 0)
                        dir = dir * -1;
                    if (tempdir > 0)
                        dir = dir * 1;

                    transform.position += dir * transform.forward * 10f * Time.deltaTime;
                }
            }
        }
    }


    private void OnMouseUp()
    {
        RRaycast();
    }
  
    void RRaycast()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), -transform.up, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("tile"))
            {
                restPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                transform.DOMove(restPos, .3f);
            }
            if (hit.transform.CompareTag("road"))
            {
                navemesh = true;
                transform.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("car"))
            RRaycast();
    }
}
