using Unity.Entities;
using System;

[Serializable]
public struct SpeedData : IComponentData {

    public float speed;
}

public class SpeedComponent : ComponentDataWrapper<SpeedData> { }