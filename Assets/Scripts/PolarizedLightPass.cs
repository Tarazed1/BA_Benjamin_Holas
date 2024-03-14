using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PolarizedLightPass : ScriptableRenderPass
{
    private Material polarizedLightMaterial = null;
    private RenderTargetIdentifier source { get; set; }
    private RenderTargetHandle temporaryRenderTargetHandle;

    public PolarizedLightPass(Shader shader)
    {
        polarizedLightMaterial = CoreUtils.CreateEngineMaterial(shader);
        temporaryRenderTargetHandle.Init("_TemporaryRenderTargetHandle");
    }

    public void Setup(RenderTargetIdentifier source)
    {
        this.source = source;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("PolarizedLightPass");

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        opaqueDesc.depthBufferBits = 0;

        cmd.GetTemporaryRT(temporaryRenderTargetHandle.id, opaqueDesc, FilterMode.Bilinear);
        Blit(cmd, source, temporaryRenderTargetHandle.Identifier(), polarizedLightMaterial);
        Blit(cmd, temporaryRenderTargetHandle.Identifier(), source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
