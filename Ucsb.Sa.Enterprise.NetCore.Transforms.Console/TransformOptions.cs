namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Console
{
    public class TransformOptions
    {
        public string SourceFile { get; set; }

        public string TransformFile { get; set; }

        public string OutputFile { get; set; }

        public bool Verbose { get; set; }
    }
}