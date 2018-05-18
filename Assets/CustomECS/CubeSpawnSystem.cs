using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

// public class CubeSpawnSystem : JobComponentSystem {
    
public class CubeSpawnSystem : ComponentSystem {
    public struct CubeSp{
        public int Length;
        [ReadOnly] public ComponentDataArray<Position> Position;
        [ReadOnly] public ComponentDataArray<Radius> RadiusComponent;
         public EntityArray Entities;
    }

    // NativeArray<MeshInstanceRenderer> mira;

    [Inject] CubeSp _Cubesp;
    // public struct CubeData{
    //     public int Length;
    //     public ComponentDataArray<Position> Position;
    //     public ComponentDataArray<CubeComp> cubeComp;
    // }

    // [Inject] private CubeData m_CubeData;

    public EntityArchetype Cube;


    // struct SpawnCubes : IJob {
    //     [ReadOnly]public float count;
    //     [ReadOnly]public float radius;
    //     public EntityCommandBuffer CommandBuffer;
    //     [ReadOnly]public EntityArchetype Cube;
    //     [ReadOnly]public MeshInstanceRenderer mir;
    //     public void Execute(){
            
    //         for (int i = 0; i < count; i++){
    //             // Debug.Log(i);
    //             CommandBuffer.CreateEntity(Cube);
    //             CommandBuffer.SetSharedComponent<MeshInstanceRenderer>(mir);
    //         }
    //     }
    // }

    

    // [Inject] EndFrameBarrier _endFrameBarrier;
    // var MIR;

    public GameObject obj;
    int localCount = 0;

    protected override void OnUpdate(){
        // Debug.Log(_Cubesp.Length);
        if(localCount >= Bootstrap.Settings.nbOfCubes - 1){
            // Debug.Log("ti fash ");
            return;
        }
        
        if(obj == null){
            obj = GameObject.Find("Cube");
        }

        Cube = Bootstrap.Cube;
                // typeof(TransformParent));


        int count = Bootstrap.Settings.nbOfCubes;
        float Radius = Bootstrap.Settings.radius;//_Cubesp.RadiusComponent[0].Value;
        float radius = 0;
        MeshInstanceRenderer mir = obj.GetComponent<MeshInstanceRendererComponent>().Value;

        float segment = math.radians((float)137.51);
        Debug.Log(segment);

        for (int i = 0; i < count; i++){
                // Debug.Log(i);
                radius = 1.3f * math.sqrt(i);
                PostUpdateCommands.CreateEntity(Cube);
                PostUpdateCommands.SetSharedComponent<MeshInstanceRenderer>(mir);
                PostUpdateCommands.SetComponent(new Position { Value = new float3(0,0,0) + new float3(
                    radius * math.sin(i*segment + Time.deltaTime)*math.cos(0),
                    radius * math.sin(0)*math.sin(i*segment + Time.deltaTime),
                    radius * math.cos(i*segment + Time.deltaTime)) });
                PostUpdateCommands.SetComponent(new RotationSpeed { Value = 2});
                localCount++;
                // PostUpdateCommands.SetComponent(new TransformParent { Value = new Position { Value = new float3(0,0,0)} });
        }
        UnityEngine.GameObject.Destroy(obj);
        
        World.Active.GetOrCreateManager<CubeSpawnSystem>().Enabled = false;
        // Debug.Log(_Cubesp.Length);
        // PostUpdateCommands.RemoveComponent<Radius>(_Cubesp.Entities[0]);
        // PostUpdateCommands.DestroyEntity(_Cubesp.Entities[0]);

            
        
    }

    // protected override JobHandle OnUpdate(JobHandle inputDeps) {
    //     // var MIR;
    //     if(obj == null){
    //         obj = GameObject.Find("Cube");
            
    //     }

    //     mira =new NativeArray<MeshInstanceRenderer>[1];
        
    //     Cube = World.Active.GetOrCreateManager<EntityManager>().CreateArchetype(typeof(Position), typeof(CubeComp), typeof(MeshInstanceRenderer), typeof(TransformMatrix));
    //     // PostUpdateCommands.CreateEntity(Cube);
    //     if(m_CubeData.Length >= 20)
    //         return inputDeps;
        
    //     Debug.Log("asdasdasd" + m_CubeData.Length);
    //     SpawnCubes SPJob = new SpawnCubes(){
    //         count = 20,
    //         radius = 10,
    //         mir = obj.GetComponent<MeshInstanceRendererComponent>().Value,
    //         CommandBuffer = _endFrameBarrier.CreateCommandBuffer(),
    //         Cube = Cube
    //         };

    //         return SPJob.Schedule(inputDeps);
    // }
}