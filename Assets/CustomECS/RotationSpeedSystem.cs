using System;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

[UpdateAfter(typeof(RotationSystemParents))]
public class RotationSpeedSystem : ComponentSystem {

	struct Data{
		public int Length;
        public ComponentDataArray<LocalPosition> Position;
		public ComponentDataArray<LocalRotation> rotation;
        // public ComponentDataArray<RotationFocus> rotationFocus;
		public ComponentDataArray<TransformMatrix> transformMatrix;
		public ComponentDataArray<TransformParent> transParent;
	}

	[Inject] Data _data;

	[ComputeJobOptimization]
	struct RotationSpeedRotation : IJobParallelFor{
		public float dt;
		// [ReadOnly]public Quaternion _data;
		public float4x4 pM;
		public ComponentDataArray<LocalPosition> position;
		public ComponentDataArray<TransformMatrix> transformMatrix;
		public ComponentDataArray<LocalRotation> rotation;

		public void Execute(int i){
			
			// rotation.Value = math.mul(math.normalize(rotation.Value), math.axisAngle(math.up(), speed.Value * dt ));
			quaternion q = math.mul(math.normalize(rotation[i].Value), math.axisAngle(math.up(), 2 * dt));
            float4x4 m = transformMatrix[i].Value;
			float4x4 p = transformMatrix[i].Value;
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

			p.m0.x = 1.0f; p.m1.x = 0; p.m2.x = 0; p.m3.x = position[i].Value.x;
            p.m0.y = 1; p.m1.y = 1.0f; p.m2.y = 0; p.m3.y = position[i].Value.y;
            p.m0.z = 0; p.m1.z =0; p.m2.z = 1.0f; p.m3.z = position[i].Value.z;
            p.m0.w = 0; p.m1.w = 0; p.m2.w = 0; p.m3.w = 1.0F;


            m.m0.x = 1.0f - (yy + zz); m.m1.x = xy + wz; m.m2.x = xz - wy; m.m3.x = 0.0F;
            m.m0.y = xy - wz; m.m1.y = 1.0f - (xx + zz); m.m2.y = yz + wx; m.m3.y = 0.0F;
            m.m0.z = xz + wy; m.m1.z = yz - wx; m.m2.z = 1.0f - (xx + yy); m.m3.z = 0.0F;
            m.m0.w = 0; m.m1.w = 0; m.m2.w = 0; m.m3.w = 1.0F;

			float4 newPos = math.mul(m, new float4 (position[i].Value.x, position[i].Value.y, position[i].Value.z,1));
			position[i] = new LocalPosition{Value = new float3 (newPos.x, newPos.y, newPos.z)};
			// m.m0.w = newPos.x; m.m1.w = newPos.y; m.m2.w = newPos.z;
            m =  math.mul(m,p);
			rotation[i] = new LocalRotation {Value = q};
			position[i] = new LocalPosition{Value = new float3 (position[i].Value.x, position[i].Value.y, position[i].Value.z)};
			transformMatrix[i] = new TransformMatrix{Value = m};
		}
	}

	protected override void OnUpdate(){
		// position = new NativeArray<float3>(Bootstrap.Settings.nbOfCubes, Allocator.TempJob);
		// rotation = new NativeArray<quaternion>(Bootstrap.Settings.nbOfCubes, Allocator.TempJob);
		// transformMatrix = new NativeArray<float4x4>(Bootstrap.Settings.nbOfCubes, Allocator.TempJob);

		// this.Enabled = false;
		// for(int i = 0; i < _data.Length; i++){
		// 	position[i] = _data.Position[i].Value;
		// 	transformMatrix[i] = _data.transformMatrix[i].Value;
		// 	rotation[i] = _data.rotation[i].Value;
		// }
		var job = new RotationSpeedRotation() {dt = Time.deltaTime, 
		pM = Bootstrap.entityManager.GetComponentData<TransformMatrix>(Bootstrap.transform).Value,
		position = _data.Position,
		rotation = _data.rotation,
		transformMatrix = _data.transformMatrix
		// , _data = _data.rotation[0].Value
		};
		job.Schedule(_data.Length, 64).Complete();
		
		// for(int i = 0; i < _data.Length; i++){
		// 	position[i] = _data.Position[i].Value;
		// 	transformMatrix[i] = _data.transformMatrix[i].Value;
		// 	_data.rotation[i].Value = rotation[i];
		// }

		// position.Dispose();
		// rotation.Dispose();
		// transformMatrix.Dispose();
	}
	
}
