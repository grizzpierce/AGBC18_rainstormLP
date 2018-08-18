using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Renderscale : MonoBehaviour {

    [Range(1f, 50f)]
    public float m_RenderScale = 1f; //Scale factor

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CommandBuffer cmd = new CommandBuffer() { name = "Renderscale" }; //Create the instruction buffer for renderscaling.
        {
            int scaledID = Shader.PropertyToID("_Renderscale"); //Fetch unique RT ID for the blits.

            //Create the downsized buffer to crunch the image
            cmd.GetTemporaryRT(scaledID,
                               source.width  / (int)m_RenderScale,
                               source.height / (int)m_RenderScale,
                               0, FilterMode.Point);

            cmd.Blit(source, scaledID);      //Copy the screen into the smaller texture.
            cmd.Blit(scaledID, destination); //Upsampling it back up with point sampling to achieve the aliasing effect.
        }
        Graphics.ExecuteCommandBuffer(cmd); //Submit.
    }

}
