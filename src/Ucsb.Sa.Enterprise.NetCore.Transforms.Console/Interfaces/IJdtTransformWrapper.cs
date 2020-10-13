using Microsoft.VisualStudio.Jdt;

namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Console
{
    public interface IJdtTransformWrapper
    {
        void DoTransform(string sourceFile, string transformFile, string outputFile);
        JsonTransformation CreateJsonTransformation(string transformFile);
    }
}