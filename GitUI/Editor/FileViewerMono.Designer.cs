﻿namespace GitUI.Editor
{
    partial class FileViewerMono
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileViewerMono));
            this.TextEditor = new System.Windows.Forms.RichTextBox();
            this.ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ignoreWhitespaceChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseNumberOfLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descreaseNumberOfLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEntireFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.treatAllFilesAsTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNonprintableCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileviewerToolbar = new System.Windows.Forms.ToolStrip();
            this.nextChangeButton = new System.Windows.Forms.ToolStripButton();
            this.previousChangeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.increaseNumberOfLines = new System.Windows.Forms.ToolStripButton();
            this.DecreaseNumberOfLines = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showEntireFileButton = new System.Windows.Forms.ToolStripButton();
            this.showNonPrintChars = new System.Windows.Forms.ToolStripButton();
            this.ignoreWhiteSpaces = new System.Windows.Forms.ToolStripButton();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.ContextMenu.SuspendLayout();
            this.fileviewerToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TextEditor
            // 
            this.TextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextEditor.Location = new System.Drawing.Point(0, 0);
            this.TextEditor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.Size = new System.Drawing.Size(660, 512);
            this.TextEditor.TabIndex = 0;
            this.TextEditor.Text = "";
            // 
            // ContextMenu
            // 
            this.ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.copyPatchToolStripMenuItem,
            this.toolStripSeparator1,
            this.ignoreWhitespaceChangesToolStripMenuItem,
            this.increaseNumberOfLinesToolStripMenuItem,
            this.descreaseNumberOfLinesToolStripMenuItem,
            this.showEntireFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.treatAllFilesAsTextToolStripMenuItem,
            this.showNonprintableCharactersToolStripMenuItem});
            this.ContextMenu.Name = "ContextMenu";
            this.ContextMenu.Size = new System.Drawing.Size(275, 214);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // copyPatchToolStripMenuItem
            // 
            this.copyPatchToolStripMenuItem.Name = "copyPatchToolStripMenuItem";
            this.copyPatchToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.copyPatchToolStripMenuItem.Text = "Copy patch";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(271, 6);
            // 
            // ignoreWhitespaceChangesToolStripMenuItem
            // 
            this.ignoreWhitespaceChangesToolStripMenuItem.Name = "ignoreWhitespaceChangesToolStripMenuItem";
            this.ignoreWhitespaceChangesToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.ignoreWhitespaceChangesToolStripMenuItem.Text = "Ignore whitespace changes";
            // 
            // increaseNumberOfLinesToolStripMenuItem
            // 
            this.increaseNumberOfLinesToolStripMenuItem.Name = "increaseNumberOfLinesToolStripMenuItem";
            this.increaseNumberOfLinesToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.increaseNumberOfLinesToolStripMenuItem.Text = "Increase number of lines visible";
            // 
            // descreaseNumberOfLinesToolStripMenuItem
            // 
            this.descreaseNumberOfLinesToolStripMenuItem.Name = "descreaseNumberOfLinesToolStripMenuItem";
            this.descreaseNumberOfLinesToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.descreaseNumberOfLinesToolStripMenuItem.Text = "Decrease number of lines visible";
            // 
            // showEntireFileToolStripMenuItem
            // 
            this.showEntireFileToolStripMenuItem.Name = "showEntireFileToolStripMenuItem";
            this.showEntireFileToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.showEntireFileToolStripMenuItem.Text = "Show entire file";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(271, 6);
            // 
            // treatAllFilesAsTextToolStripMenuItem
            // 
            this.treatAllFilesAsTextToolStripMenuItem.Name = "treatAllFilesAsTextToolStripMenuItem";
            this.treatAllFilesAsTextToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.treatAllFilesAsTextToolStripMenuItem.Text = "Treat all files as text";
            // 
            // showNonprintableCharactersToolStripMenuItem
            // 
            this.showNonprintableCharactersToolStripMenuItem.Name = "showNonprintableCharactersToolStripMenuItem";
            this.showNonprintableCharactersToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.showNonprintableCharactersToolStripMenuItem.Text = "Show nonprinting characters";
            // 
            // fileviewerToolbar
            // 
            this.fileviewerToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileviewerToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.fileviewerToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextChangeButton,
            this.previousChangeButton,
            this.toolStripSeparator3,
            this.increaseNumberOfLines,
            this.DecreaseNumberOfLines,
            this.toolStripSeparator4,
            this.showEntireFileButton,
            this.showNonPrintChars,
            this.ignoreWhiteSpaces});
            this.fileviewerToolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.fileviewerToolbar.Location = new System.Drawing.Point(243, 245);
            this.fileviewerToolbar.Name = "fileviewerToolbar";
            this.fileviewerToolbar.Size = new System.Drawing.Size(174, 23);
            this.fileviewerToolbar.TabIndex = 5;
            this.fileviewerToolbar.Visible = false;
            // 
            // nextChangeButton
            // 
            this.nextChangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextChangeButton.Image = global::GitUI.Properties.Resources._4;
            this.nextChangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextChangeButton.Name = "nextChangeButton";
            this.nextChangeButton.Size = new System.Drawing.Size(23, 20);
            this.nextChangeButton.ToolTipText = "Next change";
            // 
            // previousChangeButton
            // 
            this.previousChangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previousChangeButton.Image = global::GitUI.Properties.Resources._3;
            this.previousChangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previousChangeButton.Name = "previousChangeButton";
            this.previousChangeButton.Size = new System.Drawing.Size(23, 20);
            this.previousChangeButton.ToolTipText = "Previous change";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // increaseNumberOfLines
            // 
            this.increaseNumberOfLines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.increaseNumberOfLines.Image = ((System.Drawing.Image)(resources.GetObject("increaseNumberOfLines.Image")));
            this.increaseNumberOfLines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.increaseNumberOfLines.Name = "increaseNumberOfLines";
            this.increaseNumberOfLines.Size = new System.Drawing.Size(23, 20);
            this.increaseNumberOfLines.ToolTipText = "Increase number of visible lines";
            // 
            // DecreaseNumberOfLines
            // 
            this.DecreaseNumberOfLines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseNumberOfLines.Image = ((System.Drawing.Image)(resources.GetObject("DecreaseNumberOfLines.Image")));
            this.DecreaseNumberOfLines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseNumberOfLines.Name = "DecreaseNumberOfLines";
            this.DecreaseNumberOfLines.Size = new System.Drawing.Size(23, 20);
            this.DecreaseNumberOfLines.ToolTipText = "Decrease number of visible lines";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // showEntireFileButton
            // 
            this.showEntireFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showEntireFileButton.Image = ((System.Drawing.Image)(resources.GetObject("showEntireFileButton.Image")));
            this.showEntireFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showEntireFileButton.Name = "showEntireFileButton";
            this.showEntireFileButton.Size = new System.Drawing.Size(23, 20);
            this.showEntireFileButton.ToolTipText = "Show entire file";
            // 
            // showNonPrintChars
            // 
            this.showNonPrintChars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showNonPrintChars.Image = global::GitUI.Properties.Resources.nonprintchar;
            this.showNonPrintChars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showNonPrintChars.Name = "showNonPrintChars";
            this.showNonPrintChars.Size = new System.Drawing.Size(23, 20);
            this.showNonPrintChars.ToolTipText = "Show nonprinting characters";
            // 
            // ignoreWhiteSpaces
            // 
            this.ignoreWhiteSpaces.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ignoreWhiteSpaces.Image = ((System.Drawing.Image)(resources.GetObject("ignoreWhiteSpaces.Image")));
            this.ignoreWhiteSpaces.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ignoreWhiteSpaces.Name = "ignoreWhiteSpaces";
            this.ignoreWhiteSpaces.Size = new System.Drawing.Size(23, 20);
            this.ignoreWhiteSpaces.ToolTipText = "Ignore whitespaces";
            // 
            // PictureBox
            // 
            this.PictureBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox.Location = new System.Drawing.Point(0, 0);
            this.PictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(660, 512);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 6;
            this.PictureBox.TabStop = false;
            this.PictureBox.Visible = false;
            // 
            // FileViewerMono
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.fileviewerToolbar);
            this.Controls.Add(this.TextEditor);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FileViewerMono";
            this.Size = new System.Drawing.Size(660, 512);
            this.ContextMenu.ResumeLayout(false);
            this.fileviewerToolbar.ResumeLayout(false);
            this.fileviewerToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox TextEditor;
        private System.Windows.Forms.ContextMenuStrip ContextMenu;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ignoreWhitespaceChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseNumberOfLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem descreaseNumberOfLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showEntireFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem treatAllFilesAsTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNonprintableCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStrip fileviewerToolbar;
        private System.Windows.Forms.ToolStripButton nextChangeButton;
        private System.Windows.Forms.ToolStripButton previousChangeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton increaseNumberOfLines;
        private System.Windows.Forms.ToolStripButton DecreaseNumberOfLines;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton showEntireFileButton;
        private System.Windows.Forms.ToolStripButton showNonPrintChars;
        private System.Windows.Forms.ToolStripButton ignoreWhiteSpaces;
        private System.Windows.Forms.PictureBox PictureBox;
    }
}