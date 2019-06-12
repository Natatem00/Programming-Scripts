using UnityEngine;

/// <summary>
/// Node class for Grid
/// Version: 1.0
/// </summary>

public class Node{

    public bool isWalkable; //can player walk on this node
    public Vector3 worlNodePosition;

    public Node(bool _isWalkable, Vector3 _worldNodePosition)
    {
        isWalkable = _isWalkable;
        worlNodePosition = _worldNodePosition;
    }

}
