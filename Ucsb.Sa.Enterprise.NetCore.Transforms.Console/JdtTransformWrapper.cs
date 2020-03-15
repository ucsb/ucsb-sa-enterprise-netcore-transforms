using Microsoft.VisualStudio.Jdt;
using System.IO;

namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Console
{
    public class JdtTransformWrapper : IJdtTransformWrapper
    {
        private readonly IJsonTransformationLogger _jsonTransformationLogger;

        private const string defaultOutputFilename = "output.json";

        public JdtTransformWrapper(IJsonTransformationLogger jsonTransformationLogger)
        {
            _jsonTransformationLogger = jsonTransformationLogger;
        }

        public JsonTransformation CreateJsonTransformation(string transformFile)
        {
            return new JsonTransformation(transformFile, _jsonTransformationLogger);
        }

        public void DoTransform(string sourceFile, string transformFile, string outputFile)
        {
            _jsonTransformationLogger.LogMessage("Attempting to perform transform.");
            _jsonTransformationLogger.LogMessage($"Transforming source file: {sourceFile} using file {transformFile}");

            var absoluteSourcePath = Path.GetFullPath(sourceFile);
            JsonTransformation transformation = CreateJsonTransformation(transformFile);

            // apply transform
            using (Stream outputStream = transformation.Apply(absoluteSourcePath))
            {
                using (StreamReader streamReader = new StreamReader(outputStream))
                {
                    string finalText = streamReader.ReadToEnd();

                    if (string.IsNullOrWhiteSpace(outputFile))
                    {
                        outputFile = defaultOutputFilename;
                        _jsonTransformationLogger.LogWarning($"No output filename was given. Using default filename of \"{defaultOutputFilename}\"");
                    }

                    _jsonTransformationLogger.LogMessage($"Writing transformed file to {outputFile}");
                    File.WriteAllText($"{outputFile}", finalText);
                }
            }

            _jsonTransformationLogger.LogMessage("Done and done.");
        }

    }
}