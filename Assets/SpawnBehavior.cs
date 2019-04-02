using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject Prefab;
    public int RotationSpeed = 2;
    public float Radius = 100;
    public int NumCubes = 50000;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new SpawnerSettings
        {     
            Prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active),
            Radius = Radius,
            NumCubes = NumCubes,
        });
    }
}

public struct SpawnerSettings : IComponentData
{    
    public Entity Prefab;
    public float Radius;
    public int NumCubes;
}

//public class TranslationSystem : JobComponentSystem
//{
//    protected override void OnCreateManager()
//    {

//    }

//    private readonly float Segment = math.radians((float)137.51);

//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {
//        return inputDeps;

//        var job = new TranslationSystemTask
//        {
//            Segment = Segment,
//            dt = Time.deltaTime,
//        };
//        return job.Schedule(this, inputDeps);
//    }

//    [BurstCompile]
//    private struct TranslationSystemTask : IJobProcessComponentData<Translation, RotationData>
//    {
//        [ReadOnly] public float dt;
//        private float3 point;
//        public float Segment;

//        public void Execute(ref Translation position, [ReadOnly] ref RotationData data)
//        {
//            position.Value = new float3(
//                            data.Radius * math.sin(data.Index * Segment + dt) * math.cos(0),
//                            data.Radius * math.sin(0) * math.sin(data.Index * Segment + dt),
//                            data.Radius * math.cos(data.Index * Segment + dt));

            
//            //point = position.Value;
//            //var distanceFromCenter = math.distance(position.Value, originPoint);
//            //point.y = point.y + math.sin(distanceFromCenter + temp) * 25;
//            //position.Value = math.lerp(position.Value, point, dt * Const);
//        }
//    }
//}



// public class CubeSpawnSystem : JobComponentSystem {
public class CubeSpawnSystem : ComponentSystem
{
    public EntityArchetype Cube;
    public EntityArchetype CubeAttach;
    private ComponentGroup _settingsGroup;
    private ComponentGroup _focusGroup;

    protected override void OnCreateManager()
    {
        _settingsGroup = GetComponentGroup(new EntityArchetypeQuery
        {
            All = new ComponentType[]
            {
                typeof(SpawnerSettings)              
            }
        });

        _focusGroup = GetComponentGroup(new EntityArchetypeQuery
        {
            All = new ComponentType[]
            {
                typeof(Translation),
                typeof(RotationData),
                typeof(RotationFocus)
            }
        });
    }

    protected override void OnUpdate()
    {
        var settings = GetSingleton<SpawnerSettings>();
        var centerEntity = _focusGroup.GetSingletonEntity();

        //var focusTranslation = EntityManager.GetComponentData<Translation>(focusEntity);
 
        //var ccComponentType = GetArchetypeChunkComponentType<CharacterControllerComponentData>();
        //var translationType = GetArchetypeChunkComponentType<Translation>();

        //var rotationType = GetArchetypeChunkComponentType<Rotation>();

        var segment = math.radians((float)137.51);
        //float radius = settings.Radius;

        for (var i = 0; i < settings.NumCubes; i++)
        {
            var radius = 1.3f * math.sqrt(i);

            var cubeEntity = EntityManager.Instantiate(settings.Prefab);

            float3 pos = new float3(0, 0, 0) + new float3(
                             radius * math.sin(i * segment + Time.deltaTime) * math.cos(0),
                             radius * math.sin(0) * math.sin(i * segment + Time.deltaTime),
                             radius * math.cos(i * segment + Time.deltaTime));

            EntityManager.SetComponentData(cubeEntity, new Translation
            {
                Value = pos
            });

            EntityManager.AddComponentData(cubeEntity, new RotationData
            {
                Value = 2, 
            });

            // The child translation is happening automatically when the entity in the center is rotated.
            // It can only work because each cube has the centerEntity as parent,
            // ECS internally updates all the child the positions automatically. 

            EntityManager.AddComponentData(cubeEntity, new Parent { Value = centerEntity });
            EntityManager.AddComponentData(cubeEntity, new LocalToParent { Value = float4x4.identity });
        }

        Enabled = false;
    }

    //public unsafe struct TestJob : IJobChunk
    //{
    //    public ArchetypeChunkComponentType<Translation> Translations;
    //    public ArchetypeChunkComponentType<VoxelChunk> VoxelChunks;

    //    public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
    //    {
    //        if (chunk.Has(VoxelChunks))
    //        {
    //            using (var translationsArr = chunk.GetNativeArray(Translations))
    //            using (var voxelChunksArr = chunk.GetNativeArray(VoxelChunks))
    //            {
    //                void* translations = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(translationsArr);
    //                void* voxelChunks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(voxelChunksArr);

    //                for (int i = 0; i < voxelChunksArr.Length; i++)
    //                {
    //                    ref var voxelChunk = ref UnsafeUtilityEx.ArrayElementAsRef<VoxelChunk>(voxelChunks, i);
    //                    ref var translation = ref UnsafeUtilityEx.ArrayElementAsRef<Translation>(translations, i);

    //                    translation.Value.x = 5;
    //                    translation.Value.z = 8;
    //                }
    //            }
    //        }
    //    }
    //}

}
