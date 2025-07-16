using UnityEngine;

public class SwitchLayer : MonoBehaviour
{
    public string defaultLayerName = "Default";
    public string xRayLayerName = "RenderAbove"; // 对应 Layer 6

    private bool xRayActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            xRayActive = !xRayActive;

            string targetLayerName = xRayActive ? xRayLayerName : defaultLayerName;
            int layerNum = LayerMask.NameToLayer(targetLayerName);

            if (layerNum >= 0 && layerNum <= 31)
            {
                gameObject.layer = layerNum;
                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, layerNum);
            }
            else
            {
                Debug.LogError($"Layer '{targetLayerName}' is invalid or not in range [0..31]");
            }
        }
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
