using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TrafficLightController : MonoBehaviour {
	
	private class TrafficLightGroup{
		public LIGHT_STATES State{
			get{
				return state;
			}
			set{
				state = value;
				foreach(TrafficLight light in Lights){
					if(light){
						light.State = value;
					}
				}
			}
		}
		private LIGHT_STATES state = LIGHT_STATES.NONE;		
		public List<TrafficLight> Lights = new List<TrafficLight>();
	}
	
	public enum LIGHT_STATES {
        RED = 0,
        YELLOW,
        GREEN,
        NONE
    }
    
	public const float GREEN_TIME = 20f;
    public const float YELLOW_TIME = 3f;
    public const float TRANSITION_TIME = 3f;
	
    public PrefabItem intersectionInfo;

    public int USES_TRAF_LIGHT_FLAG = 0x00080000;

    private TrafficLightGroup group1 = new TrafficLightGroup();
    private TrafficLightGroup group2 = new TrafficLightGroup();
    private List<TrafficLight> trafficLights = new List<TrafficLight>();

	public void AddTrafficLight(TrafficLight light){
		trafficLights.Add(light);
	}

    public void SetGroups(List<NodeItem> nodes) {
        group1.Lights.Clear();
        group2.Lights.Clear();

        // get nodes from intersection info;
        var firstNode = trafficLights[0].node;
        // add the first node to the first group.
        group1.Lights.Add(trafficLights[0]);
        if (intersectionInfo.prefabID == 6 
            || intersectionInfo.prefabID == 7) {
            HandlePrefab6And7(intersectionInfo.prefabID, nodes);
            return;
        }

        for(int i = 1; i < trafficLights.Count; ++i ) {
            var trafficLight = trafficLights[i];
            var node = trafficLight.node;
            if (Mathf.Abs( Vector3.Dot(firstNode.direction.normalized, node.direction.normalized)) 
                    > 0.9  ) {
                group1.Lights.Add(trafficLight);
            } else {
				group2.Lights.Add(trafficLight);
			}
        }
    }
    //Hacks hax simon prefab6 prefab7
    void HandlePrefab6And7(int prefabtype, List<NodeItem> nodes) {
        var first = (from TrafficLight light in trafficLights
                    where light.node.position == nodes[0].position
                     select light).First();
        var second = (from TrafficLight light in trafficLights
                      where light.node.position == nodes[1].position
                      select light).First();
        var third = (from TrafficLight light in trafficLights
                     where light.node.position == nodes[2].position
                     select light).First(); 
                    
        if (prefabtype == 6) {
            group1.Lights.Add(first);
            group1.Lights.Add(second);
            group2.Lights.Add(third);
        }
        if (prefabtype == 7) {
            group1.Lights.Add(first);
            group2.Lights.Add(second);
            group1.Lights.Add(third);
        }
    }

    IEnumerator Start() {
		//initial setup
		var activeGroup = group1;
		var inactiveGroup = group2;
		
		activeGroup.State = LIGHT_STATES.GREEN;
		inactiveGroup.State = LIGHT_STATES.RED;	
		
		//Run forever
		while(true){
			activeGroup.State = LIGHT_STATES.GREEN;
			yield return new WaitForSeconds(GREEN_TIME);
			activeGroup.State = LIGHT_STATES.YELLOW;
			yield return new WaitForSeconds(YELLOW_TIME);
			activeGroup.State = LIGHT_STATES.RED;
			yield return new WaitForSeconds(TRANSITION_TIME);
			//Swap active/inactive groups
			var temp = activeGroup;
			activeGroup = inactiveGroup;
			inactiveGroup = temp;
		}		
    }

    void CreateHelpers() {
        foreach (TrafficLight trafficLight in group1.Lights) {
            var node = trafficLight.node;
            Debug.DrawRay(( node.position + Vector3.up * 5 ), node.direction,  Color.red, 100f);
        }
        foreach (TrafficLight trafficLight in group2.Lights) {
            var node = trafficLight.node;
            Debug.DrawRay(( node.position + Vector3.up * 5 ), node.direction,  Color.blue, 100f);
        }
    }

    public  LIGHT_STATES RedLightPenalty(GameObject bus) {
        if ((intersectionInfo.flags & USES_TRAF_LIGHT_FLAG ) ==  0) {
            return LIGHT_STATES.NONE;
        }
        //Get the closest node
        var closestDist = float.MaxValue;
        var groupToUse = 0;
        foreach (TrafficLight trafficLight in group1.Lights) {
            var distanceFromNode = (bus.transform.position - trafficLight.node.position).magnitude;
            if (distanceFromNode < closestDist) {
                closestDist = distanceFromNode;
                groupToUse = 1;
            }
        }
        foreach (TrafficLight trafficLight in group2.Lights) {
            var distanceFromNode = (bus.transform.position - trafficLight.node.position).magnitude;
            if (distanceFromNode < closestDist) {
                closestDist = distanceFromNode;
                groupToUse = 2;
            }
            
        }
        //Debug.Log(groupToUse);
        if (groupToUse == 0) {
            return LIGHT_STATES.NONE;
        } else if(groupToUse == 1){
			return group1.State;
		} else {
			return group2.State;
		}		
	}
}
