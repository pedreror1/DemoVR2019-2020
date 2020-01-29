using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dancingEnemiesObject : MonoBehaviour, IPooledObject
{

private void OnEnable()
{
born();
}
public void born()
{
throw new System.NotImplementedException();
}
public void Interaction()
{
throw new System.NotImplementedException();
}
public void Dead()
{
dancingEnemiesPool.Instance.returnToPool(this);
}
}