using UnityEngine;
using System.Collections;

/** Item segments are segments that are not exported with static segment info and are instead created dynamically
	from ResourceServer info. */
public abstract class ItemSegment : Segment {
	
	public Item item;
	
}
