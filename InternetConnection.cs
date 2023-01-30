using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class InternetConnection : MonoBehaviour
{
    /*-1 default, 0 - error, 1 - internet available */
    private int isInternetConnection = -1;
    private int defaultUrlIdx = 0;
    private string[] defaultUrls = new string[] 
        { "http://www.adobe.com", 
          "http://www.youtube.com/", 
          "http://www.apple.com/",
          "http://wikipedia.org" };

    public void checkInternetConnection(string url, int timeout)
    {
        isInternetConnection = -1;
        StartCoroutine(HttpHeadRequest(url, timeout));
    }

    IEnumerator HttpHeadRequest(string url, int timeout)
    {
        defaultUrlIdx %= defaultUrls.Length;
        if (url == null)
        {
            url = defaultUrls[defaultUrlIdx];
        }
        defaultUrlIdx++;

        //print("NETDBG1XYZZ GetRequest " + Time.time + " URL " + url + " TIMEOUT " + timeout);
        UnityWebRequest req = UnityWebRequest.Head(url);

        req.timeout = timeout;
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            //Debug.Log("NETDBG1 ERROR " + url);
            //Debug.Log("NETDBG1 ERROR " + $"{ req.error}: {req.downloadHandler.text}");
            isInternetConnection = 0;
        }
        else
        {
            //print("NETDBG1XYZ GetResponse FINE!!" + Time.time);
            isInternetConnection = 1;
            //Debug.Log(req.downloadHandler.text);
        }
        //callback(request);        
    }


    IEnumerator HttpGetRequest(string url, int timeout)
    {
        //print("NETDBG1 GetRequest " + Time.time);
        UnityWebRequest req = UnityWebRequest.Get(url);
      
        req.timeout = timeout;
        yield return req.SendWebRequest();

        //print("NETDBG1 GetResponse " + Time.time);
        if (req.isNetworkError || req.isHttpError)
        {
            //Debug.Log("NETDBG1 ERROR");
            //Debug.Log("NETDBG1 ERROR " + $"{ req.error}: {req.downloadHandler.text}");
            isInternetConnection = 0;
        } else
        {
            //print("NETDBG1 FINE!!");
            isInternetConnection = 1;
            //Debug.Log(req.downloadHandler.text);
        }
         //callback(request);        
    }

    public int getInternetConnectionStatus()
    {
        return isInternetConnection;
    }
}

