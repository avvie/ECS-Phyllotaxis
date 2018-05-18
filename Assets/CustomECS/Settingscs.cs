using UnityEngine;
using UnityEngine.UI;

public class Settingscs : MonoBehaviour{
    public float radius = 2;
    public int nbOfCubes = 20;
    public Text text;
    void Update(){
        text.text = (Time.deltaTime).ToString();
    }
}
