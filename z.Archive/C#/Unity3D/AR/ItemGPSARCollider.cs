using UnityEngine;

namespace ItemGPSAR
{
    public class ItemGPSARCollider : MonoBehaviour
    {
        [SerializeField]
        FindItemGPSAR script;

        protected virtual void OnTriggerEnter(Collider obj)
        {
            if (obj.tag == "Player")
            {
                script.currentCorotine = StartCoroutine(script.StateMachine());
            }
        }

        private void OnTriggerExit(Collider obj)
        {
            if (obj.tag == "Player")
            {
                script.transform.localScale = transform.parent.localScale * 5f;
                script.transform.position = transform.parent.position;
                if (script.currentCorotine != null)
                {
                    StopCoroutine(script.currentCorotine);
                }
            }
        }
    }
}