using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class QuadTreeRegion : MonoBehaviour
    {
        [HideInInspector]
        public QuadTree ParentNode;        
        private List<QuadTree> nodes = new List<QuadTree>();
        private List<Transform> ShadowVolumes = new List<Transform>();       
        public float width = 6;
        public float height = 6;
        public int MaxLevels = 5;
        public int maxObjectsPerNode = 5;
        public int calculationsPerSecond = 24;
        public bool ShowDebug = true;
        public Color debugColor = Color.red;
        bool testMode = false;
         public int id = 0;
        int playerInBounds = -1;       

        [NaughtyAttributes.Button("get Childs")]
        public void getChilds()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ShadowVolumes.Add(transform.GetChild(i).transform);

            }
        }
        public Transform getRandomChild()
        {

            return transform.GetChild(Random.Range(0, 9999)% (transform.childCount));
        }
        void Start()
        {
            ParentNode = new QuadTree(transform.position, width, height, maxObjectsPerNode, MaxLevels, 0);
            
            for (int i = 0; i < transform.childCount; i++)
            {
                ShadowVolumes.Add(transform.GetChild(i).transform);
                 
            }
            if (testMode)
            {
                Test();
            }
            
            StartCoroutine(reCalculate());
        }
        void Test()
        {
            foreach (Transform t in ShadowVolumes)
            {
                Vector3 newpos = ParentNode.Position;
                newpos.x += Random.Range(-ParentNode.Width, ParentNode.Width);
                newpos.z += Random.Range(-ParentNode.Height, ParentNode.Height);
                t.position = newpos;
            }
        }
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || playerInBounds==-1)
            {
                Gizmos.color = debugColor;
                Gizmos.DrawWireCube(transform.position, new Vector3(width * 2, 1, height * 2));
            }
            if (ShowDebug)
            {

                if (ParentNode == null)
                {
                    Start();
                }
                drawAll(ParentNode);
                
            }
        }        
        void drawAll(QuadTree q)
        {
            for (int i = 0; i < q.Nodes.Count; i++)
            {
                if (q.Nodes[i].Nodes.Count > 0)
                {
                    drawAll(q.Nodes[i]);
                }
                else
                {
                    draw(q.Nodes[i]);
                }
            }

            draw(q);
        }
        void draw(QuadTree QT)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(QT.Position + new Vector3(-QT.Width, 0, QT.Height), QT.Position + new Vector3(QT.Width, 0, QT.Height));
            Gizmos.DrawLine(QT.Position + new Vector3(-QT.Width, 0, QT.Height), QT.Position + new Vector3(-QT.Width, 0, -QT.Height));
            Gizmos.DrawLine(QT.Position + new Vector3(-QT.Width, 0, -QT.Height), QT.Position + new Vector3(QT.Width, 0, -QT.Height));
            Gizmos.DrawLine(QT.Position + new Vector3(QT.Width, 0, QT.Height), QT.Position + new Vector3(QT.Width, 0, -QT.Height));
        }       
        public int getPlayerInBounds(SMPlayerPawn Pawn)
        {
           
            if(ParentNode.isObjectInBounds(Pawn.transform))
            return Pawn.ID;
            
            return -1;
        }
        IEnumerator reCalculate()
        {
            yield return new WaitForSeconds(1 / calculationsPerSecond);

            foreach (SMPlayerPawn Pawn in QuadTreeManager.Instance.Pawns)
            {
                playerInBounds = getPlayerInBounds(Pawn);

                if (playerInBounds != -1)
                {
                    ParentNode.Clear();
                   
                    QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects.Clear();
                    
                    foreach (Transform t in ShadowVolumes)
                    {
                        ParentNode.insertObject(t);
                    }

                    ParentNode.insertObject(QuadTreeManager.Instance.Pawns[playerInBounds].transform);

                    ParentNode.Verify();

                    
                    ParentNode.Retrieve(QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects, QuadTreeManager.Instance.Pawns[playerInBounds].transform);
                    if (QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects.Contains(QuadTreeManager.Instance.Pawns[playerInBounds].transform))
                    {
                        QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects.Remove(QuadTreeManager.Instance.Pawns[playerInBounds].transform);
                    }

                    if (QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects.Count > 0)
                    {
                        QuadTreeManager.Instance.Pawns[playerInBounds].CollidingObjects = ParentNode.checkCollisions(QuadTreeManager.Instance.Pawns[playerInBounds].NearObjects, QuadTreeManager.Instance.Pawns[playerInBounds].transform);
                        if (QuadTreeManager.Instance.Pawns[playerInBounds].CollidingObjects.Count > 0)
                        { 
                            QuadTreeManager.Instance.UpdateCollision(id, true, QuadTreeManager.Instance.Pawns[playerInBounds].ID, QuadTreeManager.Instance.Pawns[playerInBounds].CollidingObjects);
                        }
                        else
                        {
                            QuadTreeManager.Instance.UpdateCollision(id, false, QuadTreeManager.Instance.Pawns[playerInBounds].ID);
                        }
                    }
                    
                }
            }
            StartCoroutine(reCalculate());

        }

    }


