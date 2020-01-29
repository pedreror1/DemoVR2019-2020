using System.Collections;
using System.Collections.Generic;
  using UnityEngine;

public class swordWeapon : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + " OUCH!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
