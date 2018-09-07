using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// AI Base Cod which can find target in setable field of view and distance.
/// Version: 1.01
/// </summary>

//Require components for AI object
#region Require components
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
#endregion

public class AITarget : MonoBehaviour {

    #region Global variables
    [SerializeField]
    protected NavMeshAgent agent; //AI object NavMeshAgent component

    protected Transform targetTransform; //target Transform component

    [SerializeField]
    protected float LookAngle; //AI object field of view(in degrees)
    [SerializeField]
    protected float LookDistance; //AI object look distance in which AI can see the target

    protected int layer_mask; //layer mask to find target
    [SerializeField]
    protected string targetLayerName = "Player"; //layer target name
    #endregion

    #region Unity functions
    protected virtual void Start()
    {
        //Finding gameobject with tag "targetLayerName" and writing it into a variable "target"
        var target = GameObject.FindGameObjectWithTag(targetLayerName);
        if (target)
        {
            //if found - sets a variable "targetTransform" to Transform component of "target"
            targetTransform = target.transform;
        }
        else
        {
            //if haven't been found - sets a variable "targetTransform" to null
            targetTransform = null;
        }
        //setting layer mask
        SetLayerMask();
    }

    protected virtual void Update()
    {
        //looking for target
        FindTarget();
    }

    protected virtual void OnCollisionEnter(Collision obj)
    {
        //if collided with target
        if (obj.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //sets AI velocity to zero
            obj.rigidbody.velocity = Vector3.zero;
        }
    }
    #endregion

    #region User functions
    protected void SetLayerMask()
    {
        //moves the one bitwise in depending on layer name
        int player_mask = 1 << LayerMask.NameToLayer(targetLayerName); //targetLayerName - identifies the target
        int mesh_mask = 1 << LayerMask.NameToLayer("Mesh"); //layer "Mesh" - identifies objects through which AI can't see
        //makes layer mask which can saves layers with names "Player" and "Mesh"
        layer_mask = player_mask | mesh_mask;
    }

    //looking for target
    protected virtual void FindTarget()
    {
        //if target in AI field of view
        if (AngleCalculate() >= -LookAngle && AngleCalculate() <= LookAngle)
        {
            RaycastHit hitobject; //variable which contains information about ray-hit object 
            //make RayCast from AI position, in target direction, gets information about hit object, sets max distance of the ray, use layer mask, doesn't activate triggers
            if (Physics.Raycast(transform.position, targetTransform.position - transform.position, out hitobject, LookDistance , layer_mask, QueryTriggerInteraction.Ignore))
            {
                //if hit object have layer name "targetLayerName"
                if (hitobject.collider.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
                {
                    //Debug ray
                    Debug.DrawRay(transform.position, targetTransform.position - transform.position, Color.red);
                    //sets AI destination - target position
                    agent.destination = targetTransform.position;
                    //sets AI view at target
                    transform.LookAt(targetTransform.position);
                }
            }
        }
    }

    //calculates angle between AI and target
    protected float AngleCalculate()
    {
        //returns angle between AI and target
        return Vector3.Angle(transform.forward, targetTransform.position - transform.position);
    }

    #endregion
}
