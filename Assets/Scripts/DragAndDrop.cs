using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector3 startPosition;
    public ItemInventory stockHandler;
    public Texture2D cursorTextureNormal;
    public Texture2D cursorTextureBlocked;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public ItemInventory magnetInv;
    public ItemInventory normalInv;
    public ItemInventory fanInv;
    public ItemInventory pushInv;
    public ItemInventory powerInv;

    public enum KeyTypes
    {
        Normal,
        Inactive,
        Magnet,
        Fan,
        Conveyor,
        Power
    }

    public string keyTypes;
    public string description;
    public TextMeshProUGUI descriptionArea;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            KeyController keyController = hit.collider.GetComponentInChildren<KeyController>();

            if (keyController != null)
            {
                if (keyController.staticKey == true)
                {
                    Cursor.SetCursor(cursorTextureBlocked, hotSpot, cursorMode);
                    Debug.Log("Static");
                }
                else
                {
                    Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
                }
            }
            else
            {
                Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
            }
        }
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
            if (keyController != null && keyController.staticKey == false)
            {
                switch (keyController.keyType)
                {
                    case KeyController.KeyType.Magnet:
                        magnetInv.quantity += 1;
                        Debug.Log("Magnet2");
                        break;
                    case KeyController.KeyType.Normal:
                        normalInv.quantity += 1;
                        break;
                    case KeyController.KeyType.Fan:
                        fanInv.quantity += 1;
                        break;
                    case KeyController.KeyType.Conveyor:
                        pushInv.quantity += 1;
                        break;
                    case KeyController.KeyType.Power:
                        powerInv.quantity += 1;
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
                        keyController.UpdateKeyType();
                        break;
                    case "Normal":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Normal;
                        keyController.UpdateKeyType();
                        break;
                    case "Fan":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Fan;
                        keyController.UpdateKeyType();
                        break;
                    case "Conveyor":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Conveyor;
                        keyController.UpdateKeyType();
                        break;
                    case "Power":
                        stockHandler.quantity -= 1;
                        keyController.keyType = KeyController.KeyType.Power;
                        keyController.UpdateKeyType();
                        break;
                    default:
                        Debug.Log("Invalid key type");
                        break;
                }

            }
            else
            {
                Debug.Log("No KeyController component found on collider.");
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
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

