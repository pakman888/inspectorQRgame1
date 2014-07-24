using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkyBoxItem {
    public int index;
    public string name;
    public string texture;
    public string top;
    public Vector3 fogColor;
    public float fogDensity;
    public bool rain;
    public bool snow;
}
