using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public void ShowFileNames()
        {
            DirectoryInfo di = new DirectoryInfo(treeView.SelectedNode.FullPath);
            FileInfo[] fiarray = { };
            ListViewItem item;

            listView.Items.Clear();

            if (di.Exists)
            {
                fiarray = di.GetFiles();
            }

            listView.BeginUpdate();
            foreach (FileInfo fi in fiarray)
            {
                item = new ListViewItem(fi.Name);
                item.SubItems.Add(fi.Extension);
                item.SubItems.Add(fi.LastWriteTime.ToString());
                listView.Items.Add(item);
            }
            listView.EndUpdate();
        }

        public void ShowDrives()
        {
            treeView.BeginUpdate();
            string[] drives = Directory.GetLogicalDrives();
            foreach (string drive in drives)
            {
                TreeNode tn = new TreeNode(drive);
                treeView.Nodes.Add(tn);
                AddDirs(tn);
            }
            treeView.EndUpdate();
        }

        public void AddDirs(TreeNode tn)
        {
            string path = tn.FullPath;
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diarray = { };

            try
            {
                if (di.Exists)
                {
                    diarray = di.GetDirectories();
                }
            }
            catch
            {
                return;
            }

            foreach (DirectoryInfo d in diarray)
            {
                TreeNode tndir = new TreeNode(d.Name);
                tn.Nodes.Add(tndir);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ShowDrives();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowFileNames();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeView.BeginUpdate();
            foreach (TreeNode tn in e.Node.Nodes)
            {
                AddDirs(tn);
            }
            treeView.EndUpdate();
        }

        

        // Ham cac chuc nang
        private void CreateNewFolder()
        {
            if (treeView.SelectedNode != null)
            {
                string folderPath = treeView.SelectedNode.FullPath;
                string newFolderPath = Path.Combine(folderPath, "New Folder");

                try
                {
                    Directory.CreateDirectory(newFolderPath);
                    TreeNode newNode = new TreeNode("New Folder");
                    treeView.SelectedNode.Nodes.Add(newNode);
                    MessageBox.Show("New folder created!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                }
            }
        }

        private string fileToCopyOrCut;
        private bool isCutOperation;

        private void CopyFile()
        {
            if (listView.SelectedItems.Count > 0)
            {
                fileToCopyOrCut = Path.Combine(treeView.SelectedNode.FullPath, listView.SelectedItems[0].Text);
                isCutOperation = false;
                MessageBox.Show("File copied: " + fileToCopyOrCut);
            }
        }

        private void CutFile()
        {
            if (listView.SelectedItems.Count > 0)
            {
                fileToCopyOrCut = Path.Combine(treeView.SelectedNode.FullPath, listView.SelectedItems[0].Text);
                isCutOperation = true;
                MessageBox.Show("File cut: " + fileToCopyOrCut);
            }
        }

        private void PasteFile()
        {
            if (!string.IsNullOrEmpty(fileToCopyOrCut) && treeView.SelectedNode != null)
            {
                string destinationPath = Path.Combine(treeView.SelectedNode.FullPath, Path.GetFileName(fileToCopyOrCut));

                try
                {
                    if (isCutOperation)
                    {
                        File.Move(fileToCopyOrCut, destinationPath);
                        MessageBox.Show("File moved to: " + destinationPath);
                    }
                    else
                    {
                        File.Copy(fileToCopyOrCut, destinationPath);
                        MessageBox.Show("File copied to: " + destinationPath);
                    }

                    fileToCopyOrCut = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during paste operation: " + ex.Message);
                }
            }
        }

        private void DeleteFile()
        {
            if (listView.SelectedItems.Count > 0)
            {
                string filePath = Path.Combine(treeView.SelectedNode.FullPath, listView.SelectedItems[0].Text);

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        MessageBox.Show("File deleted: " + filePath);
                    }
                    else if (Directory.Exists(filePath))
                    {
                        Directory.Delete(filePath, true); 
                        MessageBox.Show("Folder deleted: " + filePath);
                    }
                    ShowFileNames(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting file: " + ex.Message);
                }
            }
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            CreateNewFolder();
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            CopyFile();
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            CutFile();
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            PasteFile();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            DeleteFile();  
        }
    }
}
