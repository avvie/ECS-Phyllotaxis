using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem {
	struct Data{
		int Length;
		public ComponentDataArray<PlayerInput> input;
	}

	[Inject] private Data _data;

	protected override void OnUpdate(){
		if(Input.GetKeyDown(KeyCode.P)){
			SineSystemOnAxis.ssoa.Enabled = !SineSystemOnAxis.ssoa.Enabled;
		}
	}
	
}
