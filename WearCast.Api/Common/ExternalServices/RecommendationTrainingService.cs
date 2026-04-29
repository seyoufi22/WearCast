using System.Diagnostics;
using System.Text;

namespace WearCast.Api.Common.ExternalServices
{
    public interface IRecommendationTrainingService
    {
        Task TrainAsync();
    }

    public class RecommendationTrainingService(IConfiguration configuration, ILogger<RecommendationTrainingService> logger) : IRecommendationTrainingService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<RecommendationTrainingService> _logger = logger;

        public async Task TrainAsync()
        {
            var pythonExe = _configuration["PythonSettings:PythonExecutable"] ?? "python";
            var relativeScriptPath = _configuration["PythonSettings:TrainingScriptPath"];
            var relativeWorkingDir = _configuration["PythonSettings:WorkingDirectory"];

            if (string.IsNullOrEmpty(relativeScriptPath))
            {
                _logger.LogError("Recommendation training failed: TrainingScriptPath is not configured.");
                return;
            }

            // Resolve relative paths to absolute paths based on app root
            var baseDir = AppContext.BaseDirectory;
            // Go up from bin/Debug/net8.0 to the project root in development, 
            // or use baseDir directly in production if the folders are hosted together.
            var scriptPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", relativeScriptPath));
            var workingDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", relativeWorkingDir));

            // Fallback for production (wherebin folder might not have 4 parent levels)
            if (!File.Exists(scriptPath))
            {
                 scriptPath = Path.GetFullPath(Path.Combine(baseDir, relativeScriptPath));
                 workingDir = Path.GetFullPath(Path.Combine(baseDir, relativeWorkingDir));
            }

            _logger.LogInformation("Starting Recommendation System Training Job at {ScriptPath}...", scriptPath);

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = $"\"{scriptPath}\" --from-db",
                    WorkingDirectory = workingDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = startInfo };
                
                var output = new StringBuilder();
                var error = new StringBuilder();

                process.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    _logger.LogInformation("Recommendation System Training completed successfully.");
                    _logger.LogDebug("Output: {Output}", output.ToString());
                }
                else
                {
                    _logger.LogError("Recommendation System Training failed with exit code {ExitCode}. Error: {Error}", process.ExitCode, error.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while running the Recommendation System Training process.");
            }
        }
    }
}
