using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class CarElement : MonoBehaviour
    {
        public TextMeshPro numText;
        public int num;
        public GameObject engineSmoke;
        public Transform tileOccupied;
        public bool canRaycast;

        private Vector3 _rayPos;
        
        private void Start()
        {
            numText.text = num.ToString();
            canRaycast = true;
        }

        private void Update()
        {
            if (canRaycast)
            {
                _rayPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(_rayPos, Vector3.down, out hit, 5))
                {
                    Debug.DrawRay(_rayPos, Vector3.down * 5, Color.green);
                    
                    if (hit.collider.transform.CompareTag("tile"))
                    {
                        tileOccupied = hit.collider.transform;
                        //print("tile detected !!!");
                    }
                }          
            }
        }
    }   
}
