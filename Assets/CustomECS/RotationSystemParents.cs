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
	struct RotationSpeedParent : IJobProcessComponentData<Rotation, RotationSpeed, RotationFocus>{
		public float dt;

		public void Execute(ref Rotation rotation, [ReadOnly]ref RotationSpeed speed, [ReadOnly]ref RotationFocus tp){
			
			rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt ));
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps){
		var job = new RotationSpeedParent() { dt = Time.deltaTime};
		return job.Schedule(this, 64, inputDeps);
	}
}
