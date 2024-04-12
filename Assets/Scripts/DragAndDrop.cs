using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
/// <summary>
/// Functionality for Dragging and Dropping the different types of keys from the players inventory onto the board
/// </summary>
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

    /// <summary>
    /// <para> Normal</para>
    /// <para>Inactive</para>
    /// <para>Magnet</para>    
    /// <para>Fan</para>
    /// <para>Conveyor</para>
    /// <para>Power</para>
    /// </summary>
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


    /// <summary>
    /// When click is held and dragged the image of the chosen key will follow the cursor,
    /// <para> If a valid key object is detected below the cursor, cursor will stay white, otherwise it will become red</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; //Sets the position

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Sends a raycast out of cam
        RaycastHit hit; //Raycast

        if (Physics.Raycast(ray, out hit)) //If an object is detected
        {
            KeyController keyController = hit.collider.GetComponentInChildren<KeyController>(); //Get the KeyController script from hit object

            if (keyController != null) //If its not null
            {
                if (keyController.staticKey == true) //If the keyController is static, 
                {
                    Cursor.SetCursor(cursorTextureBlocked, hotSpot, cursorMode); //Set cursor image to a red 
                }
                else
                {
                    Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode); //Otherwise, it assumes a non-static key and sets to white
                }
            }
            else
            {
                Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode); //Resets to white
            }
        }
    }


    public void Start()
    {
        startPosition = transform.position; //Gets the spawn location and saves for later use when unselected
    }


    /// <summary>
    /// When the player releases the mouse down,
    /// <para> If a valid key is below in the raycast, swap that key and adjust the inventory to be accurate</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition; //Reset location to start position
        descriptionArea.text = description;
        Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode); //Ensure that cursor has been set back to white, if not done sometimes bugs out

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Get raycast one last time
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            KeyController keyController = hit.collider.GetComponentInChildren<KeyController>();
            if (keyController != null && keyController.staticKey == false)
            {
                switch (keyController.keyType) //Add whatever key was replaced to their inventory 
                {
                    case KeyController.KeyType.Magnet:
                        magnetInv.quantity += 1;
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
                switch (keyTypes) //Remove whatever you just placed from the board from the players inventory, set the key and update the key
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
            startPosition = transform.position;
        }
    }
    /// <summary>
    /// When hovered, display the description text for that key
    /// </summary>
    public void OnPointerEnter()
    {
        descriptionArea.text = description;
    }
}

