using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Stop());
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
