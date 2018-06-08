using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SineSystemOnAxis : JobComponentSystem {
	public static SineSystemOnAxis ssoa;
	float3 oriP = new float3(0,0,0);
	float temp = 0;
	int sign = -1;

	[ComputeJobOptimization]
	struct SineSystem : IJobProcessComponentData<LocalPosition, RotationSpeed>{ 
		[ReadOnly]public float3 originPoint;
		[ReadOnly]public float temp;
		[ReadOnly]public float dt;
		[ReadOnly]public int sign;
		[ReadOnly]public float Const;
		float3 point;
		public void Execute(ref LocalPosition position, [ReadOnly]ref RotationSpeed speed){
			// float pos = new float3(position.Value.x, position.Value.y, position.Value.z);
			point = position.Value;
			float distanceFromCenter = math.distance(position.Value, originPoint);
			point.y =  point.y +( math.sin(distanceFromCenter + temp )) * 25;
			
			position.Value = math.lerp(position.Value, point, dt * Const);
		}
	}

	protected override void OnCreateManager(int capacity)
    {
		ssoa = this;
        this.Enabled = false;
    }

	protected override JobHandle OnUpdate(JobHandle inputDeps){
		if(temp % 100000 == 0)
			sign = sign*-1;
		var job = new SineSystem() { originPoint = oriP, temp = temp, sign = sign, dt = Time.deltaTime, Const = Bootstrap.Settings.lerpFact};
		temp = temp + Time.deltaTime;
		return job.Schedule(this, 64, inputDeps);
	}
	
}
