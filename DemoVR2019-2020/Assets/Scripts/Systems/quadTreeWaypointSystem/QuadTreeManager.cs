using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
    public class QuadTreeManager : SingletonComponent<QuadTreeManager>
    {

        public List<SMPlayerPawn> Pawns= new List<SMPlayerPawn>();
        public List<QuadTreeRegion> Regions = new List<QuadTreeRegion>();
        [Range(0.0001f,0.0510f)]
        public float FadeSpeed;
        //public Transform PlayerPawn;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (SMPlayerPawn pawn in Pawns)
            {
                Gizmos.DrawWireCube(pawn.transform.position, Vector3.one);
                if (pawn.NearObjects.Count > 0)
                {
                    drawCollisions(pawn);
                }
            }
        }
        void drawCollisions(SMPlayerPawn pawn)
        {
            Gizmos.color = Color.yellow;
            foreach (SMPlayerPawn Pawn in QuadTreeManager.Instance.Pawns)
            {
                foreach (Transform nearObj in Pawn.NearObjects)
                {
                    Gizmos.DrawLine(Pawn.transform.position, nearObj.position);
                    Gizmos.DrawWireSphere(nearObj.position, 0.45f);
                }

            }
             
            foreach (Transform t in pawn.CollidingObjects)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(t.position, 1.45f);
            }
        }


        public void addPawn(SMPlayerPawn pawn)
        {
            Pawns.Add(pawn);
            for (int i = 0; i < Pawns.Count; i++)
            {
                Pawns[i].ID = i;
            }
    }
        public void removePawn(SMPlayerPawn pawn)
        {
            Pawns.Remove(pawn);
            for (int i = 0; i < Pawns.Count; i++)
            {
                Pawns[i].ID = i;
            }
        }

        public void UpdateCollision(int regionID, bool isOnShadow, int pawnID, List<Transform> collisions = null)
        {
            Pawns[pawnID].UpdateCollision(regionID, isOnShadow, collisions);            
        }
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Regions.Add(transform.GetChild(i).GetComponent<QuadTreeRegion>());
                Regions[i].id = i;
            }
        }
        
    }

 