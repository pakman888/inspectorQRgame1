[System.Serializable]
public class Item  {
	public static int KIT_buildings = 1;
	public static int KIT_road = 2;
	public static int KIT_prefab = 3;
	public static int KIT_model = 4;
	public static int KIT_mission_point = 5;
	public static int KIT_services = 6;
	public static int KIT_cut_plane = 7;
	public static int KIT_mover = 8;
	public static int KIT_particles = 9;
	public static int KIT_no_weather = 10;
	public static int KIT_city = 11;
	public static int KIT_hinge = 12;
	public static int KIT_quest_point = 13;
	public static int KIT_bus_stop = 14;
	public static int KIT_animated_model = 15;
	public static int KIT_mission_model = 16;
    
    public int kitType;
	public int flags;
	public byte distType;
	
	public int subtableIndex;
}