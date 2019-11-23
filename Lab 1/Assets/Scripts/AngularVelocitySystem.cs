using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class AngularVelocitySystem : JobComponentSystem
{
    // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
    [BurstCompile]
    struct RotationSpeedJob : IJobForEach<AngularVelocityData>
    {
        public float DeltaTime;

        // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
        public void Execute(ref AngularVelocityData rotSpeedIJobForEach)
        {
            rotSpeedIJobForEach.acceleration += new Vector3(1, 0, 0);
        }
    }

    // OnUpdate runs on the main thread.
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new RotationSpeedJob
        {
            DeltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDependencies);
    }
}
