using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

public class PlayerInputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputJob)
    {
        base.OnUpdate(inputJob);
        var Job = new Job
        {
            Horizontal = Input.GetAxis("Horizontal"),
            Vertical = Input.GetAxis("Vertical")
        };


        return Job.Schedule(this);
    }

    private struct Job : IJobProcessComponentData<InputData>
    {
        public float Horizontal;
        public float Vertical;

        public void Execute(ref InputData inputData)
        {
            inputData.horizontal = Horizontal;
            inputData.vertical = Vertical;
        }
    }
}
