using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MyImgaesViewer
{
    public partial class FrmMain : Form
    {
        private ImageList il;
        public FrmMain()
        {
            InitializeComponent();
            Preparing();
        }

        private bool Preparing()
        {
            CenterToScreen();
            UpdateDrives();
            return true;
        }
        private void UpdateDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo di in drives)
            {
                treeView1.Nodes.Add(new TreeNode(di.RootDirectory.ToString(), GetTree(di.RootDirectory.ToString())));
            }
        }

        private TreeNode[] GetTree(string path)
        {
            List<TreeNode> res = new List<TreeNode>();

            try
            {
                DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

                foreach (DirectoryInfo di in dirs)
                {
                    TreeNode[] subTree = GetSubTree(di.FullName);

                    if (subTree is null)
                    {
                        res.Add(new TreeNode(di.Name));
                    }
                    else
                    {
                        res.Add(new TreeNode(di.Name, subTree));
                    }
                }
            }
            catch { }

            return res.ToArray();
        }

        private TreeNode[] GetSubTree(string path)
        {
            List<TreeNode> res = new List<TreeNode>();

            try
            {
                DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

                foreach (DirectoryInfo di in dirs)
                {
                    res.Add(new TreeNode(di.Name));

                }
            }
            catch { }

            return res.ToArray();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            e.Node.Nodes.AddRange(GetTree(e.Node.FullPath));
            toolStripStatusLabel1.Text = e.Node.FullPath.ToString();
            DisplayImages(e.Node.FullPath.ToString());
        }

        private void DisplayImages(string path)
        {
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = null;

            if (!(this.il is null))
            {
                il.Images.Clear();
                il.Dispose();
            }

            this.il = new ImageList();
            this.il.ImageSize = new Size(150, 200);
            this.listView1.LargeImageList = this.il;

            FileInfo[] imgs = new DirectoryInfo(path).GetFiles("*.*",SearchOption.TopDirectoryOnly);
            var r = from ee in imgs where Constatns.ImagesExtenations.Contains(ee.Extension.ToLower()) select ee;


            int i = 0;
            foreach(FileInfo fi in r)
            //for (int i = 0; i < imgs.Length; i++)
            {
                this.il.Images.Add(new Bitmap(fi.FullName));
                this.listView1.Items.Add(new ListViewItem(fi.Name, i));
                i++;
            }
        }

        private void treeView1_DoubleClick(object sender, System.EventArgs e)
        {
            TreeNode tn = this.treeView1.SelectedNode;

            if(!(tn is null))
            {
                this.toolStripStatusLabel1.Text = tn.FullPath.ToString();
                DisplayImages(tn.FullPath);
            }
        }

        private void listView1_DoubleClick(object sender, System.EventArgs e)
        {
            if(this.listView1.SelectedItems.Count > 0)
            {
                string imgPath = toolStripStatusLabel1.Text + "\\" + this.listView1.Items[this.listView1.SelectedItems[0].Index].Text;
                FrmImageViewer fIV = new FrmImageViewer(imgPath);
                fIV.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FrmAboutBox fAB = new FrmAboutBox();
            fAB.ShowDialog();
        }
    }
}
