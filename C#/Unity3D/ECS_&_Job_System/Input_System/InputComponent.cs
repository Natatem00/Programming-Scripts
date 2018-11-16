using Unity.Entities;
using System;

[Serializable]
public struct InputData : IComponentData {

    public float vertical;
    public float horizontal;
}

public class InputComponent : ComponentDataWrapper<InputData> { }
