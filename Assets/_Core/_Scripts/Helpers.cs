using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Helpers
{
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> _waitDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (_waitDictionary.TryGetValue(time, out var wait)) return wait;
        
        _waitDictionary.Add(time, new WaitForSeconds(time));
        return GetWait(time);
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }
    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static Vector3 ToVector3(this Vector2 v2) 
    {
        return new Vector3(v2.x, v2.y);
    }
    public static int RandomSign()
    {
        return Random.Range(1, 3) == 1 ? 1 : -1;
    }
    public static T RandomItem<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
    public static T RandomItem<T>(this  List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static bool IsOnLayer(this GameObject gameObject, LayerMask layerMask)
    {
        return ((1 << gameObject.layer) & layerMask) != 0;
    }
    public static int Mod(int x,int m)
    {
        return (x % m + m) % m;
    }

    public static string RandomString(int length)
    {
        string glyphs = "ABCDEFGHHIJKLMNOPQRSTUVWXYZ";

        string randomString = String.Empty;
        for (int i = 0; i < length; i++)
        {
            var r = glyphs[Random.Range(0, glyphs.Length)];
            randomString = randomString + r.ToString();
        }
        return randomString;
    }
}
