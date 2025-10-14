using Code.Player;
using UnityEngine;

namespace Tild.Menu
{
    public class ClickDetector : MonoBehaviour
    {
      
        [SerializeField] private LayerMask clickableLayer;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                DetectClickedObject();
            }
        }

        private void DetectClickedObject()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, clickableLayer))
            {
                GameObject clickedObj = hit.collider.gameObject;

               
                var chest = clickedObj.GetComponent<Chest>();
                if (chest != null)
                {
                    chest.OpenChest(); 
                }
            }
        }
    }
}