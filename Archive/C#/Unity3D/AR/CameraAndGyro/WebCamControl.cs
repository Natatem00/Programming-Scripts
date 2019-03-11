using UnityEngine;

namespace Controllers.AR
{
	public class WebCamControl : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private Transform _quad;
		[SerializeField] private WebCamTexture _webcamTexture;

		private float _distance;

		public void StopCamera()
		{
			_webcamTexture.Stop();
		}

        private void Start()
        {
            Init();
        }

        private void LateUpdate()
		{
            if (_webcamTexture)
            {
                float ratioTex = _webcamTexture.width / (float)_webcamTexture.height;
                float frustumHeight = 2.0f * _distance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                float scaleY = _webcamTexture.videoVerticallyMirrored ? -1.0f : 1.0f;
                _quad.localScale = new Vector3(frustumHeight * ratioTex, scaleY * frustumHeight, 1);
            }
		}

		protected void Init()
		{
			_distance = _quad.localPosition.z;
            try
            {
                _webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name, 1920, 1080, 30);
            }
            catch
            {
                Debug.Log("Can't find camera on device");
                return;
            }
			_quad.GetComponent<Renderer>().material.mainTexture = _webcamTexture;
			_webcamTexture.Play();
		}
	}
}