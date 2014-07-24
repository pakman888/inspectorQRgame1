using UnityEngine;
using System.Collections.Generic;

public static class Extensions {
	private static void ProcessChild<T>(Transform obj, ref List<T> list) where T : Component {
	    T c = obj.GetComponent<T>();
	    if (c != null)
	        list.Add(c);
	    foreach(Transform child in obj)
	        ProcessChild<T>(child,ref list);
	}
	
	public static T[] GetComponentsInChildrenEditor<T>(this GameObject obj) where T : Component {
	    List<T> result = new List<T>();
		foreach(Transform child in obj.transform) {
	        ProcessChild<T>(child, ref result);
		}
	    
	    return result.ToArray();
	}
	   
	public static float ClampAngleNearTo(this float input, float centre, float magnitudeLimit) {
        input -= centre;
        input = input.DiffdMod(360);
        input = Mathf.Sign(input) * Mathf.Min(Mathf.Abs(input), Mathf.Abs(magnitudeLimit));
        return input;
    }
    public static bool IsAngleNear(this float input, float centre, float magnitudeLimit) {
        input -= centre;
        input = input.DiffdMod(360);
        return Mathf.Abs(input) <= Mathf.Abs(magnitudeLimit);
    }
    public static bool IsAngleBetween(this float input, float startAngle, float finishAngle) {
        return input.IsAngleNear(CenterOfAngleRange(startAngle, finishAngle), SpanOfAngleRange(startAngle, finishAngle) / 2);
    }
    public static float CenterOfAngleRange(float startAngle, float finishAngle) {
        return startAngle + SpanOfAngleRange(startAngle, finishAngle) / 2;
    }
    public static float SpanOfAngleRange(float startAngle, float finishAngle) {
        return (finishAngle - startAngle).ProperMod(360);
    }
    /// <summary>
    /// Returns the remainder of a division, ensuring the result is non-negative and as small as possible.
    /// </summary>
    public static float ProperMod(this float input, float divisor) {
        input %= divisor;
        if (input < 0) input += divisor;
        return input;
    }
    /// <summary>
    /// Returns the remainder of a division, ensuring the result has as small a magnitude as possible.
    /// </summary>
    public static float DiffdMod(this float input, float divisor) {
        input %= divisor;
        if (input >= divisor / 2) {
            input -= divisor;
        }
        if (input < -divisor / 2) {
            input += divisor;
        }
        return input;
    }
}