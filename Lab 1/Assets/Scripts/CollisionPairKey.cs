using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public class CollisionPairKey : IEqualityComparer<CollisionPairKey>
{
    private CollisionHullType2D[] _collTypes = new CollisionHullType2D[2];
    public CollisionPairKey(CollisionHullType2D collTypeA, CollisionHullType2D collTypeB)
    {
        if (collTypeA <= collTypeB)
        {
            _collTypes[0] = collTypeA;
            _collTypes[1] = collTypeB;
        }
        else
        {
            _collTypes[0] = collTypeB;
            _collTypes[1] = collTypeA;
        }
    }

    public bool Equals(CollisionPairKey x, CollisionPairKey y)
    {
        return x._collTypes.Equals(y._collTypes);
    }

    public int GetHashCode(CollisionPairKey obj)
    {
        return obj.GetHashCode();
    }
}
