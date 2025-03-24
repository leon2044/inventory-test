using UnityEngine;
using TMPro;

public class DragObject : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;
    private Vector3 originalPosition; // 🚀 ذخیره موقعیت اولیه
    private Quaternion originalRotation; // 🚀 ذخیره چرخش اولیه

    public GameObject Canvasobject;

    public int Id;
    public float _Weight;
    public string _Name;
    public Type _Type;

    public TextMeshProUGUI IdTxt, WeightTxt, NameTxt, TypeTxt;

    public Sprite icon; // آیکون آیتم

  

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


        // 🚀 ذخیره موقعیت و چرخش اولیه هنگام شروع
        //originalPosition = transform.position;
        //originalRotation = transform.rotation;

        IdTxt.text = "ID: " + Id.ToString();
        WeightTxt.text = "Weight: " + _Weight.ToString();
        NameTxt.text = "Name: " + _Name;
        TypeTxt.text = "Type: " + _Type.ToString();
    }

    void Update()
    {
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
        if (other.name == "Backpack")
        {
            print("ok");
            FindObjectOfType<Inventory>().AddItem(this);
            backpackanimation.SetBool("open", true);
            gameObject.SetActive(false); // 🚀 به جای حذف، غیرفعال شود
        }
    }

    public void RestoreItem()
    {
        // 🚀 بازگرداندن آیتم به محیط بازی
        gameObject.SetActive(true);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        Debug.Log($"آیتم {_Name} به مکان اولیه خود بازگشت.");
    }

}
