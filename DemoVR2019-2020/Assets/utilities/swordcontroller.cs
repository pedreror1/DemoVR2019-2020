using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordcontroller : MonoBehaviour
{
    SwordTrailEffect1 sfect;
    // Start is called before the first frame update
    void Start()
    {
        sfect = GetComponentInChildren<SwordTrailEffect1>();
    }
    public void enableSlash()
    {
        sfect.enableSlash = true;
    }
    public void DisableSlash()
    {
        sfect.enableSlash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("attack");
        }
    }
}
