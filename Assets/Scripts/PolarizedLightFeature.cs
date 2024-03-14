using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PolarizedLightFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class PolarizedLightSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Shader shader;
    }

    public PolarizedLightSettings settings = new PolarizedLightSettings();
    private PolarizedLightPass pass;

    public override void Create()
    {
        pass = new PolarizedLightPass(settings.shader)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(pass);
    }
}
