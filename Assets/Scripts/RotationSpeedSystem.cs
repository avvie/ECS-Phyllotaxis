using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


public class RotationSpeedSystem : JobComponentSystem
{
    [BurstCompile]
    private struct RotationSpeedRotation : IJobProcessComponentData<Rotation, RotationData>
    {
        public float dt;

        public void Execute(ref Rotation rotation, [ReadOnly] ref RotationData data)
        {
            //if (data.Radius > 0)
            //{
                rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), data.Value * dt));
            //}
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new RotationSpeedRotation
        {
            dt = Time.deltaTime

        }.Schedule(this, inputDeps);
    }
}


