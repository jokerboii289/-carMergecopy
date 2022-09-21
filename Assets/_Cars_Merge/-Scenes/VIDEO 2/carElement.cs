using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carElement : MonoBehaviour
{
    public float speed;
    public Material color; 

    // Update is called once per frame
    void Update()
    {
        transform.position += speed*transform.forward * Time.deltaTime;
        RaycastTiles();
    }

    void RaycastTiles()
    {
        if(Physics.Raycast(transform.position+ new Vector3(0,1,0),-transform.up,out RaycastHit hit,Mathf.Infinity))
        {
            if(hit.transform.CompareTag("tile"))
            {
                var obj = hit.transform.GetChild(0);
                obj.GetComponent<MeshRenderer>().material = color;
            }
        }
    }
}
