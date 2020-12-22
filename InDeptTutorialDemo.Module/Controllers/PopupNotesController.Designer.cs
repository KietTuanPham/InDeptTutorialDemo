
namespace InDeptTutorialDemo.Module.Controllers
{
    partial class PopupNotesController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ShowNoteAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ShowNoteAction
            // 
            this.ShowNoteAction.AcceptButtonCaption = null;
            this.ShowNoteAction.CancelButtonCaption = null;
            this.ShowNoteAction.Caption = "Show Notes";
            this.ShowNoteAction.Category = "Edit";
            this.ShowNoteAction.ConfirmationMessage = null;
            this.ShowNoteAction.Id = "ShowNoteAction";
            this.ShowNoteAction.ToolTip = null;
            this.ShowNoteAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ShowNoteAction_CustomizePopupWindowParams);
            this.ShowNoteAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ShowNoteAction_Execute);
            // 
            // PopupNotesController
            // 
            this.Actions.Add(this.ShowNoteAction);
            this.TargetObjectType = typeof(InDeptTutorialDemo.Module.BusinessObjects.DemoTask);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ShowNoteAction;
    }
}
