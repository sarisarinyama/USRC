using UnityEngine;

public class OutlinePreviewer : MonoBehaviour
{
    public Material previewer;
    public MeshRenderer mesh;
    // public TogglePicker togglePicker;


    private void Start()
    {
        // togglePicker.OnValueChanged += OnValueChangedLocal;
    }

    private void OnDestroy()
    {
        // if (togglePicker != null) togglePicker.OnValueChanged -= OnValueChangedLocal;
    }

    public void OnValueChangedLocal(bool value)
    {
        if (value)
            previewer.SetFloat("_OutlineWidth", 1);
        // previewer.SetColor("_OutlineColor", new Color(
        //     previewer.GetColor("_OutlineColor").r,
        //     previewer.GetColor("_OutlineColor").g,
        //     previewer.GetColor("_OutlineColor").b,
            // 1.0f)
        // );
        else
            previewer.SetFloat("_OutlineWidth", 0);
        // previewer.SetColor("_OutlineColor", new Color(
        //     previewer.GetColor("_OutlineColor").r,
        //     previewer.GetColor("_OutlineColor").g,
        //     previewer.GetColor("_OutlineColor").b,
        //     0.0f)
        // );
        // previewer.enabled= value;
    }
}