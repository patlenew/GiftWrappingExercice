using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple node class
/// </summary>
public class ConvexHullNode : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _referencePoint;

    private bool _isComplete = false;

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetLinePosition(Vector3 position)
    {
        _line.SetPosition(1, GetDistanceFromPosition(position));
    }

    public void SetComplete()
    {
        _isComplete = true;
    }

    public bool IsComplete()
    {
        return _isComplete;
    }

    public Vector3 GetPosition()
    {
        return _referencePoint.position;
    }

    private Vector3 GetDistanceFromPosition(Vector3 position)
    {
        Vector3 towardPosition = position - GetPosition();
        return towardPosition;
    }
}
