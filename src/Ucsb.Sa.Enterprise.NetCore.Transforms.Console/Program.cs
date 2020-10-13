using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Jdt;
using Serilog;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ucsb.Sa.Enterprise.NetCore.Transforms.Console
{
    [Command(Description = "A commandline wrapper around Microsoft.VisualStudio.Jdt")]
    public class Program
    {
        #region CommandArguments
        [Required]
        [FileExists]
        [Option("-s|--source <SOURCE>", "Filename of the source json file.", CommandOptionType.SingleValue)]
        public string OptionSourceFile { get; set; }

        [Required]
        [Option("-t|--transform <TRANSFORM>", "Filename of the transform file.", CommandOptionType.SingleValue)]
        public string OptionTransformFile { get; set; }

        [Option("-o|--output <OUTPUT>", "Filename of the transform file.", CommandOptionType.SingleValue)]
        public string OptionOutputFile { get; set; }

        [Option("-v|--verbose <VERBOSE>", "Verbose", CommandOptionType.NoValue)]
        public bool OptionVerbose { get; set; }
        #endregion

        #region PrivateVariables
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IJdtTransformWrapper _jdtTransformWrapper;
        private readonly CommandLineApplication<Program> _commandLineApp;
        private IConsole _console => PhysicalConsole.Singleton;
        #endregion

        public Program(ILogger<Program> logger, CommandLineApplication<Program> commandLineApplication, IJdtTransformWrapper jdtTransformWrapper)
        {
            _logger = logger;
            _commandLineApp = commandLineApplication;
            _jdtTransformWrapper = jdtTransformWrapper;
        }

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(
                    new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build())
                .CreateLogger();

            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IJsonTransformationLogger, JsonTransformerLoggerAdapter>()
                    .AddTransient<IJdtTransformWrapper, JdtTransformWrapper>()
                    .AddLogging(c => c.AddSerilog())
                    .BuildServiceProvider();

                using (var app = new CommandLineApplication<Program>())
                {
                    app.HelpOption("-?|-h|--help");
                    app.Conventions
                        .UseDefaultConventions()
                        .UseConstructorInjection(serviceProvider);

                    return app.Execute(args);
                }
            }
            catch (UnrecognizedCommandParsingException unrecognized)
            {
                var console = PhysicalConsole.Singleton;
                console.Out.WriteLine(unrecognized.Message);

                if (unrecognized.NearestMatches.Any())
                {
                    console.Out.WriteLine($"Did you mean: {string.Join(", ", unrecognized.NearestMatches.ToList())}?");
                }

                return 1;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Transformer terminated unexpectedly.");
                return 1;
            }
        }

        private void OnExecute()
        {
            _logger.LogDebug("Calling DoTransform.");
            _logger.LogDebug(_commandLineApp.Model.OptionSourceFile);

            _jdtTransformWrapper.DoTransform(_commandLineApp.Model.OptionSourceFile,
                _commandLineApp.Model.OptionTransformFile,
                _commandLineApp.Model.OptionOutputFile);

        }
    }
}