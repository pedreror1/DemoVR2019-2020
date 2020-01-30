using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
    public class SMPlayerPawn:MonoBehaviour
    {
        public Transform Target;
        public float Speed = 0.2f;

        [HideInInspector] public List<Transform> CollidingObjects= new List<Transform>();
        [HideInInspector] public List<Transform> NearObjects = new List<Transform>();
        [HideInInspector] public int ID;
        private bool isInside;
        [HideInInspector] public int RegionID = 0;


    private void OnEnable()
    {
        QuadTreeManager.Instance.addPawn(this);
    }
    
    private void Start()
    {
        Speed = Random.Range(0.01f, 0.14f);
        if (isInside)
        {
            Target = QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
        }
    }
        public void UpdateCollision(int ID, bool isOnShadow, List<Transform> collisions = null)
        {

            if (isOnShadow && collisions!=null &&  collisions.Count>0)
            {
                isInside = true;
                RegionID = ID;

                if (isInside && collisions.Contains(Target))
                {
                    Target = QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
                    Speed = Random.Range(0.01f, 0.34f);
                }
            }
            else
            {               
                if (RegionID != ID && QuadTreeManager.Instance.Regions[RegionID].getPlayerInBounds(this) == -1)
                {                
                    RegionID = ID;                    
                }
            }
        }

        private void Update()
        {
            if (Target)
            {
                transform.LookAtY(transform.position, Target.position);
                transform.position += transform.forward * Speed;
            }
            else
            {                  
                Target = QuadTreeManager.Instance.Regions[RegionID].getRandomChild();
            }
        }


    }


  