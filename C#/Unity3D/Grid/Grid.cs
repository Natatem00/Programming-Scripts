using UnityEngine;

/// <summary>
/// Creats grid in specified range(x and y) with specified node radius
/// Version: 1.0
/// </summary>

public class Grid : MonoBehaviour {

    #region Global variables
    Node[,] grid; //Node array
    [SerializeField]
    Transform player; //player game object
    [SerializeField]
    Vector2 gridSize; //size of the grid
    [SerializeField]
    float nodeRadius; //radius of the node
    [SerializeField]
    LayerMask notPass; //mask of the not-passable objects

    float nodeDiametr; //diametr of the node
    int gridNodesX, gridNodesY; //num of nodes in each axis
    #endregion

    #region Unity functions
    void Start()
    {
        nodeDiametr = 2 *nodeRadius; //sets node diametr

        if(nodeDiametr <= 0) 
        {
            nodeDiametr = 1.0f;
        }

        gridNodesX = Mathf.RoundToInt(gridSize.x / nodeDiametr); //sets num of nodes in X axis
        gridNodesY = Mathf.RoundToInt(gridSize.y / nodeDiametr); //sets num of nodes in Y axis

        if (gridNodesX <= 0 || gridNodesY <= 0)
        {
            gridNodesX = 1;
            gridNodesY = 1;
        }
        //if num of nods < node diametr
        if(gridNodesX <= nodeDiametr || gridNodesY <= nodeDiametr )
        {
            gridNodesX = 1;
            gridNodesY = 1;
            nodeDiametr = 1.0f;
        }
        CreateGrid();
    }
    #endregion

    #region User functions
    void CreateGrid()
    {
        //instantiates new grid
        grid = new Node[gridNodesX, gridNodesY];
        //finds left bottom grid position in world
        Vector3 leftBottomGridPosition = (-Vector3.right * gridNodesX * nodeDiametr/2) - (Vector3.forward * gridNodesY * nodeDiametr / 2);

        //creates nodes
        for(int x = 0; x < gridNodesX; x++)
        {
            for (int y = 0; y < gridNodesY; y++)
            {
                //node position in world coordinate system
                Vector3 nodePosition = leftBottomGridPosition + new Vector3((x * nodeDiametr + nodeRadius), 0, (y * nodeDiametr + nodeRadius));
                //checks if it's passable object
                bool isWalkable = !(Physics.CheckSphere(nodePosition, nodeRadius, notPass));
                //creates new node
                grid[x, y] = new Node(isWalkable, nodePosition);
            }
        }
    }

    Node GetPlayerCoordinatsFromGrid()
    {
        //calculates player local(grid) coordinates
        var PlayerXPosition = (player.position.x + gridNodesX / 2) / gridNodesX;
        var PlayerYPosition = (player.position.z + gridNodesY / 2) / gridNodesY;
        //clamps layer local coordinates in range(0, 1)
        PlayerXPosition = Mathf.Clamp01(PlayerXPosition);
        PlayerYPosition = Mathf.Clamp01(PlayerYPosition);

        //sets num of nodes where player is
        int x = Mathf.RoundToInt((gridNodesX - 1) * PlayerXPosition);
        int y = Mathf.RoundToInt((gridNodesY - 1) * PlayerYPosition);

        return grid[x, y];
    }
    #endregion

    #region Gizmos function
    void OnDrawGizmos()
    {
        //if grid is instantiate
        if (grid != null)
        {
            Node playerNode = GetPlayerCoordinatsFromGrid(); //finds player node position
            //checks each node on grid
            foreach (Node n in grid)
            {
                //change color
                Gizmos.color = (n.isWalkable) ? Color.white : Color.red;
                if (playerNode == n)
                {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawWireCube(n.worlNodePosition, Vector3.one * (nodeDiametr));
            }
        }
    }
    #endregion
}
