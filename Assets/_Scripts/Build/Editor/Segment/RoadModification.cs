using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoadModification  {
	
    public static void AttachRoadComponent(ref GameObject segment, ResourceServer server) {
        var segmentInfo = segment.GetComponent<SegmentExtraInfo>();
        if (segmentInfo.segmentType != 0) {
            return;
        }
        int id = segmentInfo.id;
        var roadComponent = segment.AddComponent<RoadComponent>();
        roadComponent.roadItem = (RoadItem)server.GetItem(id);
        roadComponent.roadLook = server.roadLooks[roadComponent.roadItem.roadLookID];
        roadComponent.Nodes = new NodeItem[2];
        roadComponent.Nodes[0] = server.nodes[roadComponent.roadItem.nodeIndices[0]];
        roadComponent.Nodes[1] = server.nodes[roadComponent.roadItem.nodeIndices[1]];

        if (roadComponent.Nodes[0].backwardIndex  > 0) {
            var backwardItem = server.GetItem(roadComponent.Nodes[0].backwardIndex);
            if (backwardItem.kitType == Item.KIT_road) {
                var prevRoadItem = (RoadItem) backwardItem;
                roadComponent.prevRoadLook = server.roadLooks[prevRoadItem.roadLookID];
            }
        }
        foreach (Transform child in segment.transform) {
            if (child.renderer != null) {
                var material = child.gameObject.renderer.sharedMaterial;
				if(!material){
					Debug.LogWarning(child.name + " in road segment " + segment.name + " has no material");
				}	
				else{
 	               Texture texture = material.GetTexture("_MainTex");
	                if (texture && (texture.name == "lines" || texture.name == "cracks" || texture.name == "sf_rails")) {
	                    child.gameObject.collider.enabled = false;
	                    child.transform.Translate(Vector3.up * 0.05f);
	                }
				}
            }
        }
    }

}

public class SegmentVertex {
    public Vector3 Position { get; set; }
    public Vector3 Normal { get; set; }
    public uint ColorValue { get; set; }
    public Vector2 UV { get; set; }
}