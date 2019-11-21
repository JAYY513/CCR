using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip; 

namespace Dos.Common
{ 
    //zip       zixing

    /// <summary>
    /// 解压类
    /// </summary>
    public static class GZipHelper
    {
        /// <summary>
        /// 解压文件到指定文件夹
        /// </summary>
        /// <param name="file">要解压的文件</param>
        /// <param name="path">解压到的路径</param>
        /// <returns></returns>
        public static bool unzip(string file,string path)
        {
            if(!File.Exists(file)) //要解压的文件不存在
            {
                return false;
            }
            if (!Directory.Exists(path))
            {   //解压到的路径不存在则创建
                Directory.CreateDirectory(path);
            }
            try
            {
                UnZip(file, path);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩指定文件或文件夹到文件
        /// </summary>
        /// <param name="path">要压缩的文件或文件夹路径</param>
        /// <param name="file">要压缩到的文件</param>
        /// <param name="compressionLevel">压缩等级，最高为9</param>
        /// <returns></returns>
        public static bool zip(string path, string file, int compressionLevel = 5)
        {

            try
            {
                if (File.Exists(file))
                {   //压缩到的文件路径存在则删除
                    File.Delete(file);
                }
                if (Directory.Exists(path))
                {   
                    ZipDir(path, file, compressionLevel);
                    return true;
                }
                else if (File.Exists(path))
                {
                    ZipFile(path, file, compressionLevel);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩指定文件或文件夹到文件
        /// </summary>
        /// <param name="path">要压缩的文件或文件夹路径</param>
        /// <param name="file">要压缩到的文件</param>
        /// <param name="compressionLevel">压缩等级，最高为9</param>
        /// <returns></returns>
        public static bool zip(List<string> paths, string file, int compressionLevel = 5)
        {
            if (paths == null || paths.Count == 0)
            {
                return false;
            }
            try
            {
                ZipManyFilesOrDictorys(paths, file);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region SharpZipLib调用
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dirToZip"></param>
        /// <param name="zipedFileName"></param>
        /// <param name="compressionLevel">压缩率0（无压缩）9（压缩率最高）</param>
        private static void ZipDir(string dirToZip, string zipedFileName, int compressionLevel = 9)
        {
            if (Path.GetExtension(zipedFileName) != ".zip")
            {
                zipedFileName = zipedFileName + ".zip";
            }
            using (var zipoutputstream = new ZipOutputStream(File.Create(zipedFileName)))
            {
                zipoutputstream.SetLevel(compressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = GetAllFies(dirToZip);
                foreach (DictionaryEntry item in fileList)
                {
                    FileStream fs = new FileStream(item.Key.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    // ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(dirToZip.Length + 1));
                    ZipEntry entry = new ZipEntry(Path.GetFileName(item.Key.ToString()))
                    {
                        DateTime = (DateTime)item.Value,
                        Size = fs.Length
                    };
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        private static Hashtable GetAllFies(string dir)
        {
            Hashtable filesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            } 
            GetAllDirFiles(fileDire, filesList);
            GetAllDirsFiles(fileDire.GetDirectories(), filesList);
            return filesList;
        }

        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        private static Hashtable GetAllFies(string dir, string fileType = "*.*")
        {
            Hashtable filesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }
            GetAllDirFiles(fileDire, filesList, fileType);
            GetAllDirsFiles(fileDire.GetDirectories(), filesList, fileType);
            return filesList;
        }

        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="dir">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>  
        private static void GetAllDirFiles(DirectoryInfo dir, Hashtable filesList, string fileType = "*.*")
        {
            foreach (FileInfo file in dir.GetFiles(fileType))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private static void GetAllDirsFiles(IEnumerable<DirectoryInfo> dirs, Hashtable filesList, string fileType = "*.*")
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles(fileType))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }


        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private static void GetAllDirsFiles(IEnumerable<DirectoryInfo> dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <returns>解压是否成功</returns>  
        private static void UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (directoryName != null && !directoryName.EndsWith("/"))
                    {
                    }
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {

                            int size;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="filePath">被压缩的文件名称(包含文件路径)，文件的全路径</param>
        /// <param name="zipedFileName">压缩后的文件名称(包含文件路径)，保存的文件名称</param>
        /// <param name="compressionLevel">压缩率0（无压缩）到 9（压缩率最高）</param>
        private static void ZipFile(string filePath, string zipedFileName, int compressionLevel = 9)
        {
            // 如果文件没有找到，则报错 
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件：" + filePath + "没有找到！");
            }
            // 如果压缩后名字为空就默认使用源文件名称作为压缩文件名称
            if (string.IsNullOrEmpty(zipedFileName))
            {
                string oldValue = Path.GetFileName(filePath);
                if (oldValue != null)
                {
                    zipedFileName = filePath.Replace(oldValue, "") + Path.GetFileNameWithoutExtension(filePath) + ".zip";
                }
            }
            // 如果压缩后的文件名称后缀名不是zip，就是加上zip，防止是一个乱码文件
            if (Path.GetExtension(zipedFileName) != ".zip")
            {
                zipedFileName = zipedFileName + ".zip";
            }
            // 如果指定位置目录不存在，创建该目录  C:\Users\yhl\Desktop\大汉三通
            string zipedDir = zipedFileName.Substring(0, zipedFileName.LastIndexOf("\\", StringComparison.Ordinal));
            if (!Directory.Exists(zipedDir))
            {
                Directory.CreateDirectory(zipedDir);
            }
            // 被压缩文件名称
            string filename = filePath.Substring(filePath.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            var streamToZip = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var zipFile = File.Create(zipedFileName);
            var zipStream = new ZipOutputStream(zipFile);
            var zipEntry = new ZipEntry(filename);
            zipStream.PutNextEntry(zipEntry);
            zipStream.SetLevel(compressionLevel);
            var buffer = new byte[2048];
            Int32 size = streamToZip.Read(buffer, 0, buffer.Length);
            zipStream.Write(buffer, 0, size);
            try
            {
                while (size < streamToZip.Length)
                {
                    int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            finally
            {
                zipStream.Finish();
                zipStream.Close();
                streamToZip.Close();
            }
        }

        /// <summary> 
        /// 压缩单个文件 
        /// </summary> 
        /// <param name="fileToZip">要进行压缩的文件名，全路径</param> 
        /// <param name="zipedFile">压缩后生成的压缩文件名,全路径</param> 
        private static void ZipFile(string fileToZip, string zipedFile)
        {
            // 如果文件没有找到，则报错 
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }
            using (FileStream fileStream = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();
                using (FileStream zipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream zipOutputStream = new ZipOutputStream(zipFile))
                    {
                        // string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        string fileName = Path.GetFileName(fileToZip);
                        var zipEntry = new ZipEntry(fileName)
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        zipOutputStream.PutNextEntry(zipEntry);
                        zipOutputStream.SetLevel(5);
                        zipOutputStream.Write(buffer, 0, buffer.Length);
                        zipOutputStream.Finish();
                        zipOutputStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多个目录或文件
        /// </summary>
        /// <param name="folderOrFileList">待压缩的文件夹或者文件，全路径格式,是一个集合</param>
        /// <param name="zipedFile">压缩后的文件名，全路径格式</param>
        /// <param name="password">压缩密码</param>
        /// <returns></returns>
        private static bool ZipManyFilesOrDictorys(List<string> folderOrFileList, string zipedFile, string password = null)
        {
            bool res = true;
            using (var s = new ZipOutputStream(File.Create(zipedFile)))
            {
                s.SetLevel(6);
                if (!string.IsNullOrEmpty(password))
                {
                    s.Password = password;
                }
                foreach (string fileOrDir in folderOrFileList)
                {
                    //是文件夹
                    if (Directory.Exists(fileOrDir))
                    {
                        res = ZipFileDictory(fileOrDir, s, "");
                    }
                    else if (File.Exists(fileOrDir))
                    {
                        //文件
                        res = ZipFileWithStream(fileOrDir, s);
                    }
                    else if (fileOrDir.IndexOf("*") > -1) 
                    {
                        res = ZipFileWithStream(fileOrDir, s);
                    }
                }
                s.Finish();
                s.Close();
                return res;
            }
        }
        /// <summary>
        /// 带压缩流压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        private static bool ZipFileWithStream(string fileToZip, ZipOutputStream zipStream)
        {
            //如果文件没有找到，则报错
            //if (!File.Exists(fileToZip))
            //{
            //    throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            //}
            //FileStream fs = null;
            FileStream zipFile = null;
            ZipEntry zipEntry = null;
            Crc32 crc = new Crc32();
            bool res = true;
            try
            {
                if (fileToZip.IndexOf("*") > -1)
                {
                    var files = _getFileByPath(fileToZip);
                    if (files != null && files.Count > 0)
                    {
                        foreach (string file in files)
                        {
                            //打开压缩文件
                            zipFile = File.OpenRead(file);
                            byte[] buffer = new byte[zipFile.Length];
                            zipFile.Read(buffer, 0, buffer.Length);
                            zipEntry = new ZipEntry(file);
                            zipEntry.DateTime = DateTime.Now;
                            zipEntry.Size = zipFile.Length;
                            zipFile.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            zipEntry.Crc = crc.Value;
                            zipStream.PutNextEntry(zipEntry);
                            zipStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                else
                {
                    zipFile = File.OpenRead(fileToZip);
                    byte[] buffer = new byte[zipFile.Length];
                    zipFile.Read(buffer, 0, buffer.Length);
                    zipFile.Close();
                    zipEntry = new ZipEntry(fileToZip);
                    zipStream.PutNextEntry(zipEntry);
                    zipStream.Write(buffer, 0, buffer.Length);
                } 
            }
            catch(Exception ex)
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                }

                if (zipFile != null)
                {
                    zipFile.Close();
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;

        }


        private static List<string> _getFileByPath(string path)
        {
            List<string> files = new List<string>();
            if (File.Exists(path))
            {
                files.Add(path);
                return files;
            }
            else if (Directory.Exists(path))
            {
                GetAllFies(path);
            }
            else if (path.IndexOf("*") > -1)
            {
                var t = path.Split('\\');
                if (t.Length == 1)
                {
                    return files;
                }
                string pathnew = "";
                for (int i = 0; i < t.Length - 1; i++)
                {
                    pathnew += t[i] + "\\";
                }
                var filetype = t[t.Length - 1];
                var t2 = GetAllFies(pathnew, filetype);
                foreach (var txt in t2.Keys)
                {
                    files.Add(pathnew + Path.GetFileName(txt.ToString()));
                }

            }

            return files;
        }

        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="s"></param>
        /// <param name="parentFolderName"></param>
        private static bool ZipFileDictory(string folderToZip, ZipOutputStream s, string parentFolderName)
        {
            bool res = true;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                //创建当前文件夹
                entry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/")); //加上 “/” 才会当成是文件夹创建
                s.PutNextEntry(entry);
                s.Flush();
                //先压缩文件，再递归压缩文件夹
                var filenames = Directory.GetFiles(folderToZip);
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (entry != null)
                {
                }
                GC.Collect();
                GC.Collect(1);
            }
            var folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, s, Path.Combine(parentFolderName, Path.GetFileName(folderToZip))))
                {
                    return false;
                }
            }
            return res;
        }


        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="dir">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>  
        public static void GetAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }
        #endregion

    }
}
