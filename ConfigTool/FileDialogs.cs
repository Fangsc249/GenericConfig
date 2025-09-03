using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigTool
{
    public class FileDialogs
    {
        public static string SelectFile(string folder, string filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = folder;
                openFileDialog.Filter = filter;
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                //openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            return filePath;
        }
        public static string[] SelectMultiFiles(string folder, string filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*")
        {/*2024-9-2*/
            var filePaths = new string[] { };

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = folder;
                openFileDialog.Filter = filter;
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePaths = openFileDialog.FileNames;
                }
            }
            return filePaths;
        }
        public static string SaveAsFile(string folder, string filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            var filePath = string.Empty;

            using (SaveFileDialog saveAsDialog = new SaveFileDialog())
            {
                saveAsDialog.InitialDirectory = folder;
                saveAsDialog.Filter = filter;
                saveAsDialog.FilterIndex = 2;
                saveAsDialog.RestoreDirectory = true;

                if (saveAsDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveAsDialog.FileName;
                }
            }
            return filePath;
        }

        public static string SelectFolder(string iniFolder = "c:\\")
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "请选择一个文件夹";
                folderDialog.ShowNewFolderButton = true;  // 允许用户创建新文件夹
                //folderDialog.InitialDirectory = iniFolder;
                // 显示对话框并检查用户是否点击了“确定”按钮
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    // 获取用户选择的文件夹路径
                    string selectedFolder = folderDialog.SelectedPath;
                    return selectedFolder;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

    }
}
