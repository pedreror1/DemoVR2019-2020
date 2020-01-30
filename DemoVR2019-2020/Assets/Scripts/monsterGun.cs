using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
public class monsterGun : MonoBehaviour
{

    [SerializeField] Transform shootPos;
    [SerializeField] float timebetweenShots = 0.35f;
    float lastShotTime;
    bool canshoot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canshoot)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f)
            {
                var newBullet = MonsterBulletPool.Instance.getFromPool();
                newBullet.transform.position = shootPos.position;
                newBullet.transform.parent = MonsterBulletPool.Instance.transform;
                newBullet.transform.rotation = shootPos.rotation;
                newBullet.gameObject.SetActive(true);
                canshoot = false;
                lastShotTime = Time.time;
            }
        }
        else
        {
            if(Time.time>(lastShotTime+timebetweenShots))
            {
                canshoot=true;

            }
        }
    }
}
