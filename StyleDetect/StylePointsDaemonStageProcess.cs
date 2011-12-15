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


                    var accessRights = declaration.GetAccessRights();

                    if (accessRights == AccessRights.PUBLIC && !declaration.IsStatic && !declaration.IsVirtual &&
                        !declaration.IsOverride)
                    {

                        highlights.Add(new HighlightingInfo(docRange, new MakeMethodVirtualSuggestion(declaration)));
                    }
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

    /// <summary>
    /// Highlighting to show name of declared item.
    /// </summary>
    public class NameInfoHighlighting : IHighlighting
    {
        private readonly IMethodDeclaration declaration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameInfoHighlighting"/> class.
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        public NameInfoHighlighting(IMethodDeclaration declaration)
        {
            this.declaration = declaration;
        }

        /// <summary>
        /// Returns true if data (PSI, text ranges) associated with highlighting is valid
        /// </summary>
        public bool IsValid()
        {
            return declaration.IsValid();
        }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar"/> is <c>true</c>)
        /// To override the default mechanism of tooltip, mark the implementation class with
        /// <see cref="T:JetBrains.ReSharper.Daemon.DaemonTooltipProviderAttribute"/> attribute, and then this property will not be called
        /// </summary>
        public string ToolTip
        {
            get { return declaration.DeclaredName; }
        }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar"/> is <c>true</c>)
        /// </summary>
        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        /// <summary>
        /// Specifies the offset from the Range.StartOffset to set the cursor to when navigating
        /// to this highlighting. Usually returns <c>0</c>
        /// </summary>
        public int NavigationOffsetPatch
        {
            get { return 0; }
        }
    }
}