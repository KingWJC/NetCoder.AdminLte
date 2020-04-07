using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCode.Util
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        #region 操作文件目录
        /// <summary>
        /// 获取当前程序根目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 检验目录，若目录已存在则不变
        /// </summary>
        /// <param name="path">目录位置</param>
        public static void CheckDirectory(string path)
        {
            if (path.Contains("\\"))
                Directory.CreateDirectory(GetPathDirectory(path));
        }

        /// <summary>
        /// 获取文件位置中的目录位置（不包括文件名）
        /// </summary>
        /// <param name="path">文件位置</param>
        /// <returns></returns>
        public static string GetPathDirectory(string path)
        {
            if (!path.Contains("\\"))
                return GetCurrentDir();

            string pathDirectory = string.Empty;
            string pattern = @"^(.*\\).*?$";
            Match match = Regex.Match(path, pattern);

            return match.Groups[1].ToString();
        }

        /// <summary>
        /// 生成绝对路径
        /// </summary>
        /// <param name="uploadFolder"></param>
        /// <returns></returns>
        public static string GenerateFullPath(string uploadFolder)
        {
            return GetCurrentDir() + uploadFolder;
        }

        /// <summary>
        /// 删除指定路径下的文件夹
        /// </summary>
        /// <param name="relativePath"></param>
        public static void DeleteDir(string relativePath)
        {
            var path = GetPathDirectory(GenerateFullPath(relativePath));
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        #endregion

        #region 文件写入操作
        /// <summary>
        /// 输出字符串到文件
        /// 注：使用系统默认编码;若文件不存在则创建新的,若存在则覆盖
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        public static void WriteTxt(string content, string path)
        {
            WriteTxt(content, path, null, null);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码;若文件不存在则创建新的,若存在则覆盖
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码</param>
        public static void WriteTxt(string content, string path, Encoding encoding)
        {
            WriteTxt(content, path, encoding, null);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义模式,使用默认编码
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileModel">输出方法</param>
        public static void WriteTxt(string content, string path, FileMode fileModel)
        {
            WriteTxt(content, path, null, fileModel);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码以及写入模式
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="fileModel">写入模式</param>
        public static void WriteTxt(string content, string path, Encoding encoding, FileMode fileModel)
        {
            WriteTxt(content, path, encoding, (FileMode?)fileModel);
        }

        /// <summary>
        /// 输出字符串到文件
        /// 注：使用自定义编码以及写入模式
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="fileModel">写入模式</param>
        private static void WriteTxt(string content, string path, Encoding encoding, FileMode? fileModel)
        {
            CheckDirectory(path);

            if (encoding == null)
                encoding = Encoding.Default;
            if (fileModel == null)
                fileModel = FileMode.Create;

            using (FileStream fileStream = new FileStream(path, fileModel.Value))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
                {
                    streamWriter.Write(content);
                    streamWriter.Flush();
                }
            }
        }

        /// <summary>
        /// 输出日志到指定文件
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="path">日志文件位置（默认为D:\测试\a.log）</param>
        public static void WriteLog(string msg, string path = @"Log.txt")
        {
            string content = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{msg}";

            WriteTxt(content, $"{GetCurrentDir()}{content}");
        }
        #endregion

        #region 文件读取操作
        /// <summary>
        /// 获取当前目录下的分块文件ID的数组
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static int[] GetChunkArray(string folder)
        {
            string[] list = Directory.GetFiles(GenerateFullPath(folder));
            int[] result = new int[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                result[i] = list[i].Substring(list[i].LastIndexOf("-") + 1).ToInt();
            }
            return result;
        }

        /// <summary>
        /// 获取指定文件的二进制流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetFileData(string path)
        {
            byte[] data = null;
            try
            {
                path = GetCurrentDir() + path;
                if (File.Exists(path))
                {
                    using (FileStream fs = File.OpenRead(path))
                    {
                        data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return data;
        }
        #endregion

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        public static void MergeFiles(string targetFile, string folder, string filename)
        {
            try
            {
                CheckDirectory(targetFile);

                using (FileStream fs = File.Create(targetFile))
                {
                    var list = new DirectoryInfo(folder).GetFiles().CastToList<FileInfo>();
                    list.Sort((a, b) =>
                    {
                        string p1 = a.Name;
                        string p2 = b.Name;
                        int i1 = p1.LastIndexOf("-");
                        int i2 = p2.LastIndexOf("-");
                        return Int32.Parse(p2.Substring(i2)).CompareTo(Int32.Parse(p1.Substring(i1)));
                    });
                    foreach (var p in list)
                    {
                        var bytes = File.ReadAllBytes(p.FullName);
                        fs.Write(bytes, 0, bytes.Length);
                        p.Delete();
                        bytes = null;
                    };
                    fs.Flush();
                    fs.Close();
                    Directory.Delete(folder);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
