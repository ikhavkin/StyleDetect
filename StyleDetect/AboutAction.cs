using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace Codevolve.StyleDetect
{
    /// <summary>
    /// About action.
    /// </summary>
    [ActionHandler("Codevolve.StyleDetect.About")]
    public class AboutAction : IActionHandler
    {
        #region IActionHandler Members

        /// <summary>
        /// Updates action visual presentation. If presentation.Enabled is set to false, Execute
        /// will not be called.
        /// </summary>
        /// <param name="context">DataContext</param>
        /// <param name="presentation">presentation to update</param>
        /// <param name="nextUpdate">delegate to call</param>
        /// <returns></returns>
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        /// <summary>
        /// Executes action. Called after Update, that set ActionPresentation.Enabled to true.
        /// </summary>
        /// <param name="context">DataContext</param>
        /// <param name="nextExecute">delegate to call</param>
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show("StyleDetect", "About StyleDetect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}