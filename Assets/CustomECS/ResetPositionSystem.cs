using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ResetPositionSystem : ComponentSystem
{
	protected override void OnCreateManager(int capacity)
    {
		// ssoa = this;
        this.Enabled = false;
    }

    struct Data{
		public int Length;
		public ComponentDataArray<ResetPosition> resetPosition;
	}

	[Inject] private Data _data;

	struct CubeDataReturn{
		int Length;
		public ComponentDataArray<ResetPositionFlag> resetPositionFlag;
		public ComponentDataArray<LocalPosition> localPosition;
	}

	// [Inject] private CubeDataReturn _cubeDataReturn;

	// struct CubeData{
	// 	int Length;
	// 	public SubtractiveComponent<ResetPositionFlag> resetPositionFlag;
	// 	public ComponentDataArray<TransformParent> localPosition;
	// 	// public ComponentDataArray<Entity> entity;
	// }

	// [Inject] private CubeData _cubeData;

	[ComputeJobOptimization]
	struct AssignFlags : IJobProcessComponentData<TransformParent>{
		public EntityCommandBuffer ecb;
		public EntityArray ea;
				// [ReadOnly]public Quaternion _data;

		public void Execute( [ReadOnly]ref TransformParent tp){
			// ecb.CreateEntity();
		}
	}
	protected override void OnUpdate(){
		var job = new AssignFlags() { ecb = PostUpdateCommands
		// , _data = _data.rotation[0].Value
		};
		
	}
}
