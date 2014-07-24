using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Curves {
	/**
	 * @brief Evaluate the cubic Bernstein polynomials at given parameter.
	 *
	 * @verbatim
	 * B(0) = (1-t)^3           =  -t^3 + 3t^2 - 3t + 1
	 * B(1) = 3 * (1-t)^2 * t   =  3t^3 - 6t^2 + 3t
	 * B(2) = 3 * (1-t) * t^2   = -3t^3 + 3t^2
	 * B(3) = t^3               =   t^3
	 * @endverbatim
	 *
	 * @param[out] result
	 * @param t
	 *
	 * @see compute_bernstein_dt()
	 * @see compute_bernstein_dt_dt()
	 */
	static Vector4 ComputeBernstein(float t) {
		float q = 1.0f - t;
		return new Vector4(
			q * q * q,
			3.0f * t * q * q,
			3.0f * t * t * q,
			t * t * t
		);
	}
	
	/**
	 * @brief Evaluate the (first) derivative of cubic Bernstein polynomials at given parameter.
	 *
	 * @verbatim
	 * B'(0) = -3t^2 + 6t - 3
	 * B'(1) = 9t^2 - 12t + 3
	 * B'(2) = -9t^2 + 6t
	 * B'(3) = 3t^2
	 * @endverbatim
	 *
	 * @param[out] result
	 * @param t
	 *
	 * @see compute_bernstein()
	 * @see compute_bernstein_dt_dt()
	 */
	static Vector4 ComputeBernsteinDt(float t) {
		return new Vector4(
			- 3.0f * t * t +  6.0f * t - 3.0f,
			  9.0f * t * t - 12.0f * t + 3.0f,
			- 9.0f * t * t +  6.0f * t,
			  3.0f * t * t
		);
	}


	/**
	 * @brief Evaluate the second derivative of cubic Bernstein polynomials at given parameter.
	 *
	 * @verbatim
	 * B''(0) = -6t + 6
	 * B''(1) = 18t - 12
	 * B''(2) = -18t + 6
	 * B''(3) = 6t
	 * @endverbatim
	 *
	 * @param[out] result
	 * @param t
	 *
	 * @see compute_bernstein()
	 * @see compute_bernstein_dt()
	 */
//	static Vector4 compute_bernstein_dt_dt(float t)
//	{
//		return new Vector4(
//			- 6.0f * t - 6.0f,
//			 18.0f * t - 12.0f,
//			-18.0f * t + 6.0f,
//			  6.0f * t
//		);
//	}


	/**
	 * @brief Evaluate the cubic Bezier curve at given parameter point.
	 *
	 * Standard Bezier curve is a linear combination of control points (@a p1, @a p2, @a p3, @a p4),
	 * weighted by factors obtained from cubic Bernstein polynomials evaluated at point @a t.
	 *
	 * @param[out] result
	 * @param p1
	 * @param p2
	 * @param p3
	 * @param p4
	 * @param t
	 */
	public static Vector3 EvaluateBezierCurve(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t) {
		Vector4 c = ComputeBernstein(t);
		return new Vector3(
			c[0] * p1[0] + c[1] * p2[0] + c[2] * p3[0] + c[3] * p4[0],
			c[0] * p1[1] + c[1] * p2[1] + c[2] * p3[1] + c[3] * p4[1],
			c[0] * p1[2] + c[1] * p2[2] + c[2] * p3[2] + c[3] * p4[2]
		);
	}


	/**
	 * @brief Evaluate the derivative of cubic Bezier curve.
	 *
	 * Compute the linear combination of control points  (@a p1, @a p2, @a p3, @a p4),
	 * weighted by factors obtained from first derivatives of cubic Bernstein polynomials, evaluated at point @a t.
	 *
	 * @param[out] result
	 * @param p1
	 * @param p2
	 * @param p3
	 * @param p4
	 * @param t
	 *
	 * @see evaluate_bezier_curve()
	 */
	static Vector3 EvaluateBezierCurveDt(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t){
		Vector4 c = ComputeBernsteinDt(t);
		return new Vector3 (
			c[0] * p1[0] + c[1] * p2[0] + c[2] * p3[0] + c[3] * p4[0],
			c[0] * p1[1] + c[1] * p2[1] + c[2] * p3[1] + c[3] * p4[1],
			c[0] * p1[2] + c[1] * p2[2] + c[2] * p3[2] + c[3] * p4[2]
		);
	}

	/**
	 * @brief smooth continuous curve helper
	 *
	 * @param p1 position of the starting waypoint
	 * @param tang1 tangential vector (direction vector) at the starting waypoint
	 * @param p2 position of the ending waypoint
	 * @param tang2 tangential vector (direction vector) at the ending waypoint
	 * @param coef coefficient ranging from 0.0f to 1.0f suggesting how far from start to end to generate a point
	 * @param[out] pos filled with appropriate position
	 * @param[out] tangvec filled with appropriate tangential vector (normalized)
	 *
	 * Smooth continuous curve helper. Based on piecewise cubic bezier curves.
	 *
	 * Suggested usage: Waypoint navigation.
	 *
	 * Feed the function with two waypoint positions (p1 and p2), and the tangential vectors
	 * in those positions (tang1 and tang2). The coef parameter can range from 0.0f to 1.0. As
	 * the coef grows ( with time in the system ), the function generates appropriate positions and
	 * tangentials along a bezier curve passing through those two waypoints.
	 *
	 * Once coef is getting past 1.0, it is time for you to generate a new waypoint and direction,
	 * and once "decrement coef by 1.0" when adding a new curve segment, copy the values from
	 * <p2, tang2> into <p1, tang1>. This way you will insure the next curve segment will not
	 * only be continuous, but also that the transition will be smooth.
	 *
	 * Note that the length of the tangential vectors affects the curvature of the segments!
	 * Usually, you will want the length of the tangential vectors to be in 10's of percentage
	 * points of the length of the <p2-p1> vector
	 *
	 * @bug Does not follow our coding standard - parameter passing.
	 */
	public static List<Vector3> SmoothCurve(Vector3 p1, Vector3 tang1, Vector3 p2, Vector3 tang2, float coef, bool evalTangent) {
		// reformat input for use in the support helper functions
	
		Vector3 pp1 = p1;
		Vector3 pp2 = p1 + tang1;
		Vector3 pp3 = p2 - tang2;
		Vector3 pp4 = p2;
	
		// Compute position along the Bezier curve.
	
		Vector3 pos = EvaluateBezierCurve(pp1, pp2, pp3, pp4, coef);
	
		// Compute appropriate tangential vector along the curve.
		Vector3 tangent = Vector3.zero;
		if (evalTangent) {
			tangent = EvaluateBezierCurveDt(pp1, pp2, pp3, pp4, coef);
			tangent.Normalize();
		}
		
		return new List<Vector3>() {pos, tangent};
	}


///**
// * @brief Compute the curvature at given point on the curve, restricting the curve to XZ plane.
// *
// * The curvature is inverse of the radius of circle approximating the curve at given point.
// * The smaller the curvature is, the larger the radius is -
// * the "straighter" the curve is.
// * As such, it is true measure of "curviness"  of the curve.
// *
// * In case there would be a division be zero error,
// * the function returns curvature of 0.
// * It is quite acceptable solution for us -
// * the code above has to cope with line segment (curvature 0).
// *
// * @param point_1
// * @param tangent_1
// * @param point_2
// * @param tangent_2
// * @param t the parameter value to compute the curvature at.
// * @return The curvature.
// *
// * @see smooth_curve()
// */
//S_LDLL_EXPORT_DEF(float smooth_curve_planar_curvature(const float3 &point_1, const float3 &tangent_1, const float3 &point_2, const float3 &tangent_2, const float t))
//{
//	// For information how to compute curvature of 2d curve,
//	// you might want to check the MathWorld:
//	// http://mathworld.wolfram.com/Curvature.html
//	//
//	//          x'(t) * y''(t) - y'(t) * x''(t)
//	// K(t) = -----------------------------------
//	//        pow( sqr(x'(t)) + sqr(y'(t)), 3/2 )
//	//
//
//	// The curve evaluation is based off the points, not points and tangents.
//	// Compute the control points.
//
//	const float3 &p1 = point_1;
//	const float3 p2 = point_1 + tangent_1;
//	const float3 p3 = point_2 - tangent_2;
//	const float3 &p4 = point_2;
//
//	// Coefficients of the first derivative of Bernestein polynomials.
//	float4 c1;
//	compute_bernstein_dt(&c1, t);
//
//	// Coefficients of the second derivative of Bernestein polynomials.
//
//	float4 c2;
//	compute_bernstein_dt_dt(&c2, t);
//
//	// We want the XZ plane projection - "re-index" the vector components.
//
//	const unsigned X = 0;
//	const unsigned Y = 2;
//
//	// Evaluate the derivatives of our Bezier curve (projected in XZ plane).
//
//	const float x1 = c1[0] * p1[X] + c1[1] * p2[X] + c1[2] * p3[X] + c1[3] * p4[X];
//	const float y1 = c1[0] * p1[Y] + c1[1] * p2[Y] + c1[2] * p3[Y] + c1[3] * p4[Y];
//	const float x2 = c2[0] * p1[X] + c2[1] * p2[X] + c2[2] * p3[X] + c2[3] * p4[X];
//	const float y2 = c2[0] * p1[Y] + c2[1] * p2[Y] + c2[2] * p3[Y] + c2[3] * p4[Y];
//
//	const float denominator = v_pow(v_sqr(x1) + v_sqr(y1), 3.0f / 2.0f);
//	if (denominator < vec_traits_t<float>::EPSILON()) {
//		return 0.0f;
//	}
//	const float numerator = x1 * y2 - y1 * x2;
//	return numerator / denominator;
//}


} /* namespace prism */

/* eof */

