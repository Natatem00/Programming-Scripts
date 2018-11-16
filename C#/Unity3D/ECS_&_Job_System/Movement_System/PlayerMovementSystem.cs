using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

public class PlayerMovementSystem : JobComponentSystem {

    protected override JobHandle OnUpdate(JobHandle inputDepth)
    {
        var Job = new Job();
        Job.time = Time.deltaTime;
        return Job.Schedule(this, inputDepth);
    }

    private struct Job : IJobProcessComponentData<Position, InputData, SpeedData, PlayerTag>
    {
        [ReadOnly]
        public float time;

        public void Execute(ref Position positionData, [ReadOnly]ref InputData inputData, [ReadOnly]ref SpeedData speedData, [ReadOnly]ref PlayerTag tag)
        {
            positionData.Value += new float3(inputData.horizontal, 0, inputData.vertical) * speedData.speed * time;
        }
    }
}
