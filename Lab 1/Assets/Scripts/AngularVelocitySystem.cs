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
    struct RotationSpeedJob : IJobForEach<Rotation, Translation, AngularVelocityData>
    {
        public float DeltaTime;

        // The [ReadOnly] attribute tells the job scheduler that this job will not write to rotSpeedIJobForEach
        public void Execute(ref Rotation rotation, ref Translation translation, ref AngularVelocityData rotSpeedIJobForEach)
        {

            // Update position / velocity
            rotSpeedIJobForEach.velocity += rotSpeedIJobForEach.acceleration * DeltaTime;
            rotSpeedIJobForEach.position += rotSpeedIJobForEach.velocity * DeltaTime;

            // Update Acceleration
            rotSpeedIJobForEach.acceleration = rotSpeedIJobForEach.force * rotSpeedIJobForEach.invMass;

            translation.Value = rotSpeedIJobForEach.position;


            // calculate w
            Quaternion newRot = new Quaternion(rotSpeedIJobForEach.angularVelocity.x, rotSpeedIJobForEach.angularVelocity.y, rotSpeedIJobForEach.angularVelocity.z, 0);

            // wq dt/2
            Quaternion temp = (rotSpeedIJobForEach.rotation * newRot) * new Quaternion(0, 0, 0, (DeltaTime / 2.0f));

            // (q) + (wq dt/2) and normalize the result
            rotSpeedIJobForEach.rotation = new Quaternion(temp.x + rotSpeedIJobForEach.rotation.x, temp.y + rotSpeedIJobForEach.rotation.y, temp.z + rotSpeedIJobForEach.rotation.z, temp.w + rotSpeedIJobForEach.rotation.w).normalized;

            // Change Angular velocity as well
            rotSpeedIJobForEach.angularVelocity += rotSpeedIJobForEach.angularAcceleration * DeltaTime;

            // Update Angular acceleration as well
            rotSpeedIJobForEach.angularAcceleration = rotSpeedIJobForEach.inertiaTensor * rotSpeedIJobForEach.torque;

            rotation.Value = rotSpeedIJobForEach.rotation;
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
