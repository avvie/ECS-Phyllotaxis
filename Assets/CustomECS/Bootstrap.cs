using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;

public sealed class Bootstrap{

	public static EntityArchetype CubeSpawner;
	public static EntityArchetype Cube;

	public static Settingscs Settings;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize(){
		Debug.Log("init");
		EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();

		CubeSpawner = entityManager.CreateArchetype(typeof(Position), typeof(Radius));

		// Cube = entityManager.CreateArchetype(typeof(Position), typeof(Heading));
		// entityManager.CreateEntity(CubeSpawner);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public static void InitializeWithScene(){
		Debug.Log("InitializeWithScene");
		Settings = GameObject.Find("Settings").GetComponent<Settingscs>();
		World.Active.GetOrCreateManager<EntityManager>().CreateEntity(CubeSpawner);
	}
}
