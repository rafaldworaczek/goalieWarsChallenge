using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics.FixedPoint;
using System;
using GlobalsNS;


public class BezierCuve : MonoBehaviour
{
    // Start is called before the first frame update
    private float t = 0;
    public Rigidbody ballRb;
    private playerControllerMultiplayer playerMainScript;
    private static float BALL_NEW_RADIUS = 0.2585f;

    void Start()
    {
        playerMainScript = Globals.player1MainScript;

    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        if (t <= 1f) {
            Vector3 outPoint = bezierCurve3DGetPoint(new Vector3(0f, 0f, -14f),
                                  new Vector3(0f, 2f, 0f)  ,  
                                  new Vector3(0f, 0f, 14f),
                                  t,
                                  true);

            t += Time.deltaTime / 2f;
            Debug.Log("#DBGTIME " + outPoint + " t " + t);

            ballRb.transform.position = outPoint;
        }
    }*/

    public static Vector3 bezierCurvePlaneInterPoint(
                                              Rigidbody gObject,
                                              Vector3[,] arr,
                                              int s,
                                              int e,
                                              bool overwritteZ)
                                               
    {
        int mid = (s + e) / 2;
        Vector3 midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[mid, 0]);

        if ((e <= s))
           /// ((midPoint.z < 0.5f) && (midPoint.z >= 0f)))
        {
            //if (overwritteZ)
            //    midPoint.z = arr[mid, 1].x;

            /*Use z vector cord as time */
            //print("#DBGBINARYSEARCH MANUALCALCULATION globalPoint " + arr[mid, 0] + " localPoint " + midPoint);
            if (Mathf.Abs(midPoint.z) < 0.1f)
            {
                if (overwritteZ)
                    midPoint.z = arr[mid, 1].x;
                return midPoint;
            }
            else
            {
                if (!overwritteZ)
                {
                    return bezierCurvePlaneInterPoint(
                                    0f,
                                    1f,
                                    gObject,
                                    arr[mid, 0],
                                    arr[mid + 1, 0],
                                    false);
                }
                else
                {
                    midPoint = bezierCurvePlaneInterPoint(
                            0f,
                            1f,
                            gObject,
                            arr[mid, 0],
                            arr[mid + 1, 0],
                            true);

                    midPoint.z = arr[mid, 1].x;
                    return midPoint;
                }
            }            
        }

        if ((e - s) <= 1)
        {
            if (e > s)
            {
                if (Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]).z < 0)
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);

                        //print("#DBGBINARYSEARCH MANUALCALCULATION 1 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[s, 0]);
                        midPoint.z = arr[s, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[s, 0],
                                            arr[s + 1, 0],
                                            true);
                            midPoint.z = arr[s, 1].x;
                            return midPoint;
                        }
                    }
                }
                else
                {
                    if (!overwritteZ)
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        //print("#DBGBINARYSEARCH MANUALCALCULATION 2 midPoint " + midPoint + " localPoint " + midPoint);
                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            return bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            false);
                        }
                    }
                    else
                    {
                        midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]);
                        midPoint.z = arr[e, 1].x;

                        if (Mathf.Abs(midPoint.z) < 0.1f)
                            return midPoint;
                        else
                        {
                            midPoint = bezierCurvePlaneInterPoint(
                                            0f,
                                            1f,
                                            gObject,
                                            arr[e, 0],
                                            arr[e + 1, 0],
                                            true);
                            midPoint.z = arr[e, 1].x;
                            return midPoint;
                        }

                        //midPoint.z = arr[e, 1].x;               
                    }
                }
            }
        }

        if (midPoint.z >= 0f)
                return bezierCurvePlaneInterPoint(gObject, arr, mid, e, overwritteZ);
            else
                return bezierCurvePlaneInterPoint(gObject, arr, s, mid, overwritteZ);
    }

    public static int bezierCurvePlaneInterPoint(
                                            Rigidbody gObject,
                                            Vector3[,] arr,
                                            int s,
                                            int e)

    {
        int mid = (s + e) / 2;
        Vector3 midPoint = Globals.InverseTransformPointUnscaled(gObject.transform, arr[mid, 0]);

        if (e <= s) {
            //||
          //  (midPoint.z < 0.5f && midPoint.z >= 0f)) { 
          
            /*Use z vector cord as time */
            //print("#DBGBINARYSEARCH globalPoint " + arr[mid, 0] + " localPoint " + midPoint);
            return mid;
        }

        if ((e - s) <= 1)
        {

            if (e > s)
            {
                if (Globals.InverseTransformPointUnscaled(gObject.transform, arr[e, 0]).z < 0)
                {
                    return s;
                }
                else
                {
                    return e;
                }
            }
        }

        if (midPoint.z >= 0f)
            return bezierCurvePlaneInterPoint(gObject, arr, mid, e);
        else
            return bezierCurvePlaneInterPoint(gObject, arr, s, mid);
    }

    static Collider[] hitColliders;
    public static int bezierCurvePlaneInterPoint(
                                           Vector3 playerPos,
                                           Vector3[,] arr,
                                           int s,
                                           int mid,
                                           int e,
                                           GameObject leftHand,
                                           GameObject rightHand,
                                           Rigidbody playerRb)
    {
        int layerId = 19;
        int layerMask = 1 << layerId;
        /*print("DEBUGGK1045 ballLocPlary " + s + " mid " + mid + " e " + e
            + " plaeyrPos " + playerPos +
            " arr[mid, 0] " + arr[mid, 0]);*/

        /*print("DEBUGGK1045 ballPos " + arr[mid, 0] 
            + "ballLocPlary " + 
            Globals.InverseTransformPointUnscaled(
                    playerRb.transform, arr[mid, 0]) + 
                    " leftHand " + 
                    leftHand.transform.position 
                    + " rightHand " + 
                    rightHand.transform.position 
                    + " rightToBallLocal " +
                    Globals.InverseTransformPointUnscaled(
                        rightHand.transform, arr[mid, 0])
                    + " leftToBallLocal " +
                     Globals.InverseTransformPointUnscaled(
                        leftHand.transform, arr[mid, 0])
                     + " leftdistance " + Vector3.Distance(leftHand.transform.position, arr[mid, 0])
                     + " rightDistance " + Vector3.Distance(rightHand.transform.position, arr[mid, 0]));
                     */
        hitColliders = Physics.OverlapSphere(
                            arr[mid, 0],
                            BALL_NEW_RADIUS,
                            Physics.AllLayers);

        if (PredictCollision.doesItCollideWithPlayer(hitColliders))
            e = mid - 1;
        else        
            s = mid;

        /*print("DEBUGGK1045 beziercurve s " + s + " mid " + mid + " e " + e
            + " plaeyrPos " + playerPos +
            " arr[mid, 0] " + arr[mid, 0]
            + " hitColliders " + hitColliders.Length);*/

        int nonCollideIdx = s;
        for (int i = s; i <= e; i++)
        {
            if (!PredictCollision.doesItCollideWithPlayer(
                 Physics.OverlapSphere(
                     arr[i, 0],
                     BALL_NEW_RADIUS,
                     layerMask)))
            {
                nonCollideIdx = i;
                /*print("DEBUGGK1045 ballLocPlary s " + s + " mid " + mid + " e " + e
                    + " nonCollideIdx " + nonCollideIdx
                    + " playerPos " + playerPos
                    + " ballPos " + arr[i, 0]
                    + " I " + i);
                print(" r " +
    Globals.InverseTransformPointUnscaled(
            playerRb.transform, arr[i, 0]) +
            " leftHand " +
            leftHand.transform.position
            + " rightHand " +
            rightHand.transform.position
            + " rightToBallLocal " +
            Globals.InverseTransformPointUnscaled(
                rightHand.transform, arr[i, 0])
            + " leftToBallLocal " +
             Globals.InverseTransformPointUnscaled(
                leftHand.transform, arr[i, 0])
             + " leftdistance " + Vector3.Distance(leftHand.transform.position, arr[i, 0])
             + " rightDistance " + Vector3.Distance(rightHand.transform.position, arr[i, 0])
             + " I " + i);*/
            }
            else
            {
                break;
            }
        }

        /*print("DEBUGGK1045 RESULT beziercurve s " + s + " mid " + mid + " e " + e
            + " nonCollideIdx " + nonCollideIdx
            + " playerPos " + playerPos
            + " result " + arr[nonCollideIdx, 0]);*/

        return nonCollideIdx;
    }

    public static Vector3 bezierCurve3DGetPoint(Vector3 p0,
                                                Vector3 p1,
                                                Vector3 p2,
                                                float t,
                                                bool isFixedMath)
    {
        Vector3 outPoint = Vector3.zero;
        if (!isFixedMath)
        {
            outPoint =
                (1 - t) * (1f - t) * p0 + 2 * (1f - t) * t * p1 + t * t * p2;
        } else
        {
            fp valueOne = 1m;
            fp valueTwo = 2m;
            fp tFix = Convert.ToDecimal(t);
            fp p0x = Convert.ToDecimal(p0.x);
            fp p0y = Convert.ToDecimal(p0.y);
            fp p0z = Convert.ToDecimal(p0.z);
            fp p1x = Convert.ToDecimal(p1.x);
            fp p1y = Convert.ToDecimal(p1.y);
            fp p1z = Convert.ToDecimal(p1.z);
            fp p2x = Convert.ToDecimal(p2.x);
            fp p2y = Convert.ToDecimal(p2.y);
            fp p2z = Convert.ToDecimal(p2.z);

            outPoint.x = (float)
                   ((valueOne - tFix) * (valueOne - tFix) * p0x + valueTwo *
                   (valueOne - tFix) * tFix * p1x + tFix * tFix * p2x);

            outPoint.y = (float)
                   ((valueOne - tFix) * (valueOne - tFix) * p0y + valueTwo *
                   (valueOne - tFix) * tFix * p1y + tFix * tFix * p2y);

            outPoint.z = (float)
                   ((valueOne - tFix) * (valueOne - tFix) * p0z + valueTwo *
                   (valueOne - tFix) * tFix * p1z + tFix * tFix * p2z);

            print("#DEBUG outPoint " + outPoint);
        }

        return outPoint;
    }

    private static Vector3 bezierCurvePlaneInterPoint(float s,
                                              float e,
                                              Rigidbody rb,
                                              Vector3 startPos,
                                              Vector3 endPos,
                                              bool overwritteZ)
    {
        float time = (s + e) / 2.0f;
        Vector3 midPoint = getCurveShotPosLocal(rb, startPos, endPos, time);

    //print("BEZIERCURE " + " S " + s + " E " + e + " TIME " + time + " POINT " + point);

        if ((e - s) <= 0.0005f ||
            (Mathf.Abs(midPoint.z) <= 0.1f) ||
            (e <= s))
    {
        /*Use z vector cord as time */
        if (overwritteZ)
            midPoint.z = time;
        return midPoint;
    }

    Vector3 endPoint = getCurveShotPosLocal(rb, startPos, endPos, e);
    if (!Globals.compareSign(midPoint.z, endPoint.z))
        return bezierCurvePlaneInterPoint(time, e, rb, startPos, endPos, overwritteZ);
    else
        return bezierCurvePlaneInterPoint(s, time, rb, startPos, endPos, overwritteZ);
    }

    private static Vector3 getCurveShotPosLocal(Rigidbody gObject,
                                         Vector3 startPos,
                                         Vector3 endPos,
                                         float currentTime)
    {
        Vector3 m1 = Vector3.Lerp(startPos, endPos, currentTime);

        return Globals.InverseTransformPointUnscaled(gObject.transform, m1);
    }
}
