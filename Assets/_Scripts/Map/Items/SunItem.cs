using UnityEngine;
using System.Collections;

[System.Serializable]
public class SunItem {
    public int index;
    public string name;
    public Vector3 ambient;
    public Vector3 diffuse;
    public Vector3 specular;
    public Vector3 enviroment;
    public Vector3 skyboxColor;
    public Vector3 sunDirection;
    public bool lampsOn;
    public bool night;
    public string sunModel;
    
}
