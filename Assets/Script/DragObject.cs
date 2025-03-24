using UnityEngine;
using TMPro;


public class DragObject : MonoBehaviour
{
    //this script for items


    private Camera cam;


    // To find out whether to get an item or not
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public GameObject Canvasobject;


    [Header("ItemsDetails")]
    public int Id;
    public float _Weight;
    public string _Name;
    public Type _Type;
    public Sprite icon;


    public TextMeshProUGUI IdTxt, WeightTxt, NameTxt, TypeTxt;

    public Animator backpackanimation;

    public enum Type
    {
        glass,
        wood,
        iron
    }

    void Start()
    {
        cam = Camera.main;



        //originalPosition = transform.position;
        //originalRotation = transform.rotation;

        IdTxt.text = "ID: " + Id.ToString();
        WeightTxt.text = "Weight: " + _Weight.ToString();
        NameTxt.text = "Name: " + _Name;
        TypeTxt.text = "Type: " + _Type.ToString();
    }

    void Update()
    {
        //Grabbing an object with the mouse

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Canvasobject.SetActive(true);
                    isDragging = true;
                    zDistance = cam.WorldToScreenPoint(transform.position).z;
                    offset = transform.position - GetMouseWorldPos();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Canvasobject.SetActive(false);
        }

        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zDistance;
        return cam.ScreenToWorldPoint(mousePoint);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Getting into the backpack

        if (other.name == "Backpack")
        {
            FindObjectOfType<Inventory>().AddItem(this);
            backpackanimation.SetBool("open", true);
            gameObject.SetActive(false);
        }
    }

    public void RestoreItem()
    {
        gameObject.SetActive(true);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

}
