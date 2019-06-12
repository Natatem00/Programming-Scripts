using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class PlayerRotationSystem : JobComponentSystem {

    protected override JobHandle OnUpdate(JobHandle inDepth)
    {
        float3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        int layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, layer))
        {
            inDepth.Complete();
            var Job = new Job();
            Job.pos = (float3)hit.point;
            return Job.Schedule(this);
        }
        return inDepth;
    }

    struct Job : IJobProcessComponentData<Rotation, Position, PlayerTag>
    {
        public float3 pos;
        public void Execute(ref Rotation rotation, [ReadOnly]ref Position position, [ReadOnly]ref PlayerTag tag)
        {
            float3 direction = pos - position.Value;
            var rot = Quaternion.LookRotation(direction);
            rot.x = rot.z = 0;
            rotation.Value = rot;
        }

    }
}
