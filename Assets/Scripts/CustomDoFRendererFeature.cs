using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomDoFRendererFeature : ScriptableRendererFeature
{
    class CustomDoFRenderPass : ScriptableRenderPass
    {
        private Material dofMaterial;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryRenderTarget;

        public CustomDoFRenderPass(Material material)
        {
            dofMaterial = material;
            temporaryRenderTarget.Init("_TemporaryRenderTarget");
        }

        public void Setup(RenderTargetIdentifier sourceIdentifier)
        {
            this.source = sourceIdentifier;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("DistanceBasedDoF");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(temporaryRenderTarget.id, opaqueDesc, FilterMode.Bilinear);
            Blit(cmd, source, temporaryRenderTarget.Identifier(), dofMaterial, 0);
            Blit(cmd, temporaryRenderTarget.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private CustomDoFRenderPass dofPass;
    public Material dofMaterial;

    public override void Create()
    {
        //dofMaterial = CoreUtils.CreateEngineMaterial("Hidden/Custom/DistanceBasedDoF");
        dofPass = new CustomDoFRenderPass(dofMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        dofPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(dofPass);
    }
}
