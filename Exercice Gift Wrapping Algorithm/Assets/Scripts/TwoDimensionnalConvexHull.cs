using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The algorithm generator, which instanciates all nodes, and then run a coroutine animation for each sequence
/// </summary>
public class TwoDimensionnalConvexHull : MonoBehaviour
{
    [Header("Gift Wrapping Settings")]
    [SerializeField] private int _numberOfNodes = 100;
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private ConvexHullNode _nodeRef;
    [SerializeField] private Vector2Int _instantiationRange;

    [Header("Debug Settings Settings")]
    [SerializeField] private float _waitForSecondDelay = 0.2f;

    private bool _isAnimating;
    private int _currentIndex = 1;
    private List<ConvexHullNode> _currentNodes = new List<ConvexHullNode>();
    private ConvexHullNode _currentLeftestNode;
    private Coroutine _currentDrawingCoroutine;

    public void GenerateNodes()
    {
        if (!CanGenerate())
        {
            return;
        }

        CleanNodes();

        _currentIndex = 1;

        for (int i = 0; i < _numberOfNodes; i++)
        {
            ConvexHullNode newNode = Instantiate(_nodeRef, _nodeParent);
            newNode.name = newNode.name + " " + i.ToString();

            Vector3 newPos = new Vector3(Random.Range(-_instantiationRange.x, _instantiationRange.x), 0f, Random.Range(-_instantiationRange.y, _instantiationRange.y));

            newNode.SetLocalPosition(newPos);
            _currentNodes.Add(newNode);
        }

        _currentNodes = _currentNodes.OrderBy(node => node.GetPosition().x).ToList();

        _currentLeftestNode = _currentNodes[0];

        _isAnimating = true;

        _currentDrawingCoroutine = StartCoroutine(DrawingAnimation());
    }

    private IEnumerator DrawingAnimation()
    {
        ConvexHullNode currentPotentialNode = _currentNodes[_currentIndex];

        for (int j = 0; j < _currentNodes.Count; j++)
        {
            Vector3 distancePotentialToLeftNode = currentPotentialNode.GetPosition() - _currentLeftestNode.GetPosition();
            Vector3 distanceCurrentToLeftNode = _currentNodes[j].GetPosition() - _currentLeftestNode.GetPosition();

            float _currentCrossValue = GetCrossVectorY(distancePotentialToLeftNode.normalized, distanceCurrentToLeftNode.normalized);

            if (_currentCrossValue <= 0f)
            {
                continue;
            }

            yield return new WaitForSeconds(_waitForSecondDelay);

            _currentLeftestNode.SetLinePosition(_currentNodes[j].GetPosition());

            currentPotentialNode = _currentNodes[j];
        }

        SetNextLeftestNode(currentPotentialNode);
    }

    private float GetCrossVectorY(Vector3 a, Vector3 b)
    {
        return Vector3.Cross(a, b).y;
    }

    private void SetNextLeftestNode(ConvexHullNode chosenNode)
    {
        _currentDrawingCoroutine = null;

        _currentLeftestNode.SetLinePosition(chosenNode.GetPosition());
        _currentLeftestNode.SetComplete();

        if (chosenNode.IsComplete())
        {
            _currentLeftestNode.SetLinePosition(_currentNodes[0].GetPosition());

            EndAnimation();
        }
        else
        {
            _currentIndex++;

            _currentLeftestNode = chosenNode;

            _currentDrawingCoroutine = StartCoroutine(DrawingAnimation());
        }
    }

    private void EndAnimation()
    {
        _isAnimating = false;
    }

    public void CleanNodes()
    {
        _isAnimating = false;

        if(_currentDrawingCoroutine != null)
        {
            StopCoroutine(_currentDrawingCoroutine);
            _currentDrawingCoroutine = null;
        }

        foreach (ConvexHullNode node in _currentNodes)
        {
            if (node != null)
            {
                Destroy(node.gameObject);
            }
        }

        _currentNodes.Clear();
    }

    private bool CanGenerate()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Only testable in-game");
            return false;
        }

        if (_isAnimating)
        {
            Debug.LogWarning("Animation Playing");
            return false;
        }

        return true;
    }
}
