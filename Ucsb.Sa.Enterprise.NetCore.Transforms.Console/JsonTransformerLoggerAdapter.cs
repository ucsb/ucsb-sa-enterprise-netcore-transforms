using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Jdt;
using System;

namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Console
{
    /// <summary>
    /// An adapter to pass logging information from the parser to the common ILogger interface
    /// </summary>
    public class JsonTransformerLoggerAdapter : IJsonTransformationLogger
    {
        private readonly ILogger _logger;

        public JsonTransformerLoggerAdapter(ILogger<JsonTransformation> logger)
        {
            _logger = logger;
        }

        public void LogMessage(string message) => _logger.LogInformation(message);

        public void LogMessage(string message, string fileName, int lineNumber, int linePosition)
        {
            _logger.LogInformation("{message}, in file {fileName), at line {lineNumber}, position {linePosition}", message, fileName, lineNumber, linePosition);
        }

        public void LogWarning(string message) => _logger.LogWarning(message);

        public void LogWarning(string message, string fileName) => _logger.LogWarning("{message}, in {fileName}", message, fileName);

        public void LogWarning(string message, string fileName, int lineNumber, int linePosition)
        {
            _logger.LogWarning("{message}, in file {fileName), at line {lineNumber}, position {linePosition}", message, fileName, lineNumber, linePosition);
        }

        public void LogError(string message) => _logger.LogError(message);

        public void LogError(string message, string fileName, int lineNumber, int linePosition)
        {
            _logger.LogError("{message}, in file {fileName), at line {lineNumber}, position {linePosition}", message, fileName, lineNumber, linePosition);
        }

        public void LogErrorFromException(Exception ex) => _logger.LogError("Exception occurred.", ex);

        public void LogErrorFromException(Exception ex, string fileName, int lineNumber, int linePosition)
        {
            _logger.LogError("{@ex}, in file {fileName), at line {lineNumber}, position {linePosition}", ex, fileName, lineNumber, linePosition);
        }
    }
}