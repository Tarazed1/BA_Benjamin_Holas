using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenuForRenderPipeline("Custom/DistanceBasedDoF", typeof(UniversalRenderPipeline))]
//[PostProcess(typeof(CustomDoFEffectRenderer), PostProcessEvent.AfterStack, "Custom/DistanceBasedDoF")]
public sealed class CustomDoFEffect : VolumeComponent, IPostProcessComponent 
{
    [Tooltip("Blur Strength")]
    public FloatParameter blurStrength = new FloatParameter(10f);

    [Tooltip("Focus Distance")]
    public FloatParameter focusDistance = new FloatParameter (10f);

    [Tooltip("Focus Range")]
    public FloatParameter focusRange = new FloatParameter (10f);

    public bool IsActive()
    {
        return blurStrength.value > 0;
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}

//public sealed class CustomDoFEffectRenderer : PostProcessEffectRenderer<CustomDoFEffect>
//{
//    public override void Render(PostProcessRenderContext context)
//    {
//        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/DistanceBasedDoF"));
//        sheet.properties.SetFloat("_BlurStrength", settings.blurStrength);
//        sheet.properties.SetFloat("_FocusDistance", settings.focusDistance);
//        sheet.properties.SetFloat("_FocusRange", settings.focusRange);
//        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
//    }
//}
