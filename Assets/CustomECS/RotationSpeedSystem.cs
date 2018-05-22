using System;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

[UpdateAfter(typeof(RotationSystemParents))]
public class RotationSpeedSystem : ComponentSystem {

	// struct Data{
	// 	public int Length;
    //     [ReadOnly] public ComponentDataArray<Position> Position;
	// 	[ReadOnly] public ComponentDataArray<Rotation> rotation;
    //     [ReadOnly] public ComponentDataArray<RotationFocus> rotationFocus;
	// }

	// [Inject] Data _data;

	[ComputeJobOptimization]
	struct RotationSpeedRotation : IJobProcessComponentData<LocalRotation, RotationSpeed, TransformParent>{
		public float dt;
		// [ReadOnly]public Quaternion _data;

		public void Execute(ref LocalRotation rotation, [ReadOnly]ref RotationSpeed speed, [ReadOnly]ref TransformParent tp){
			
			rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt ));
		}
	}

	protected override void OnUpdate(){
		var job = new RotationSpeedRotation() { dt = Time.deltaTime
		// , _data = _data.rotation[0].Value
		};
		job.Schedule(this, 64).Complete();
	}
	
}
