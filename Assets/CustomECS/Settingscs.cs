using Unity.Rendering;
using UnityEngine;
public class Settingscs : MonoBehaviour {
    public float radius = 2;
    public int nbOfCubes = 20;
    public float lerpFact = 1;

    public Mesh cube;
    public Material mat;
    MeshInstanceRenderer MSI;

    void Start() {
        GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube = GO.GetComponent<MeshFilter>().mesh;

        MSI = new MeshInstanceRenderer();
        MSI.material = mat;
        MSI.mesh = cube;
        Destroy(GO);
    }

    public MeshInstanceRenderer getMSI() {
        return MSI;
    }
}