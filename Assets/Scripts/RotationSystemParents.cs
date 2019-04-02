using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystemParents : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotationSpeedParent { dt = Time.deltaTime };
        return job.Schedule(this, inputDeps);
    }

    // Use this for initialization
    [BurstCompile]
    private struct RotationSpeedParent : IJobProcessComponentData<Rotation, RotationData, RotationFocus>
    {
        public float dt;

        public void Execute(ref Rotation rotation, [ReadOnly] ref RotationData data, [ReadOnly] ref RotationFocus tp)
        {
            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), data.Value * dt));
        }
    }
}

