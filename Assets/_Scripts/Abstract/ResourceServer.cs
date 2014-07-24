using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ResourceServer : DerivedSingleton<ResourceServer> {
	public List<NodeItem> nodes;
	[SerializeField] private List<Item> items;
	
	public Dictionary<int, int> stopUIDsToNodeIndices{
		get{
			if(_stopNodeDictionary == null){
				BuildDictionaries();
			}
			return _stopNodeDictionary;
		}
	}
	private Dictionary<int, int> _stopNodeDictionary;
	
	public List<int> stopUIDsToNodeIndicesKeys;
	public List<int> stopUIDsToNodeIndicesValues;
    public List<int> buildingsIndices;
    public List<int> roadIndices;
    public List<int> prefabIndices;
    public List<int> modelIndices;
    public List<BusStopLook> busStopLooks;
	public List<RoadLook> roadLooks;
	public List<PrefabDef> prefabDefs;
	public List<string> modelPaths;
	public List<string> vegetationPaths;
    public List<SkyBoxItem> skyBoxDefs;
    public List<SunItem> sunItems;
	
	public List<BuildingItem> 		buildingItems		= new List<BuildingItem>();
	public List<RoadItem> 			roadItems           = new List<RoadItem>();
	public List<PrefabItem> 		prefabItems         = new List<PrefabItem>();
	public List<ModelItem> 			modelItems          = new List<ModelItem>();
	public List<CutPlaneItem> 		cutPlaneItems       = new List<CutPlaneItem>();
	public List<MoverItem> 			moverItems          = new List<MoverItem>();
	public List<NoWeatherItem> 		noWeatherItems      = new List<NoWeatherItem>();
	public List<CityItem> 			cityItems           = new List<CityItem>();
	public List<QuestPointItem> 	questPointItems     = new List<QuestPointItem>();
	public List<BusStopItem> 		busStopItems        = new List<BusStopItem>();
	public List<AnimatedModelItem> 	animatedModelItems  = new List<AnimatedModelItem>();
	public List<MissionModelItem> 	missionModelItems   = new List<MissionModelItem>();

	// Dictionaries are not serialized by Unity but lists are, so we rebuild them
	// from key lists and corresponding value lists at runtime
	private void BuildDictionaries() {
		_stopNodeDictionary = stopUIDsToNodeIndicesKeys.Select((k, i) => new { k, v = stopUIDsToNodeIndicesValues[i] }).ToDictionary(x => x.k, x => x.v);
	}

	public Item GetItem(int index){
		int type = items[index].kitType;
		int subtableIndex = items[index].subtableIndex;
		if (type == Item.KIT_buildings) {
            return buildingItems[subtableIndex];
        }
        else if (type == Item.KIT_road) {
            return roadItems[subtableIndex];
        }
        else if (type == Item.KIT_prefab) {
            return prefabItems[subtableIndex];
        }
        else if (type == Item.KIT_model) {
            return modelItems[subtableIndex];
        }
        else if (type == Item.KIT_mission_point) {
            throw new Exception("Mission point items should not exist!");
        }
        else if (type == Item.KIT_services) {
            throw new Exception("Services items should not exist!");
        }
        else if (type == Item.KIT_cut_plane) {
            return cutPlaneItems[subtableIndex];
        }
        else if (type == Item.KIT_mover) {
            return moverItems[subtableIndex];
        }
        else if (type == Item.KIT_particles) {
            throw new Exception("Particles items should not exist!");
        }
        else if (type == Item.KIT_no_weather) {
            return noWeatherItems[subtableIndex];
        }
        else if (type == Item.KIT_city) {
           return cityItems[subtableIndex];
        }
        else if (type == Item.KIT_hinge) {
            throw new Exception("Hinge items should not exist!");
        }
        else if (type == Item.KIT_quest_point) {
           return questPointItems[subtableIndex];
        }
        else if (type == Item.KIT_bus_stop) {
            return busStopItems[subtableIndex];
        }
        else if (type == Item.KIT_animated_model) {
            return animatedModelItems[subtableIndex];
        }
        else if (type == Item.KIT_mission_model) {
            return missionModelItems[subtableIndex];
        }
		return null;
	}
	
#if UNITY_EDITOR
	[SerializeField] private string mbdFilename = "Assets/base/map/bus1.mbd";
    [SerializeField] private string roadDefFilename = "Assets/base/def/world/road.def";
    [SerializeField] private string prefabDefFilename = "Assets/base/def/world/prefab.def";
	[SerializeField] private string modelDefFilename = "Assets/base/def/world/model.def";
	[SerializeField] private string vegetationDefFilename = "Assets/base/def/world/vegetation.def";
    [SerializeField] private string skyBoxDefFileName = "Assets/base/def/sky_data.def";
    [SerializeField] private string sunDefFileName = "Assets/base/def/sun_data.def";
    [SerializeField] private string busStopDefFilename = "Assets/base/def/world/bus_stop.def";
	private Dictionary<int, int> segmentLoD;

	public int GetLoD(int itemId){
		if(segmentLoD == null){
			segmentLoD = SegmentLoD.LoadRawData();
		}
		int result;
		if(!segmentLoD.TryGetValue(itemId, out result)){
			result = SegmentLoD.Default;
		}
		return result;
	}

    protected override void OnCreate() {		
		roadLooks = RoadDefParser.Parse(roadDefFilename);
        prefabDefs = PrefabDefParser.Parse(prefabDefFilename);
		modelPaths = ModelDefParser.Parse(modelDefFilename);
		vegetationPaths = VegetationDefParser.Parse(vegetationDefFilename);
        skyBoxDefs = SkyBoxParsing.Parse(skyBoxDefFileName);
        sunItems = SunParser.Parse(sunDefFileName);
        busStopLooks = BusStopDefParser.Parse(busStopDefFilename);
        ParseAllItemsAndNodes(mbdFilename);
    }
	
    void ParseAllItemsAndNodes(string filename) {
        var mbdFile = File.Open(filename, FileMode.Open);
        var reader = new BinaryReader(mbdFile);

        reader.ReadInt32(); // save map version - we don't give a damn bout it
        var nodeCount = reader.ReadUInt32();
        var itemCount = reader.ReadUInt32();

        items = new List<Item>();
        nodes = new List<NodeItem>();
		
        buildingsIndices = new List<int>();
        roadIndices = new List<int>();
        prefabIndices = new List<int>();
        modelIndices = new List<int>();
		stopUIDsToNodeIndicesKeys = new List<int>();
		stopUIDsToNodeIndicesValues = new List<int>();

        for (int i = 0; i < itemCount; i++) {
            var type = reader.ReadInt32();
            Item item = null;
            if (type == Item.KIT_buildings) {
                item = BuildingItemParser.Parse(reader);
				item.subtableIndex = buildingItems.Count;
				buildingItems.Add((BuildingItem)item);
                buildingsIndices.Add(i);
            }
            else if (type == Item.KIT_road) {
                item = RoadItemParser.Parse(reader);
				item.subtableIndex = roadItems.Count;
				roadItems.Add((RoadItem)item);
                roadIndices.Add(i);
            }
            else if (type == Item.KIT_prefab) {
                item = PrefabItemParser.Parse(reader, this);
				item.subtableIndex = prefabItems.Count;
				prefabItems.Add((PrefabItem)item);
                prefabIndices.Add(i);
            }
            else if (type == Item.KIT_model) {
                item = ModelItemParser.Parse(reader);
				item.subtableIndex = modelItems.Count;
				modelItems.Add((ModelItem)item);
                modelIndices.Add(i);
            }
            else if (type == Item.KIT_mission_point) {
                throw new Exception("Mission point items should not exist!");
            }
            else if (type == Item.KIT_services) {
                throw new Exception("Services items should not exist!");
            }
            else if (type == Item.KIT_cut_plane) {
                item = CutPlaneItemParser.Parse(reader);
				item.subtableIndex = cutPlaneItems.Count;
				cutPlaneItems.Add((CutPlaneItem)item);
            }
            else if (type == Item.KIT_mover) {
                item = MoverItemParser.Parse(reader);
				item.subtableIndex = moverItems.Count;
				moverItems.Add((MoverItem)item);
            }
            else if (type == Item.KIT_particles) {
                throw new Exception("Particles items should not exist!");
            }
            else if (type == Item.KIT_no_weather) {
                item = NoWeatherItemParser.Parse(reader);
				item.subtableIndex = noWeatherItems.Count;
				noWeatherItems.Add((NoWeatherItem)item);
            }
            else if (type == Item.KIT_city) {
                item = CityItemParser.Parse(reader);
				item.subtableIndex = cityItems.Count;
				cityItems.Add((CityItem)item);
            }
            else if (type == Item.KIT_hinge) {
                throw new Exception("Hinge items should not exist!");
            }
            else if (type == Item.KIT_quest_point) {
                item = QuestPointItemParser.Parse(reader);
				item.subtableIndex = questPointItems.Count;
				questPointItems.Add((QuestPointItem)item);
            }
            else if (type == Item.KIT_bus_stop) {
                item = BusStopItemParser.Parse(reader);
				item.subtableIndex = busStopItems.Count;
				var busStopItem = (BusStopItem)(item);
				stopUIDsToNodeIndicesKeys.Add(busStopItem.stopUID);
				stopUIDsToNodeIndicesValues.Add(busStopItem.index);
				busStopItems.Add(busStopItem);
            }
            else if (type == Item.KIT_animated_model) {
                item = AnimatedModelItemParser.Parse(reader);
				item.subtableIndex = animatedModelItems.Count;
				animatedModelItems.Add((AnimatedModelItem)item);
            }
            else if (type == Item.KIT_mission_model) {
                item = MissionModelItemParser.Parse(reader);
				item.subtableIndex = missionModelItems.Count;
				missionModelItems.Add((MissionModelItem)item);
            }
            else {
                throw new Exception(type.ToString());
            }
            item.kitType = type;        
            items.Add(item);
        }

        for (int i = 0; i < nodeCount; i++) {
            var node = NodeItemParser.Parse(reader);
            nodes.Add(node);
        }
        mbdFile.Close();
    }
#endif
}