using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Particle3DSystem : JobComponentSystem
{
    // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
    [BurstCompile]
    struct ParticleUpdateJob : IJobForEach<Rotation, Translation, Particle3DData>
    {
        public float DeltaTime;

        // This function is the update function for the particle 3D
        public void Execute(ref Rotation rotation, ref Translation translation, ref Particle3DData particleUpdateJob)
        {

            // Update position / velocity
            particleUpdateJob.velocity += particleUpdateJob.acceleration * DeltaTime;
            particleUpdateJob.position += particleUpdateJob.velocity * DeltaTime;

            // Update Acceleration
            particleUpdateJob.acceleration = particleUpdateJob.force * particleUpdateJob.invMass;

            // Update position values
            translation.Value = particleUpdateJob.position;


            // calculate w
            Quaternion newRot = new Quaternion(particleUpdateJob.angularVelocity.x, particleUpdateJob.angularVelocity.y, particleUpdateJob.angularVelocity.z, 0);

            // wq dt/2
            Quaternion temp = (particleUpdateJob.rotation * newRot) * new Quaternion(0, 0, 0, (DeltaTime / 2.0f));

            // (q) + (wq dt/2) and normalize the result
            particleUpdateJob.rotation = new Quaternion(temp.x + particleUpdateJob.rotation.x, temp.y + particleUpdateJob.rotation.y, temp.z + particleUpdateJob.rotation.z, temp.w + particleUpdateJob.rotation.w).normalized;

            // Change Angular velocity as well
            particleUpdateJob.angularVelocity += particleUpdateJob.angularAcceleration * DeltaTime;

            // Update Angular acceleration as well
            particleUpdateJob.angularAcceleration = particleUpdateJob.inertiaTensor * particleUpdateJob.torque;

            // Update rotational values
            rotation.Value = particleUpdateJob.rotation;
        }
    }

    // OnUpdate runs on the main thread.
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ParticleUpdateJob
        {
            DeltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDependencies);
    }
}
