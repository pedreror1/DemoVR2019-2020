using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] [ExecuteInEditMode]
public class QuadTree
{
    public Vector3 Position;
    public float Width = 250;
    public float Height = 250;
    public int Level = 0;
    public int MaxLevel = 5;
    public int MaxObjectsPerNode = 5;
    public List<QuadTree> Nodes = new List<QuadTree>();
    public List<Transform> Objects = new List<Transform>();
    public int NumChildObjs = 0;


    public void Clear()
    {
        Objects.Clear();
        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].Clear();
        }

    }
    public int getChildObjectsCount()
    {
        int numOfChilds = 0;
        foreach (QuadTree q2 in Nodes)
        {
            numOfChilds+=q2.getChildObjectsCount();
          
        }
        numOfChilds += Objects.Count;
        return numOfChilds;
    }
    public void ClearNode()
    {         
        foreach (QuadTree q2 in Nodes)
        {
            if (q2.Objects.Count > 0)
            {
                Objects.AddRange(q2.Objects);
            }
        }         
        Nodes.Clear();
    }
    public bool isObjectInBounds(Transform obj)
    {
        if (obj.position.x >= (Position.x - Width) &&
            obj.position.x <= (Position.x + Width) &&
            obj.position.z >= (Position.z - Height) &&
            obj.position.z <= (Position.z + Height))
        {
            return true;
        }

        return false;
    }

    public QuadTree(Vector3 position, float width, float height,int maxLevel, int maxObjectsPerNode, int level)
    {
        this.Position = position;
        this.Width = width;
        this.Height = height;
        this.MaxLevel = maxLevel;
        this.MaxObjectsPerNode = maxObjectsPerNode;
        this.Level = level;
    }

    public List<QuadTree> Split()
    {
        if (Level < MaxLevel)
        {
            float subWidth = (Width / 2f);
            float subHeight = (Height / 2f);
            Nodes.Add(new QuadTree(Position + new Vector3(subWidth, 0, subHeight), subWidth, subHeight,MaxLevel, MaxObjectsPerNode, Level + 1));
            Nodes.Add(new QuadTree(Position + new Vector3(-subWidth, 0, subHeight), subWidth, subHeight, MaxLevel, MaxObjectsPerNode, Level + 1));
            Nodes.Add(new QuadTree(Position + new Vector3(-subWidth, 0, -subHeight), subWidth, subHeight, MaxLevel, MaxObjectsPerNode, Level + 1));
            Nodes.Add(new QuadTree(Position + new Vector3(subWidth, 0, -subHeight), subWidth, subHeight, MaxLevel, MaxObjectsPerNode, Level + 1));
            return Nodes;
        }

        return null;
    }
    public List<Transform> Retrieve(List<Transform> objectList, Transform Volume)
    {
        int index = getCuadrantIndex(Volume);
        if (index != -1)
        {
            if (Nodes.Count > 0)
            {
                Nodes[index].Retrieve(objectList, Volume);
            }

            objectList.AddRange(Objects);
        }
        return objectList;
    }

    public int getCuadrantIndex(Transform Volume)
    {
        float VMidPoint = Position.x;
        float HMidPoint = Position.z;

        int index = -1;
        bool topQuad = (Volume.position.z > HMidPoint && Volume.position.z < HMidPoint + Height);
        bool bottomQuad = (Volume.position.z < HMidPoint && Volume.position.z > HMidPoint - Height);

        if (Volume.position.x < VMidPoint && Volume.position.x > VMidPoint - Width)
        {
            if (topQuad)
            {
                index = 1;
            }
            else if (bottomQuad)
            {
                index = 2;
            }
        }
        else if (Volume.position.x > VMidPoint && Volume.position.x < VMidPoint + Width)
        {
            if (topQuad)
            {
                index = 0;
            }
            else if (bottomQuad)
            {
                index = 3;
            }
        }
       
        return index;

    }
    public void Verify()
    {
        NumChildObjs = getChildObjectsCount();
        foreach (QuadTree q in Nodes)
        {
            q.Verify();
        }

        if (Nodes.Count == 4)
        {
            if (NumChildObjs <= MaxObjectsPerNode)
            {
                ClearNode();
            }
        }
    }
    public void insertObject(Transform Volume)
    {
        if (Nodes.Count>0)
        {           
                int index = getCuadrantIndex(Volume);
                if (index != -1)
                {
                    Nodes[index].insertObject(Volume);
                    return;
                }
            
        }
        Objects.Add(Volume);
        NumChildObjs = getChildObjectsCount();
        if (Objects.Count > MaxObjectsPerNode && Level < MaxLevel)
        {
            if (Nodes.Count == 0) 
            {
                Split();
            }
            int i = 0;
            while (i < Objects.Count)
            {
                int index = getCuadrantIndex(Objects[i]);
                if (index != -1)
                {
                    Transform t = Objects[i];
                    Nodes[index].insertObject(Objects[i]);
                    Objects.Remove(t);
                }
                else
                {
                    i++;
                }

            }
            NumChildObjs = getChildObjectsCount();
        }
         

    }

    public List<Transform> checkCollisions(List<Transform> nearObjects, Transform playerPawn)
    {
        List<Transform> collidingObjects = new List<Transform>();
        foreach(Transform g in nearObjects)
        {
            CollidingObject b = g.GetComponentInChildren<CollidingObject>();
            if (b != null)
            {
                if (b.Intersects(playerPawn))
                {
                    collidingObjects.Add(g);
                }
            }
            
            
        }
        return collidingObjects;
    }
}
