using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float RotationPerSecond = 1;
    private bool _rotate = true;
    

    protected void Update()
    {
        if (_rotate) RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);



       /* Color col = RenderSettings.skybox.GetColor("_Tint");
        RenderSettings.skybox.SetColor("_Tint", col.r + Time.time, col.g  )*/

    }

   /* public void ToggleSkyboxRotation()
    {
        _rotate = !_rotate;
    }*/
}