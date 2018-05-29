using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystemParents : JobComponentSystem {

	// Use this for initialization
	[ComputeJobOptimization]
    struct RotationSpeedParent : IJobProcessComponentData<TransformMatrix, Rotation, RotationFocus> {
        public float dt;
        // public float4x4 pM;
        public void Execute(ref TransformMatrix tranformMatrix, ref Rotation rotation, [ReadOnly]ref RotationFocus tp) {
            quaternion q = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), 2 * dt));
            float4x4 m = tranformMatrix.Value;

            float x = q.value.x * 2.0F;
            float y = q.value.y * 2.0F;
            float z = q.value.z * 2.0F;
            float xx = q.value.x * x;
            float yy = q.value.y * y;
            float zz = q.value.z * z;
            float xy = q.value.x * y;
            float xz = q.value.x * z;
            float yz = q.value.y * z;
            float wx = q.value.w * x;
            float wy = q.value.w * y;
            float wz = q.value.w * z;

            m.m0.x = 1.0f - (yy + zz); m.m1.x = xy + wz; m.m2.x = xz - wy; m.m3.x = 0.0F;
            m.m0.y = xy - wz; m.m1.y = 1.0f - (xx + zz); m.m2.y = yz + wx; m.m3.y = 0.0F;
            m.m0.z = xz + wy; m.m1.z = yz - wx; m.m2.z = 1.0f - (xx + yy); m.m3.z = 0.0F;
            m.m0.w = 0.0F; m.m1.w = 0.0F; m.m2.w = 0.0F; m.m3.w = 1.0F;
            
            rotation.Value = q;
            tranformMatrix.Value = m;
            // rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt ));
        }
    }

	protected override JobHandle OnUpdate(JobHandle inputDeps){
		var job = new RotationSpeedParent() { dt = Time.deltaTime};
		return job.Schedule(this, 64, inputDeps);
	}
}
