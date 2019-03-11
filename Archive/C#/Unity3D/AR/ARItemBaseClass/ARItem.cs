using UnityEngine;

namespace ItemGPSAR
{

    public class ARItem : MonoBehaviour
    {

        [HideInInspector]
        public Transform targetTrans;                      // Player's transform
        [SerializeField]
        protected string ARCameraName = "ARCamera";
        [SerializeField]
        string targetTag = "Player";

        [SerializeField]
        float scaleCoof = 1f;

        protected virtual void Awake()
        {
            targetTrans = GameObject.FindGameObjectWithTag(targetTag).transform;
        }


        public virtual float distanceToTarget
        {
            get { return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetTrans.position.x, targetTrans.position.z)); }
        }

        public virtual void ChangeItemScale(float distance)
        {
            float scale = scaleCoof / distance;
            transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(scale, scale, scale), 5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 8,
                                             transform.position.z);
        }

        public virtual void PickUping()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
