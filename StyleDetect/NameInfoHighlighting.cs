using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Codevolve.StyleDetect
{
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