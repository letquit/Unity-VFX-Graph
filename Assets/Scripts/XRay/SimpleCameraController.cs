using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f;       // 键盘移动速度
    public float mouseSensitivity = 2.0f; // 鼠标灵敏度
    public float scrollSpeed = 2.0f;     // 滚轮缩放速度

    private float yaw = 0.0f; // 水平旋转角度
    private float pitch = 0.0f; // 垂直旋转角度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 锁定鼠标在屏幕内并隐藏鼠标指针
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 键盘移动控制
        float moveForward = Input.GetAxis("Vertical") * moveSpeed;
        float moveRight = Input.GetAxis("Horizontal") * moveSpeed;
        transform.Translate(Vector3.forward * moveForward * Time.deltaTime);
        transform.Translate(Vector3.right * moveRight * Time.deltaTime);

        // 鼠标移动控制（无需点击）
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -90.0f, 90.0f); // 限制俯仰角度
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        // 鼠标滚轮缩放控制
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        transform.Translate(Vector3.forward * scroll * Time.deltaTime);
    }
}
