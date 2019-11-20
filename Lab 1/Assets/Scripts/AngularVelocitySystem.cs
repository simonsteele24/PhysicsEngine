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
    struct RotationSpeedJob : IJobForEach<Rotation, AngularVelocityData>
    {
        public float DeltaTime;

        // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
        public void Execute(ref Rotation rotation, [ReadOnly] ref AngularVelocityData rotSpeedIJobForEach)
        {
            //rotSpeedIJobForEach.angularAcceleration = rotSpeedIJobForEach.inertiaTensor * rotSpeedIJobForEach.torque;
            //rotSpeedIJobForEach.torque.Set(0.0f, 0.0f, 0.0f);

            Quaternion newRot = new Quaternion(rotSpeedIJobForEach.angularVelocity.x, rotSpeedIJobForEach.angularVelocity.y, rotSpeedIJobForEach.angularVelocity.z, 0);

            // wq dt/2
            Quaternion temp = (rotation.Value * newRot) * new Quaternion(0, 0, 0, (DeltaTime / 2.0f));

            // (q) + (wq dt/2) and normalize the result
            newRot = rotation.Value;

            rotation.Value = new Quaternion(temp.x + newRot.x, temp.y + newRot.y, temp.z + newRot.z, temp.w + newRot.w).normalized;

            // Change Angular velocity as well
            rotSpeedIJobForEach.angularVelocity += rotSpeedIJobForEach.angularAcceleration * DeltaTime;
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
