﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using GitCommands;
using ResourceManager.Translation;

namespace GitUI
{
    public partial class FormDeleteBranch : GitExtensionsForm
    {
        private readonly TranslationString _branchDeleted = new TranslationString("Command executed");

        private readonly string _defaultBranch;
        private readonly TranslationString _deleteBranchCaption = new TranslationString("Delete branch");

        private readonly TranslationString _deleteBranchQuestion =
            new TranslationString(
                "Are you sure you want to delete this branch?\nDeleting a branch can cause commits to be deleted too!");

        public FormDeleteBranch(string defaultBranch)
        {
            InitializeComponent();
            Translate();
            _defaultBranch = defaultBranch;
        }

        private void FormDeleteBranchLoad(object sender, EventArgs e)
        {
            Branches.DisplayMember = "Name";
            Branches.DataSource = GitCommandHelpers.GetHeads(false, true);

            if (_defaultBranch != null)
                Branches.Text = _defaultBranch;
        }

        private void OkClick(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(_deleteBranchQuestion.Text, _deleteBranchCaption.Text, MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    var deleteBranchResult = GitCommandHelpers.DeleteBranch(Branches.Text, ForceDelete.Checked);
                    MessageBox.Show(_branchDeleted.Text + Environment.NewLine + deleteBranchResult,
                                    _deleteBranchCaption.Text);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            Close();
        }
    }
}