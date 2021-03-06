using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GitCommands;
using ResourceManager.Translation;
using PatchApply;
using GitUI.Hotkey;
using GitUI.Script;

namespace GitUI
{
    public partial class FormCommit : GitExtensionsForm //, IHotkeyable
    {
        #region Translation strings
        private readonly TranslationString _alsoDeleteUntrackedFiles =
            new TranslationString("Do you also want to delete the new files that are in the selection?" +
                                  Environment.NewLine + Environment.NewLine + "Choose 'No' to keep all new files.");

        private readonly TranslationString _alsoDeleteUntrackedFilesCaption = new TranslationString("Delete");

        private readonly TranslationString _amendCommit =
            new TranslationString("You are about to rewrite history." + Environment.NewLine +
                                  "Only use amend if the commit is not published yet!" + Environment.NewLine +
                                  Environment.NewLine + "Do you want to continue?");

        private readonly TranslationString _amendCommitCaption = new TranslationString("Amend commit");

        private readonly TranslationString _deleteFailed = new TranslationString("Delete file failed");

        private readonly TranslationString _deleteSelectedFiles =
            new TranslationString("Are you sure you want delete the selected file(s)?");

        private readonly TranslationString _deleteSelectedFilesCaption = new TranslationString("Delete");

        private readonly TranslationString _deleteUntrackedFiles =
            new TranslationString("Are you sure you want to delete all untracked files?");

        private readonly TranslationString _deleteUntrackedFilesCaption =
            new TranslationString("Delete untracked files.");

        private readonly TranslationString _enterCommitMessage = new TranslationString("Please enter commit message");
        private readonly TranslationString _enterCommitMessageCaption = new TranslationString("Commit message");

        private readonly TranslationString _enterCommitMessageHint = new TranslationString("Enter commit message");

        private readonly TranslationString _mergeConflicts =
            new TranslationString("There are unresolved mergeconflicts, solve mergeconflicts before committing.");

        private readonly TranslationString _mergeConflictsCaption = new TranslationString("Merge conflicts");

        private readonly TranslationString _noFilesStaged =
            new TranslationString("There are no files staged for this commit. Are you sure you want to commit?");

        private readonly TranslationString _noStagedChanges = new TranslationString("There are no staged changes");
        private readonly TranslationString _noUnstagedChanges = new TranslationString("There are no unstaged changes");

        private readonly TranslationString _notOnBranch =
            new TranslationString("You are not working on a branch." + Environment.NewLine +
                                  "This commit will be unreferenced when switching to another branch and can be lost." +
                                  Environment.NewLine + "" + Environment.NewLine + "Do you want to continue?");

        private readonly TranslationString _notOnBranchCaption = new TranslationString("Not on a branch.");

        private readonly TranslationString _onlyStageChunkOfSingleFileError =
            new TranslationString("You can only use this option when selecting a single file");

        private readonly TranslationString _resetChanges =
            new TranslationString("Are you sure you want to reset the changes to the selected files?");

        private readonly TranslationString _resetChangesCaption = new TranslationString("Reset changes");

        private readonly TranslationString _resetSelectedChanges =
            new TranslationString("Are you sure you want to reset all selected files?");

        private readonly TranslationString _resetStageChunkOfFileCaption = new TranslationString("Unstage chunk of file");
        private readonly TranslationString _stageDetails = new TranslationString("Stage Details");
        private readonly TranslationString _stageFiles = new TranslationString("Stage {0} files");
        private readonly TranslationString _selectOnlyOneFile = new TranslationString("You must have only one file selected.");

        private readonly TranslationString _stageSelectedLines = new TranslationString("Stage selected line(s)");
        private readonly TranslationString _unstageSelectedLines = new TranslationString("Unstage selected line(s)");
        private readonly TranslationString _resetSelectedLines = new TranslationString("Reset selected line(s)");
        private readonly TranslationString _resetSelectedLinesConfirmation = new TranslationString("Are you sure you want to reset the changes to the selected lines?");

        #endregion

        private GitCommandsInstance _gitGetUnstagedCommand;
        private readonly SynchronizationContext _syncContext;
        public bool NeedRefresh;
        private GitItemStatus _currentItem;
        private bool _currentItemStaged;
        private readonly ToolStripItem _StageSelectedLinesToolStripMenuItem;

        public FormCommit()
        {
            _syncContext = SynchronizationContext.Current;

            InitializeComponent();
            splitRight.Panel2MinSize = 130;
            Translate();

            SolveMergeconflicts.Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold);

            SelectedDiff.ExtraDiffArgumentsChanged += SelectedDiffExtraDiffArgumentsChanged;

            closeDialogAfterEachCommitToolStripMenuItem.Checked = Settings.CloseCommitDialogAfterCommit;
            closeDialogAfterAllFilesCommittedToolStripMenuItem.Checked = Settings.CloseCommitDialogAfterLastCommit;

            Unstaged.SetNoFilesText(_noUnstagedChanges.Text);
            Staged.SetNoFilesText(_noStagedChanges.Text);
            Message.SetEmptyMessage(_enterCommitMessageHint.Text);

            Unstaged.SelectedIndexChanged += UntrackedSelectionChanged;
            Staged.SelectedIndexChanged += TrackedSelectionChanged;

            Unstaged.DoubleClick += Unstaged_DoubleClick;
            Staged.DoubleClick += Staged_DoubleClick;

            Unstaged.Focus();

            SelectedDiff.AddContextMenuEntry(null, null);
            _StageSelectedLinesToolStripMenuItem = SelectedDiff.AddContextMenuEntry(_stageSelectedLines.Text, StageSelectedLinesToolStripMenuItemClick);
            SelectedDiff.AddContextMenuEntry(_resetSelectedLines.Text, ResetSelectedLinesToolStripMenuItemClick);

            splitMain.SplitterDistance = Settings.CommitDialogSplitter;

            this.HotkeysEnabled = true;
            this.Hotkeys = HotkeySettingsManager.LoadHotkeys(HotkeySettingsName);

            Commit.Focus();
        }

        #region Hotkey commands

        public const string HotkeySettingsName = "Commit";

        internal enum Commands : int
        {
            FocusUnstagedFiles,
            FocusSelectedDiff,
            FocusStagedFiles,
            FocusCommitMessage,
            StageSelectedFile,
            UnStageSelectedFile,
        }

        private bool FocusStagedFiles()
        {
            FocusFileList(this.Staged);
            return true;
        }

        private bool FocusUnstagedFiles()
        {
            FocusFileList(this.Unstaged);
            return true;
        }

        /// <summary>Helper method that moves the focus to the supplied FileStatusList</summary>
        private bool FocusFileList(FileStatusList fileStatusList)
        {
            fileStatusList.Focus();
            return true;
        }

        private bool FocusSelectedDiff()
        {
            this.SelectedDiff.Focus();
            return true;
        }

        private bool FocusCommitMessage()
        {
            this.Message.StartEditing();
            return true;
        }

        private bool StageSelectedFile()
        {
            if (Unstaged.Focused)
            {
                StageClick(this, null);
                return true;
            }
            return false;
        }

        private bool UnStageSelectedFile()
        {
            if (Staged.Focused)
            {
                UnstageFilesClick(this, null);
                return true;
            }
            return false;
        }

        protected override bool ExecuteCommand(int cmd)
        {
            Commands command = (Commands)cmd;

            switch (command)
            {
                case Commands.FocusStagedFiles: return FocusStagedFiles(); 
                case Commands.FocusUnstagedFiles: return FocusUnstagedFiles(); 
                case Commands.FocusSelectedDiff: return FocusSelectedDiff(); 
                case Commands.FocusCommitMessage: return FocusCommitMessage(); 
                case Commands.StageSelectedFile: return StageSelectedFile(); 
                case Commands.UnStageSelectedFile: return UnStageSelectedFile();
                default: return false;
            }
        }

        #endregion


        public void ShowDialogWhenChanges()
        {
            Initialize();
            while (_gitGetUnstagedCommand.IsRunning)
            {
                Thread.Sleep(200);
            }

            var allChangedFiles = GitCommandHelpers.GetAllChangedFilesFromString(_gitGetUnstagedCommand.Output.ToString());
            if (allChangedFiles.Count > 0)
            {
                ShowDialog();
            }
            else
            {
                DisposeGitGetUnstagedCommand();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            DisposeGitGetUnstagedCommand();

            base.OnClosing(e);
        }

        private void DisposeGitGetUnstagedCommand()
        {
            if (_gitGetUnstagedCommand != null)
            {
                _gitGetUnstagedCommand.Exited -= GitCommandsExited;
                _gitGetUnstagedCommand.Dispose();
            }
        }

        private void StageSelectedLinesToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Prepare git command
            string args = "apply --cached --whitespace=nowarn";

            if (_currentItemStaged) //staged
                args += " --reverse";

            string patch = PatchManager.GetSelectedLinesAsPatch(SelectedDiff.GetText(), SelectedDiff.GetSelectionPosition(), SelectedDiff.GetSelectionLength(), _currentItemStaged);

            if (!string.IsNullOrEmpty(patch))
            {
                GitCommandHelpers.RunCmd(Settings.GitCommand, args, patch);
                ScanClick(null, null);
            }
        }

        private void ResetSelectedLinesToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(_resetSelectedLinesConfirmation.Text, _resetChangesCaption.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // Prepare git command
            string args = "apply --whitespace=nowarn --reverse";

            if (_currentItemStaged) //staged
                args += " --index";

            string patch = PatchManager.GetSelectedLinesAsPatch(SelectedDiff.GetText(), SelectedDiff.GetSelectionPosition(), SelectedDiff.GetSelectionLength(), true);

            if (!string.IsNullOrEmpty(patch))
            {
                GitCommandHelpers.RunCmd(Settings.GitCommand, args, patch);
                ScanClick(null, null);
            }
        }

        private void EnableStageButtons(bool enable)
        {
            toolUnstageItem.Enabled = enable;
            toolUnstageAllItem.Enabled = enable;
            toolStageItem.Enabled = enable;
            toolStageAllItem.Enabled = enable;
            workingToolStripMenuItem.Enabled = enable;
        }

        private void Initialize()
        {
            EnableStageButtons(false);

            Cursor.Current = Cursors.WaitCursor;

            if (_gitGetUnstagedCommand == null)
            {
                _gitGetUnstagedCommand = new GitCommandsInstance();
                _gitGetUnstagedCommand.Exited += GitCommandsExited;
            }

            // Load unstaged files
            var allChangedFilesCmd =
                GitCommandHelpers.GetAllChangedFilesCmd(
                    !showIgnoredFilesToolStripMenuItem.Checked,
                    showUntrackedFilesToolStripMenuItem.Checked);
            _gitGetUnstagedCommand.CmdStartProcess(Settings.GitCommand, allChangedFilesCmd);
 
            Loading.Visible = true;
            LoadingStaged.Visible = true;

            Cursor.Current = Cursors.Default;
        }

        private void InitializedStaged()
        {
            Cursor.Current = Cursors.WaitCursor;
            Staged.GitItemStatuses = null;
            SolveMergeconflicts.Visible = GitCommandHelpers.InTheMiddleOfConflictedMerge();
            Staged.GitItemStatuses = GitCommandHelpers.GetStagedFiles();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        ///   Loads the unstaged output.
        ///   This method is passed in to the SetTextCallBack delegate
        ///   to set the Text property of textBox1.
        /// </summary>
        private void LoadUnstagedOutput()
        {
            var allChangedFiles = GitCommandHelpers.GetAllChangedFilesFromString(_gitGetUnstagedCommand.Output.ToString());

            var unStagedFiles = new List<GitItemStatus>();
            var stagedFiles = new List<GitItemStatus>();

            foreach (var fileStatus in allChangedFiles)
            {
                if (fileStatus.IsStaged)
                    stagedFiles.Add(fileStatus);
                else
                    unStagedFiles.Add(fileStatus);
            }

            Unstaged.GitItemStatuses = null;
            Unstaged.GitItemStatuses = stagedFiles;
            Staged.GitItemStatuses = null;
            Staged.GitItemStatuses = unStagedFiles;

            Loading.Visible = false;
            LoadingStaged.Visible = false;

            EnableStageButtons(true);
            workingToolStripMenuItem.Enabled = true;

            var inTheMiddleOfConflictedMerge = GitCommandHelpers.InTheMiddleOfConflictedMerge();
            SolveMergeconflicts.Visible = inTheMiddleOfConflictedMerge;
        }

        protected void ShowChanges(GitItemStatus item, bool staged)
        {
            _currentItem = item;
            _currentItemStaged = staged;

            if (item == null)
                return;

            if (item.Name.EndsWith(".png"))
            {
                SelectedDiff.ViewFile(item.Name);
            }
            else if (item.IsTracked)
            {
                SelectedDiff.ViewCurrentChanges(item.Name, item.OldName, staged);
            }
            else
            {
                SelectedDiff.ViewFile(item.Name);
            }

            _StageSelectedLinesToolStripMenuItem.Text = staged ? _unstageSelectedLines.Text : _stageSelectedLines.Text;
        }

        private void TrackedSelectionChanged(object sender, EventArgs e)
        {
            if (Staged.SelectedItems.Count == 0)
                return;

            Unstaged.SelectedItem = null;
            ShowChanges(Staged.SelectedItems[0], true);
        }

        private void UntrackedSelectionChanged(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            Staged.SelectedItem = null;
            ShowChanges(Unstaged.SelectedItems[0], false);
        }

        private void CommitClick(object sender, EventArgs e)
        {
            CheckForStagedAndCommit(false, false);
        }

        private void CheckForStagedAndCommit(bool amend, bool push)
        {
            if (Staged.GitItemStatuses.Count == 0)
            {
                if (MessageBox.Show(_noFilesStaged.Text, _noStagedChanges.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) ==
                    DialogResult.No)
                    return;
            }

            DoCommit(amend, push);
        }

        private void DoCommit(bool amend, bool push)
        {
            if (GitCommandHelpers.InTheMiddleOfConflictedMerge())
            {
                MessageBox.Show(_mergeConflicts.Text, _mergeConflictsCaption.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(Message.Text))
            {
                MessageBox.Show(_enterCommitMessage.Text, _enterCommitMessageCaption.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (GitCommandHelpers.GetSelectedBranch().Equals("(no branch)", StringComparison.OrdinalIgnoreCase) &&
                MessageBox.Show(_notOnBranch.Text, _notOnBranchCaption.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            try
            {
                SetCommitMessageFromTextBox(Message.Text);

                ScriptManager.RunEventScripts(ScriptEvent.BeforeCommit);

                var form = new FormProcess(GitCommandHelpers.CommitCmd(amend, toolAuthor.Text));
                form.ShowDialog();

                NeedRefresh = true;

                if (form.ErrorOccurred())
                    return;

                ScriptManager.RunEventScripts(ScriptEvent.AfterCommit);

                Message.Text = string.Empty;

                if (push)
                {
                    GitUICommands.Instance.StartPushDialog(true);
                }

                if (Settings.CloseCommitDialogAfterCommit)
                {
                    Close();
                    return;
                }

                if (Unstaged.GitItemStatuses.Any(gitItemStatus => gitItemStatus.IsTracked))
                {
                    InitializedStaged();
                    return;
                }

                if (Settings.CloseCommitDialogAfterLastCommit)
                    Close();
                else
                    InitializedStaged();
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Exception: {0}", e.Message));
            }
        }

        private void ScanClick(object sender, EventArgs e)
        {
            toolRefreshItem.Enabled = false;
            Initialize();
            toolRefreshItem.Enabled = true;
        }

        private void StageClick(object sender, EventArgs e)
        {
            Stage(Unstaged.SelectedItems);
        }

        private void Stage(ICollection<GitItemStatus> gitItemStatusses)
        {
            EnableStageButtons(false);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Maximum = gitItemStatusses.Count * 2;
                toolStripProgressBar1.Value = 0;

                var files = new List<GitItemStatus>();

                foreach (var gitItemStatus in gitItemStatusses)
                {
                    toolStripProgressBar1.Value = Math.Min(toolStripProgressBar1.Maximum - 1, toolStripProgressBar1.Value + 1);
                    files.Add(gitItemStatus);
                }

                if (Settings.ShowErrorsWhenStagingFiles)
                {
                    FormStatus.ProcessStart processStart =
                        form =>
                        {
                            form.AddOutput(string.Format(_stageFiles.Text,
                                                         files.Count));
                            var output = GitCommandHelpers.StageFiles(files);
                            form.AddOutput(output);
                            form.Done(string.IsNullOrEmpty(output));
                        };
                    var process = new FormStatus(processStart, null) { Text = _stageDetails.Text };
                    process.ShowDialogOnError();
                }
                else
                {
                    GitCommandHelpers.StageFiles(files);
                }

                InitializedStaged();
                var stagedFiles = (List<GitItemStatus>)Staged.GitItemStatuses;
                var unStagedFiles = (List<GitItemStatus>)Unstaged.GitItemStatuses;
                Unstaged.GitItemStatuses = null;

                unStagedFiles.RemoveAll(item => stagedFiles.Exists(i => i.Name == item.Name || i.OldName == item.Name) && files.Exists(i => i.Name == item.Name));

                Unstaged.GitItemStatuses = unStagedFiles;

                toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;

                toolStripProgressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            EnableStageButtons(true);

            Commit.Enabled = true;
            Amend.Enabled = true;
            AcceptButton = Commit;
            Cursor.Current = Cursors.Default;

            if (Settings.RevisionGraphShowWorkingDirChanges)
                NeedRefresh = true;
        }

        private void UnstageFilesClick(object sender, EventArgs e)
        {
            EnableStageButtons(false);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (Staged.GitItemStatuses.Count > 10 && Staged.SelectedItems.Count == Staged.GitItemStatuses.Count)
                {
                    Loading.Visible = true;
                    LoadingStaged.Visible = true;
                    GitCommandHelpers.ResetMixed("HEAD");
                    Initialize();
                }
                else
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Maximum = Staged.SelectedItems.Count * 2;
                    toolStripProgressBar1.Value = 0;

                    var files = new List<GitItemStatus>();
                    var allFiles = new List<GitItemStatus>();

                    foreach (var item in Staged.SelectedItems)
                    {
                        toolStripProgressBar1.Value = Math.Min(toolStripProgressBar1.Maximum - 1, toolStripProgressBar1.Value + 1);
                        if (!item.IsNew)
                        {
                            toolStripProgressBar1.Value = Math.Min(toolStripProgressBar1.Maximum - 1, toolStripProgressBar1.Value + 1);
                            GitCommandHelpers.UnstageFileToRemove(item.Name);

                            if (item.IsRenamed)
                                GitCommandHelpers.UnstageFileToRemove(item.OldName);
                        }
                        else
                        {
                            files.Add(item);
                        }
                        allFiles.Add(item);
                    }

                    GitCommandHelpers.UnstageFiles(files);

                    InitializedStaged();
                    var stagedFiles = (List<GitItemStatus>)Staged.GitItemStatuses;
                    var unStagedFiles = (List<GitItemStatus>)Unstaged.GitItemStatuses;
                    Unstaged.GitItemStatuses = null;
                    foreach (var item in allFiles)
                    {
                        var item1 = item;
                        if (stagedFiles.Exists(i => i.Name == item1.Name))
                            continue;

                        var item2 = item;
                        if (unStagedFiles.Exists(i => i.Name == item2.Name))
                            continue;

                        if (item.IsNew && !item.IsChanged && !item.IsDeleted)
                            item.IsTracked = false;
                        else
                            item.IsTracked = true;

                        if (item.IsRenamed)
                        {
                            GitItemStatus clone = new GitItemStatus();
                            clone.Name = item.OldName;
                            clone.IsDeleted = true;
                            clone.IsTracked = true;
                            clone.IsStaged = false;
                            unStagedFiles.Add(clone);

                            item.IsRenamed = false;
                            item.IsNew = true;
                            item.IsTracked = false;
                            item.OldName = string.Empty;
                        }

                        item.IsStaged = false;
                        unStagedFiles.Add(item);
                    }
                    Staged.GitItemStatuses = stagedFiles;
                    Unstaged.GitItemStatuses = unStagedFiles;

                    toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
                }
                toolStripProgressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            EnableStageButtons(true);
            Cursor.Current = Cursors.Default;

            if (Settings.RevisionGraphShowWorkingDirChanges)
                NeedRefresh = true;
        }


        private void ResetSoftClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItem == null ||
                MessageBox.Show(_resetChanges.Text, _resetChangesCaption.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) !=
                DialogResult.Yes)
                return;

            var deleteNewFiles = false;
            var askToDeleteNewFiles = true;
            var output = new StringBuilder();
            foreach (var item in Unstaged.SelectedItems)
            {
                if (item.IsNew)
                {
                    if (!deleteNewFiles && askToDeleteNewFiles)
                    {
                        var result = MessageBox.Show(_alsoDeleteUntrackedFiles.Text,
                                                     _alsoDeleteUntrackedFilesCaption.Text, MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            deleteNewFiles = true;

                        askToDeleteNewFiles = false;
                    }

                    if (deleteNewFiles)
                        File.Delete(Settings.WorkingDir + item.Name);
                }
                else
                {
                    output.Append(GitCommandHelpers.ResetFile(item.Name));
                }
            }

            if (!string.IsNullOrEmpty(output.ToString()))
                MessageBox.Show(output.ToString(), _resetChangesCaption.Text);

            Initialize();
        }

        private void DeleteFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                SelectedDiff.ViewText("", "");
                if (Unstaged.SelectedItem == null ||
                    MessageBox.Show(_deleteSelectedFiles.Text, _deleteSelectedFilesCaption.Text, MessageBoxButtons.YesNo) !=
                    DialogResult.Yes)
                    return;

                foreach (var item in Unstaged.SelectedItems)
                    File.Delete(Settings.WorkingDir + item.Name);

                Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_deleteFailed.Text + Environment.NewLine + ex.Message);
            }
        }

        private void SolveMergeConflictsClick(object sender, EventArgs e)
        {
            if (GitUICommands.Instance.StartResolveConflictsDialog())
                Initialize();
        }

        private void DeleteSelectedFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(_deleteSelectedFiles.Text, _deleteSelectedFilesCaption.Text, MessageBoxButtons.YesNo) !=
                DialogResult.Yes)
                return;

            try
            {
                foreach (var gitItemStatus in Unstaged.SelectedItems)
                    File.Delete(Settings.WorkingDir + gitItemStatus.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_deleteFailed.Text + Environment.NewLine + ex);
            }
            Initialize();
        }

        private void ResetSelectedFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(_resetSelectedChanges.Text, _resetChangesCaption.Text, MessageBoxButtons.YesNo) !=
                DialogResult.Yes)
                return;

            foreach (var gitItemStatus in Unstaged.SelectedItems)
            {
                GitCommandHelpers.ResetFile(gitItemStatus.Name);
            }
            Initialize();
        }

        private void ResetAlltrackedChangesToolStripMenuItemClick(object sender, EventArgs e)
        {
            ResetClick(null, null);
        }

        private void EditGitIgnoreToolStripMenuItemClick(object sender, EventArgs e)
        {
            GitUICommands.Instance.StartEditGitIgnoreDialog();
            Initialize();
        }

        private void StageAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            Stage(Unstaged.GitItemStatuses);
        }

        private void UnstageAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            GitCommandHelpers.ResetMixed("HEAD");
            Initialize();
        }

        private void FormCommitShown(object sender, EventArgs e)
        {
            if (_gitGetUnstagedCommand == null)
                Initialize();

            AcceptButton = Commit;

            var message = GitCommandHelpers.GetMergeMessage();

            if (string.IsNullOrEmpty(message) && File.Exists(GitCommands.Commit.GetCommitMessagePath()))
                message = File.ReadAllText(GitCommands.Commit.GetCommitMessagePath(), Settings.Encoding);
            if (!string.IsNullOrEmpty(message))
                Message.Text = message;

            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    var text =
                        string.Format("Commit to {0} ({1})", GitCommandHelpers.GetSelectedBranch(),
                                      Settings.WorkingDir);

                    _syncContext.Post(state1 => Text = text, null);
                });
        }

        public static void SetCommitMessageFromTextBox(string commitMessageText)
        {
            //Save last commit message in settings. This way it can be used in multiple repositories.
            Settings.LastCommitMessage = commitMessageText;

            var path = Settings.WorkingDirGitDir() + Settings.PathSeparator + "COMMITMESSAGE";

            //Commit messages are UTF-8 by default unless otherwise in the config file.
            //The git manual states:
            //  git commit and git commit-tree issues a warning if the commit log message 
            //  given to it does not look like a valid UTF-8 string, unless you 
            //  explicitly say your project uses a legacy encoding. The way to say 
            //  this is to have i18n.commitencoding in .git/config file, like this:...
            Encoding encoding;
            string encodingString = GitCommandHelpers.GetLocalConfig().GetValue("i18n.commitencoding");
            if (string.IsNullOrEmpty(encodingString))
                encodingString = GitCommandHelpers.GetGlobalConfig().GetValue("i18n.commitencoding");

            if (!string.IsNullOrEmpty(encodingString))
            {
                try
                {
                    encoding = Encoding.GetEncoding(encodingString);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "Unsupported encoding set in git config file: " + encodingString + Environment.NewLine + "Please check the setting i18n.commitencoding in your local and/or global config files. Commit aborted.");
                    return;
                }
            }
            else
            {
                encoding = new UTF8Encoding(false);
            }

            using (var textWriter = new StreamWriter(path, false, encoding))
            {
                var lineNumber = 0;
                foreach (var line in commitMessageText.Split('\n'))
                {
                    if (lineNumber == 1 && !String.IsNullOrEmpty(line))
                        textWriter.WriteLine();

                    textWriter.WriteLine(line);
                    lineNumber++;
                }
            }
        }

        private void FormCommitFormClosing(object sender, FormClosingEventArgs e)
        {
            GitCommands.Commit.SetCommitMessage(Message.Text);
            SavePosition("commit");

            Settings.CommitDialogSplitter = splitMain.SplitterDistance;
        }

        private void DeleteAllUntrackedFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                _deleteUntrackedFiles.Text,
                _deleteUntrackedFilesCaption.Text,
                MessageBoxButtons.YesNo) !=
                DialogResult.Yes)
                return;
            new FormProcess("clean -f").ShowDialog();
            Initialize();
        }

        private void ShowIgnoredFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            showIgnoredFilesToolStripMenuItem.Checked = !showIgnoredFilesToolStripMenuItem.Checked;
            ScanClick(null, null);
        }

        private void CommitMessageToolStripMenuItemDropDownOpening(object sender, EventArgs e)
        {
            commitMessageToolStripMenuItem.DropDownItems.Clear();
            AddCommitMessageToMenu(Settings.LastCommitMessage);

            string localLastCommitMessage = GitCommandHelpers.GetPreviousCommitMessage(0);
            if (!localLastCommitMessage.Trim().Equals(Settings.LastCommitMessage.Trim()))
                AddCommitMessageToMenu(localLastCommitMessage);
            AddCommitMessageToMenu(GitCommandHelpers.GetPreviousCommitMessage(1));
            AddCommitMessageToMenu(GitCommandHelpers.GetPreviousCommitMessage(2));
            AddCommitMessageToMenu(GitCommandHelpers.GetPreviousCommitMessage(3));
        }

        private void AddCommitMessageToMenu(string commitMessage)
        {
            if (string.IsNullOrEmpty(commitMessage))
                return;

            var toolStripItem =
                new ToolStripMenuItem
                {
                    Tag = commitMessage,
                    Text =
                        commitMessage.Substring(0,
                                                Math.Min(Math.Min(50, commitMessage.Length),
                                                         commitMessage.Contains("\n") ? commitMessage.IndexOf('\n') : 99)) +
                        "..."
                };

            commitMessageToolStripMenuItem.DropDownItems.Add(toolStripItem);
        }

        private void CommitMessageToolStripMenuItemDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Message.Text = ((string)e.ClickedItem.Tag).Trim();
        }

        private void AddFileTogitignoreToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            SelectedDiff.ViewText("", "");
            var item = Unstaged.SelectedItem;
            new FormAddToGitIgnore(item.Name).ShowDialog();
            Initialize();
        }

        private void SelectedDiffExtraDiffArgumentsChanged(object sender, EventArgs e)
        {
            ShowChanges(_currentItem, _currentItemStaged);
        }

        private void RescanChangesToolStripMenuItemClick(object sender, EventArgs e)
        {
            ScanClick(null, null);
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            var item = Unstaged.SelectedItem;
            var fileName = item.Name;

            Process.Start((Settings.WorkingDir + fileName).Replace(Settings.PathSeparatorWrong, Settings.PathSeparator));
        }

        private void OpenWithToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            var item = Unstaged.SelectedItem;
            var fileName = item.Name;

            OpenWith.OpenAs(Settings.WorkingDir + fileName.Replace(Settings.PathSeparatorWrong, Settings.PathSeparator));
        }

        private void FilenameToClipboardToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            var fileNames = new StringBuilder();
            foreach (var item in Unstaged.SelectedItems)
            {
                //Only use appendline when multiple items are selected.
                //This to make it easier to use the text from clipboard when 1 file is selected.
                if (fileNames.Length > 0)
                    fileNames.AppendLine();

                fileNames.Append((Settings.WorkingDir + item.Name).Replace(Settings.PathSeparatorWrong, Settings.PathSeparator));
            }
            Clipboard.SetText(fileNames.ToString());
        }

        private void OpenWithDifftoolToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 0)
                return;

            var item = Unstaged.SelectedItem;
            var fileName = item.Name;

            var cmdOutput = GitCommandHelpers.OpenWithDifftool(fileName);

            if (!string.IsNullOrEmpty(cmdOutput))
                MessageBox.Show(cmdOutput);
        }


        private void ResetPartOfFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count != 1)
            {
                MessageBox.Show(_onlyStageChunkOfSingleFileError.Text, _resetStageChunkOfFileCaption.Text);
                return;
            }

            foreach (var gitItemStatus in Unstaged.SelectedItems)
            {
                GitCommandHelpers.RunRealCmd
                    (Settings.GitCommand,
                     string.Format("checkout -p \"{0}\"", gitItemStatus.Name));
                Initialize();
            }
        }

        private void FormCommitLoad(object sender, EventArgs e)
        {
            RestorePosition("commit");
        }

        private void GitCommandsExited(object sender, EventArgs e)
        {
            _syncContext.Post(_ => LoadUnstagedOutput(), null);
        }

        private void ResetClick(object sender, EventArgs e)
        {
            if (!Abort.AbortCurrentAction())
                return;

            Initialize();
            NeedRefresh = true;
        }

        private void AmendClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Message.Text))
            {
                Message.Text = GitCommandHelpers.GetPreviousCommitMessage(0).Trim();
                return;
            }

            if (MessageBox.Show(_amendCommit.Text, _amendCommitCaption.Text, MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
                DoCommit(true, false);
        }

        private void ShowUntrackedFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            showUntrackedFilesToolStripMenuItem.Checked = !showUntrackedFilesToolStripMenuItem.Checked;
            ScanClick(null, null);
        }

        private void editFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = Unstaged.SelectedItem;
            var fileName = Settings.WorkingDir + item.Name;

            new FormEditor(fileName).ShowDialog();

            UntrackedSelectionChanged(null, null);
        }

        private void CommitAndPush_Click(object sender, EventArgs e)
        {
            CheckForStagedAndCommit(false, true);
        }

        private void FormCommitActivated(object sender, EventArgs e)
        {
            if (Settings.RefreshCommitDialogOnFormFocus)
                ScanClick(null, null);
        }

        private void ViewFileHistoryMenuItem_Click(object sender, EventArgs e)
        {
            if (Unstaged.SelectedItems.Count == 1)
            {
                GitUICommands.Instance.StartFileHistoryDialog(Unstaged.SelectedItem.Name, null);
            }
            else
                MessageBox.Show(this, _selectOnlyOneFile.Text, "Error");
        }

        void Unstaged_DoubleClick(object sender, EventArgs e)
        {
            StageClick(sender, e);
        }

        void Staged_DoubleClick(object sender, EventArgs e)
        {
            UnstageFilesClick(sender, e);
        }

        private void Message_KeyUp(object sender, KeyEventArgs e)
        {
            // Ctrl + Enter = Commit
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                CheckForStagedAndCommit(false, false);
                e.Handled = true;
            }
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent adding a line break when all we want is to commit
            if (e.Control && e.KeyCode == Keys.Enter)
                e.Handled = true;
        }


        private void closeDialogAfterEachCommitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeDialogAfterEachCommitToolStripMenuItem.Checked = !closeDialogAfterEachCommitToolStripMenuItem.Checked;
            Settings.CloseCommitDialogAfterCommit = closeDialogAfterEachCommitToolStripMenuItem.Checked;
        }

        private void closeDialogAfterAllFilesCommittedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeDialogAfterAllFilesCommittedToolStripMenuItem.Checked = !closeDialogAfterAllFilesCommittedToolStripMenuItem.Checked;
            Settings.CloseCommitDialogAfterLastCommit = closeDialogAfterAllFilesCommittedToolStripMenuItem.Checked;

        }

        private void refreshDialogOnFormFocusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshDialogOnFormFocusToolStripMenuItem.Checked = !refreshDialogOnFormFocusToolStripMenuItem.Checked;
            Settings.RefreshCommitDialogOnFormFocus = refreshDialogOnFormFocusToolStripMenuItem.Checked;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
                ScanClick(null, null);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolAuthor_TextChanged(object sender, EventArgs e)
        {
            toolAuthorLabelItem.Enabled = toolAuthorLabelItem.Checked = !string.IsNullOrEmpty(toolAuthor.Text);
        }

        private void toolAuthorLabelItem_Click(object sender, EventArgs e)
        {
            toolAuthor.Text = "";
            toolAuthorLabelItem.Enabled = toolAuthorLabelItem.Checked = false;
        }

        private void toolbarUnstaged_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
