using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace VisualStudioExtension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AICommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e");

        /// <summary>
        /// VS Package that provides this command
        /// </summary>
        private readonly AsyncPackage package;

        private readonly VisualStudioExtension _extension;

        /// <summary>
        /// Initializes a new instance of the <see cref="AICommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="extension">The extension service.</param>
        private AICommand(AsyncPackage package, VisualStudioExtension extension)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            _extension = extension ?? throw new ArgumentNullException(nameof(extension));

            if (AsyncPackage.GetGlobalService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.ExecuteAsync, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AICommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="extension">The extension service.</param>
        /// <returns>A Task representing the async work of command initialization.</returns>
        public static async Task InitializeAsync(AsyncPackage package, VisualStudioExtension extension)
        {
            // Switch to the main thread - the call to AddCommand in AICommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            Instance = new AICommand(package, extension);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void ExecuteAsync(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                // Example implementation - you can replace with your specific logic
                string result = await _extension.PerformAIAction("YourEndpoint", new { YourData = "YourValue" });
                
                // Show a message box with the result
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    result,
                    "AI Action Result",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    $"Error: {ex.Message}",
                    "AI Action Error",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }
    }
    
    internal static class AICommandPackage
    {
        public static async Task InitializeAsync(AsyncPackage package, VisualStudioExtension extension)
        {
            await AICommand.InitializeAsync(package, extension);
        }
    }
}
