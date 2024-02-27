using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector3 startPosition;
    public ItemInventory stockHandler;

    public ItemInventory magnetInv;
    public ItemInventory normalInv;
    public ItemInventory fanInv;
    public ItemInventory pushInv;

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
            KeyController keyController = hit.collider.GetComponentInChildren<KeyController>();
            if (keyController != null)
            {             
                
                switch (keyController.keyType)
                {
                    case KeyController.KeyType.Magnet:
                        magnetInv.quantity +=1;
                        Debug.Log("Magnet2");
                        break;
                    case KeyController.KeyType.Normal:
                        normalInv.quantity +=1;
                        break;
                    case KeyController.KeyType.Fan:
                        fanInv.quantity +=1;
                        break;
                    case KeyController.KeyType.Conveyor:
                        pushInv.quantity +=1;
                        break;
                    default:
                        Debug.Log("Invalid key type");
                        break;
                }    
                switch (keyTypes)
                {
                    case "Magnet":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Magnet;
                        break;
                    case "Normal":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Normal;
                        break;
                    case "Fan":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Fan;
                        break;
                    case "Conveyor":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Conveyor;
                        break;
                    default:
                        Debug.Log("Invalid key type");
                        break;
                }




                
                keyController.UpdateKeyType();
            }
            else
            {
                Debug.Log("No KeyController component found on collider.");
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Test1");
        if (stockHandler.inStock)
        {
            stockHandler.quantity -= 1;
            Debug.Log(stockHandler.quantity);
            startPosition = transform.position;
        }
    }

    public void OnPointerEnter()
    {
        descriptionArea.text = description;
    }
}

