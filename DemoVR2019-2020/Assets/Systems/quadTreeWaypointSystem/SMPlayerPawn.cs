using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
    public class SMPlayerPawn:MonoBehaviour
    {
        public Transform target;
        public float speed = 0.2f;

         [HideInInspector] public List<Transform> CollidingObjects= new List<Transform>();
        [HideInInspector] public List<Transform> NearObjects = new List<Transform>();
         [HideInInspector] public int ID;
         private bool isInside;
        [HideInInspector] public int RegionID = 0;



        private void Start()
        {
            speed = Random.Range(0.01f, 0.14f);
            if (isInside)
                target=  QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
        }
        public void UpdateCollision(int ID, bool isOnShadow, List<Transform> collisions = null)
        {
            if (isOnShadow && collisions!=null &&  collisions.Count>0)
            {
                isInside = true;
                RegionID = ID;
            if (isInside && collisions.Contains(target))
            {
                target = QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
                speed = Random.Range(0.01f, 0.34f);
            }
            }
            else
            {
               
                if (RegionID == ID)
                {
                     
                }
                else
                {
                    if (QuadTreeManager.Instance.Regions[RegionID].getPlayerInBounds(this) == -1)
                    {
                        RegionID = ID;
                        
                    }
                }
            }
        }
        private void Update()
        {


            if (target)
            {
                transform.LookAtY(transform.position, target.position);
                transform.position += transform.forward * speed;
            }
            else
            {
                  
                    target = QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
            }
        }


    }


  