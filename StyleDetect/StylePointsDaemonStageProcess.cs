using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace Codevolve.StyleDetect
{
    /// <summary>
    /// Daemon stage process to gather and analyze "style points" (code examples that show particular code style).
    /// </summary>
    public class StylePointsDaemonStageProcess : IDaemonStageProcess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StylePointsDaemonStageProcess"/> class.
        /// </summary>
        /// <param name="process">The associated daemon process.</param>
        public StylePointsDaemonStageProcess(IDaemonProcess process)
        {
            DaemonProcess = process;
        }

        /// <summary>
        /// Executes the process.
        /// The process should check for <see cref="P:JetBrains.ReSharper.Daemon.IDaemonProcess.InterruptFlag"/> periodically (with intervals less than 100 ms)
        /// and throw <see cref="T:JetBrains.Application.Progress.ProcessCancelledException"/> if it is true.
        /// Failing to do so may cause the program to prevent user from typing while analysing the code.
        /// Stage results should be passed to <param name="commiter"/>. If DaemonStageResult is <c>null</c>, it means that no highlightings available
        /// </summary>
        public void Execute(Action<DaemonStageResult> commiter)
        {
            if (DaemonProcess.InterruptFlag)
            {
                return;
            }

            var sourceFile = DaemonProcess.SourceFile as ICSharpFile;
            if (sourceFile != null)
            {
                var highlights = new List<HighlightingInfo>();

                var processor = new RecursiveElementProcessor<IMethodDeclaration>(declaration =>
                {
                    var docRange = declaration.GetNameDocumentRange();

                    highlights.Add(new HighlightingInfo(docRange, new NameInfoHighlighting(declaration)));
                });

                sourceFile.ProcessDescendants(processor);

                commiter(new DaemonStageResult(highlights));
            }
        }

        /// <summary>
        /// Whole daemon process
        /// </summary>
        public IDaemonProcess DaemonProcess { get; private set; }
    }
}