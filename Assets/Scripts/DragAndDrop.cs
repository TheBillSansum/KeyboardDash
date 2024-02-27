using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector3 startPosition;


    public enum KeyTypes
    {
        Normal,
        Inactive,
        Magnet,
        Fan,
        Conveyor
    }

    public string keyTypes;
    public string description;
    public TextMeshProUGUI descriptionArea;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }


    public void Start()
    {
        startPosition = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        descriptionArea.text = description;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponentInChildren<KeyController>())
            {
                switch (keyTypes)
                {
                    case "Magnet":
                        hit.collider.GetComponentInChildren<KeyController>().keyType = (KeyController.KeyType)KeyTypes.Magnet;

                        break;

                    case "Normal":
                        hit.collider.GetComponentInChildren<KeyController>().keyType = (KeyController.KeyType)KeyTypes.Normal;
                        break;

                    //case KeyType.Inactive:
                    //    break;

                    case "Fan":
                        hit.collider.GetComponentInChildren<KeyController>().keyType = (KeyController.KeyType)KeyTypes.Fan;
                        break;

                    case "Conveyor":
                        hit.collider.GetComponentInChildren<KeyController>().keyType = (KeyController.KeyType)KeyTypes.Conveyor;
                        break;
                }   

                hit.collider.GetComponentInChildren<KeyController>().UpdateKeyType();
            }
            else
            {
                Debug.Log("No collider detected.");
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnPointerEnter()
    {
        descriptionArea.text = description;
    }
}

