using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public sealed class Bootstrap{

	public static EntityArchetype CubeSpawner;
	public static EntityArchetype Cube;
	public static EntityArchetype rotationFocus;
	public static EntityArchetype InputLayer;

	public static Settingscs Settings;
	public static Entity transform;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize(){
		Debug.Log("init");
		EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        Cube = entityManager.CreateArchetype(typeof(LocalPosition), typeof(CubeComp),
                typeof(MeshInstanceRenderer), typeof(TransformMatrix), typeof(LocalRotation), typeof(RotationSpeed), typeof(TransformParent));

		rotationFocus = World.Active.GetOrCreateManager<EntityManager>().CreateArchetype(typeof(Position), typeof(Rotation)
				, typeof(RotationSpeed), typeof(TransformMatrix), typeof(RotationFocus));

        CubeSpawner = entityManager.CreateArchetype(typeof(Position), typeof(Radius));
		
		InputLayer = entityManager.CreateArchetype(typeof(PlayerInput));

        entityManager.CreateEntity(CubeSpawner);
		entityManager.CreateEntity(InputLayer);

		transform = entityManager.CreateEntity(rotationFocus);
		entityManager.SetComponentData<Position>(transform, new Position {Value = new float3(0, 0, 0)});
		entityManager.SetComponentData<RotationSpeed>(transform, new RotationSpeed { Value = 5});
        // Cube = entityManager.CreateArchetype(typeof(Position), typeof(Heading));
		
        // entityManager.CreateEntity(CubeSpawner);
    }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public static void InitializeWithScene(){
		Debug.Log("InitializeWithScene");
		Settings = GameObject.Find("Settings").GetComponent<Settingscs>();
		
	}
}
