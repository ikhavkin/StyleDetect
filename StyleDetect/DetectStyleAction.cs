using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.Util;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace Codevolve.StyleDetect
{
    /// <summary>
    /// Detect style action to show statistics.
    /// </summary>
    [ActionHandler("Codevolve.StyleDetect.DetectStyle")]
    public class DetectStyleAction : IActionHandler
    {
        #region Implementation of IActionHandler

        /// <summary>
        /// Updates action visual presentation. If presentation.Enabled is set to false, Execute
        ///             will not be called.
        /// </summary>
        /// <param name="context">DataContext</param><param name="presentation">presentation to update</param><param name="nextUpdate">delegate to call</param>
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var solution = context.GetData(DataConstants.SOLUTION);
            bool visible = solution != null;

            if (!visible)
            {
                presentation.Visible = false;
            }

            return visible;
        }

        /// <summary>
        /// Executes action. Called after Update, that set ActionPresentation.Enabled to true.
        /// </summary>
        /// <param name="context">DataContext</param><param name="nextExecute">delegate to call</param>
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.ShowInfo("Test");
        }

        #endregion
    }
}