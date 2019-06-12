using Unity.Entities;

public struct PlayerTag : IComponentData { }

public class PlayerTagComponent : ComponentDataWrapper<PlayerTag> { }
