using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] [ExecuteInEditMode]
public class QuadTree
{
    public Vector3 position;
    public float width = 250;
    public float height = 250;
    public int level = 0;
    public int MaxLevel = 5;
    public int maxObjectsPerNode = 5;
    public List<QuadTree> nodes = new List<QuadTree>();
    public List<Transform> Objects = new List<Transform>();
    public int numchildobjs = 0;


    public void Clear()
    {
        Objects.Clear();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Clear();
        }

    }
    public int getChildObjectsCount()
    {
        int numOfChilds = 0;
        foreach (QuadTree q2 in nodes)
        {
            numOfChilds+=q2.getChildObjectsCount();
          
        }
        numOfChilds += Objects.Count;
        return numOfChilds;
    }
    public void ClearNode()
    {         
        foreach (QuadTree q2 in nodes)
        {
            if (q2.Objects.Count > 0)
            {
                Objects.AddRange(q2.Objects);
            }
        }         
        nodes.Clear();
    }
    public bool isObjectInBounds(Transform obj)
    {
        if (obj.position.x >= (position.x - width) &&
            obj.position.x <= (position.x + width) &&
            obj.position.z >= (position.z - height) &&
            obj.position.z <= (position.z + height))
        {
            return true;
        }

        return false;
    }

    public QuadTree(Vector3 position, float width, float height,int maxLevel, int maxObjectsPerNode, int level)
    {
        this.position = position;
        this.width = width;
        this.height = height;
        this.MaxLevel = maxLevel;
        this.maxObjectsPerNode = maxObjectsPerNode;
        this.level = level;
    }

    public List<QuadTree> Split()
    {
        if (level < MaxLevel)
        {
            float subWidth = (width / 2f);
            float subHeight = (height / 2f);
            nodes.Add(new QuadTree(position + new Vector3(subWidth, 0, subHeight), subWidth, subHeight,MaxLevel, maxObjectsPerNode, level + 1));
            nodes.Add(new QuadTree(position + new Vector3(-subWidth, 0, subHeight), subWidth, subHeight, MaxLevel, maxObjectsPerNode, level + 1));

            nodes.Add(new QuadTree(position + new Vector3(-subWidth, 0, -subHeight), subWidth, subHeight, MaxLevel, maxObjectsPerNode, level + 1));
            nodes.Add(new QuadTree(position + new Vector3(subWidth, 0, -subHeight), subWidth, subHeight, MaxLevel, maxObjectsPerNode, level + 1));
            return nodes;
        }

        return null;
    }
    public List<Transform> Retrieve(List<Transform> objectList, Transform Volume)
    {
        int index = getCuadrantIndex(Volume);
        if (index != -1)
        {
            if (nodes.Count > 0)
            {
                nodes[index].Retrieve(objectList, Volume);
            }

            objectList.AddRange(Objects);
        }
        return objectList;
    }

    public int getCuadrantIndex(Transform Volume)
    {
        float VMidPoint = position.x;
        float HMidPoint = position.z;

        int index = -1;
        bool topQuad = (Volume.position.z > HMidPoint && Volume.position.z < HMidPoint + height);
        bool bottomQuad = (Volume.position.z < HMidPoint && Volume.position.z > HMidPoint - height);

        if (Volume.position.x < VMidPoint && Volume.position.x > VMidPoint - width)
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
        else if (Volume.position.x > VMidPoint && Volume.position.x < VMidPoint + width)
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
        numchildobjs = getChildObjectsCount();
        foreach (QuadTree q in nodes)
        {
            q.Verify();
        }

        if (nodes.Count == 4)
        {
            if (numchildobjs <= maxObjectsPerNode)
            {
                ClearNode();
            }
        }
    }
    public void insertObject(Transform Volume)
    {
        if (nodes.Count>0)
        {           
                int index = getCuadrantIndex(Volume);
                if (index != -1)
                {
                    nodes[index].insertObject(Volume);
                    return;
                }
            
        }
        Objects.Add(Volume);
        numchildobjs = getChildObjectsCount();
        if (Objects.Count > maxObjectsPerNode && level < MaxLevel)
        {
            if (nodes.Count == 0) 
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
                    nodes[index].insertObject(Objects[i]);
                    Objects.Remove(t);
                }
                else
                {
                    i++;
                }

            }
            numchildobjs = getChildObjectsCount();
        }
         

    }

    public List<Transform> checkCollisions(List<Transform> nearObjects, Transform playerPawn)
    {
        List<Transform> collidingObjects = new List<Transform>();
        foreach(Transform g in nearObjects)
        {
            ShadeVolume b = g.GetComponentInChildren<ShadeVolume>();
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
