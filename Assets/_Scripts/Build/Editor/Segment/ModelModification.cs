using UnityEngine;
using System.Collections;

public class ModelModification {

    public static void AddModelInfoToSegment(ref GameObject go, ResourceServer server, SegmentExtraInfo info) {
        if (server.GetItem(info.id).GetType() == typeof(ModelItem)) {
            var modelComponent = go.AddComponent<ModelComponent>();
            modelComponent.modelItem = (ModelItem)server.GetItem(info.id);
            modelComponent.node = server.nodes[modelComponent.modelItem.nodeIndex];
        }
        else if ( server.GetItem(info.id).GetType() == typeof(MissionModelItem)){
            var missionModelComponent = go.AddComponent<MissionModelItemComponent>();
            missionModelComponent.missionModelItem = (MissionModelItem) server.GetItem(info.id);
        } 

    }

}