using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event UnityAction<int> OnAddPoints;
    public static event UnityAction<int> OnObjectDestroy;

    public static void RaiseAddPoints(int points) => OnAddPoints?.Invoke(points);
    public static void ObjectBeignDestroyed(int objectId) => OnObjectDestroy?.Invoke(objectId);
}
