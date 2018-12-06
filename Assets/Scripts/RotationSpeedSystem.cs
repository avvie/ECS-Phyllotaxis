using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


public class RotationSpeedSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var job = new RotationSpeedRotation
        {
            dt = Time.deltaTime
        };
        job.Schedule(this).Complete();
        var job2 = new BoundUpdateJob { };
        //job2.Schedule(this).Complete();

    }

    [BurstCompile]
    private struct RotationSpeedRotation : IJobProcessComponentData<Rotation, RotationSpeed, Parent>
    {
        public float dt;

        public void Execute(ref Rotation rotation, [ReadOnly] ref RotationSpeed speed, [ReadOnly] ref Parent tp)
        {
            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), speed.Value * dt));
        }
    }

    [BurstCompile]
    private struct BoundUpdateJob : IJobProcessComponentData<Position, Rotation, MeshRenderBounds> {

        public void Execute([ReadOnly] ref Position pos, [ReadOnly] ref Rotation rot, ref MeshRenderBounds mrb) {

            mrb = new MeshRenderBounds {
                Center = math.mul(rot.Value, pos.Value).xyz,
                Radius = mrb.Radius
            };
        }
    }

    
}
