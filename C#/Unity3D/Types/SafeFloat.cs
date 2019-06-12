using UnityEngine;

struct SafeFloat
{
    float value;
    float salt;

    public SafeFloat(float value)
    {
        salt = Random.Range(float.MinValue * 0.25f, float.MaxValue * 0.25f);
        this.value = Mathf.Sqrt(value) * salt;
    }

    public override bool Equals(object obj)
    {
        return (float)this == (float)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return ((float)this).ToString();
    }

    public static implicit operator float(SafeFloat safeInt)
    {
        return Mathf.Pow(safeInt.value, 2) / safeInt.salt;
    }

    public static implicit operator SafeFloat(float normalInt)
    {
        return new SafeFloat(normalInt);
    }
}

