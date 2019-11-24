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
    struct RotationSpeedJob : IJobForEach<Translation, AngularVelocityData>
    {
        public float DeltaTime;

        // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
        public void Execute(ref Translation translation, ref AngularVelocityData rotSpeedIJobForEach)
        {
            // Update position / velocity
            rotSpeedIJobForEach.velocity += rotSpeedIJobForEach.acceleration * DeltaTime;
            rotSpeedIJobForEach.position += rotSpeedIJobForEach.velocity * DeltaTime;

            // Update Acceleration
            rotSpeedIJobForEach.acceleration = rotSpeedIJobForEach.force * rotSpeedIJobForEach.invMass;

            translation.Value = rotSpeedIJobForEach.position;
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
