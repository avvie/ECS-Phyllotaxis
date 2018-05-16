using System;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public class RotationSpeedSystem : JobComponentSystem {

	[ComputeJobOptimization]
	struct RotationSpeedRotation : IJobProcessComponentData<Rotation, RotationSpeed>{
		public float dt;

		public void Execute(ref Rotation rotation, [ReadOnly]ref RotationSpeed speed){
			rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt ));
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps){
		var job = new RotationSpeedRotation() { dt = Time.deltaTime};
		return job.Schedule(this, 64, inputDeps);
	}
	
}
