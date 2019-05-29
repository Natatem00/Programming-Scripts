using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SafeInt
{
    int value;
    int salt;

    public SafeInt(int value)
    {
        salt = (int)Random.Range(int.MinValue * 0.25f, int.MaxValue * 0.25f);
        this.value = value ^ salt;
    }

    public override bool Equals(object obj)
    {
        return (int)this == (int)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return ((int)this).ToString();
    }

    public static implicit operator int(SafeInt safeInt)
    {
        return safeInt.value ^ safeInt.salt;
    }

    public static implicit operator SafeInt(int normalInt)
    {
        return new SafeInt(normalInt);
    }
}

