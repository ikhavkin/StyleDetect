using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;

namespace Codevolve.StyleDetect
{
    /// <summary>
    /// Daemon stage to gather and analyze "style points" (code examples that show particular code style).
    /// </summary>
    [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
    public class StylePointsDaemonStage : IDaemonStage
    {
        /// <summary>
        /// Creates a code analysis process corresponding to this stage for analysing a file.
        /// </summary>
        /// <returns>
        /// Code analysis process to be executed or <c>null</c> if this stage is not available for this file.
        /// </returns>
        public IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings,
                                                 DaemonProcessKind processKind)
        {
            return new StylePointsDaemonStageProcess(process);
        }

        /// <summary>
        /// Check the error stripe indicator necessity for this stage after processing given <paramref name="sourceFile"/>
        /// </summary>
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settingsStore)
        {
            return ErrorStripeRequest.NONE;
        }
    }
}