using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate_Car : MonoBehaviour
{
    public GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        Instantiate(car,transform.position,transform.rotation);
    }
}
