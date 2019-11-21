using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Dos.Common.Helper
{
    public class FtpHelper
    {

        private static ManualResetEvent timeoutObject;
        private static Socket socket = null;
        private static bool isConn = false;
        /// <summary>
        /// 通过socket判断ftp是否通畅(异步socket连接,同步发送接收数据)
        /// </summary> 
        /// <returns></returns>
        public static bool CheckFtp(string ip, string ftpuser, string ftppas, out string errmsg, int port = 21, int timeout = 2000)
        {
            #region 输入数据检查
            if (ftpuser.Trim().Length == 0)
            {
                errmsg = "FTP用户名不能为空,请检查设置!";
                return false;
            }
            if (ftppas.Trim().Length == 0)
            {
                errmsg = "FTP密码不能为空,请检查设置!";
                return false;
            }
            IPAddress address;
            try
            {
                address = IPAddress.Parse(ip);
            }
            catch
            {
                errmsg = string.Format("FTP服务器IP:{0}解析失败,请检查是否设置正确!", ip);
                return false;
            }
            #endregion
            isConn = false;

            bool ret = false;
            byte[] result = new byte[1024];
            int pingStatus = 0, userStatus = 0, pasStatus = 0, exitStatus = 0; //连接返回,用户名返回,密码返回,退出返回
            timeoutObject = new ManualResetEvent(false);
            try
            {
                int receiveLength;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = timeout;
                socket.ReceiveTimeout = timeout;//超时设置成2000毫秒

                try
                {
                    socket.BeginConnect(new IPEndPoint(address, port), new AsyncCallback(callBackMethod), socket); //开始异步连接请求
                    if (!timeoutObject.WaitOne(timeout, false))
                    {
                        socket.Close();
                        socket = null;
                        pingStatus = -1;
                    }
                    if (isConn)
                    {
                        pingStatus = 200;
                    }
                    else
                    {
                        pingStatus = -1;
                    }
                }
                catch (Exception ex)
                {
                    pingStatus = -1;
                }

                if (pingStatus == 200) //状态码200 - TCP连接成功
                {
                    receiveLength = socket.Receive(result);
                    pingStatus = getFtpReturnCode(result, receiveLength); //连接状态
                    if (pingStatus == 220)//状态码220 - FTP返回欢迎语
                    {
                        socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "USER " + ftpuser, Environment.NewLine)));
                        receiveLength = socket.Receive(result);
                        userStatus = getFtpReturnCode(result, receiveLength);
                        if (userStatus == 331)//状态码331 - 要求输入密码
                        {
                            socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "PASS " + ftppas, Environment.NewLine)));
                            receiveLength = socket.Receive(result);
                            pasStatus = getFtpReturnCode(result, receiveLength);
                            if (pasStatus == 230)//状态码230 - 登入因特网
                            {
                                errmsg = string.Format("FTP:{0}@{1}登陆成功", ip, port);
                                ret = true;
                                socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "QUIT", Environment.NewLine))); //登出FTP
                                receiveLength = socket.Receive(result);
                                exitStatus = getFtpReturnCode(result, receiveLength);
                            }
                            else
                            { // 状态码230的错误
                                errmsg = string.Format("FTP:{0}@{1}登陆失败,用户名或密码错误({2})", ip, port, pasStatus);
                            }
                        }
                        else
                        {// 状态码331的错误 
                            errmsg = string.Format("使用用户名:'{0}'登陆FTP:{1}@{2}时发生错误({3}),请检查FTP是否正常配置!", ftpuser, ip, port, userStatus);
                        }
                    }
                    else
                    {// 状态码220的错误 
                        errmsg = string.Format("FTP:{0}@{1}返回状态错误({2}),请检查FTP服务是否正常运行!", ip, port, pingStatus);
                    }
                }
                else
                {// 状态码200的错误
                    errmsg = string.Format("无法连接FTP服务器:{0}@{1},请检查FTP服务是否启动!", ip, port);
                }
            }
            catch (Exception ex)
            { //连接出错 
                errmsg = string.Format("FTP:{0}@{1}连接出错:", ip, port) + ex.Message;
                //Common.Logger(errmsg);
                ret = false;
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close(); //关闭socket
                    socket = null;
                }
            }
            return ret;
        }
        private static void callBackMethod(IAsyncResult asyncResult)
        {
            try
            {
                socket = asyncResult.AsyncState as Socket;
                if (socket != null)
                {
                    socket.EndConnect(asyncResult);
                    isConn = true;
                }
            }
            catch (Exception ex)
            {
                isConn = false;
            }
            finally
            {
                timeoutObject.Set();
            }
        }
        /// <summary>
        /// 传递FTP返回的byte数组和长度,返回状态码(int)
        /// </summary>
        /// <param name="retByte"></param>
        /// <param name="retLen"></param>
        /// <returns></returns>
        private static int getFtpReturnCode(byte[] retByte, int retLen)
        {
            try
            {
                string str = Encoding.ASCII.GetString(retByte, 0, retLen).Trim();
                return int.Parse(str.Substring(0, 3));
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void UploadFile(System.Windows.Controls.ProgressBar pgb_progress, FileInfo fileinfo, string targetDir, string hostname, string username, string password,int port=21)
        {
            pgb_progress.Dispatcher.Invoke(() => { pgb_progress.Value = 3; });

            //1. check target
            string target;
            if (targetDir.Trim() == "")
            {
                return;
            }
            string filename = fileinfo.Name;
            if (!string.IsNullOrEmpty(filename))
                target = filename;
            else
                target = Guid.NewGuid().ToString();  //使用临时文件名

            string URI = "FTP://" + hostname + ":"+ port + "/" + targetDir + "/" + target;

            //没有目录则新建一个
            if(ftpIsExistsFile(targetDir, hostname,username,password,port) == false)
            {
                MakeDir(targetDir, hostname, username, password, port);
            }

            ///WebClient webcl = new WebClient();
            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);

            //设置FTP命令 设置所要执行的FTP命令，
            //ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;//假设此处为显示指定路径下的文件列表
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            //告诉ftp文件大小
            ftp.ContentLength = fileinfo.Length;
            //缓冲大小设置为2KB
            double persent = 0;

            int sendedCount = 0;

            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1 + 1];
            int dataRead;

            //打开一个文件流 (System.IO.FileStream) 去读上传的文件
            using (FileStream fs = fileinfo.OpenRead())
            {
                try
                {
                    //把上传的文件写入流
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            //每次读文件流的2KB
                            dataRead = fs.Read(content, 0, BufferSize);
                            rs.Write(content, 0, dataRead); 
                            sendedCount += dataRead;
                            persent = ((double)sendedCount) / ((double)fileinfo.Length) *100;

                            persent = 3d+persent*((99d-3d)/100d);

                            pgb_progress.Dispatcher.Invoke(() => { pgb_progress.Value = persent; });

                        } while (!(dataRead < BufferSize));
                        rs.Close();
                    }

                }
                catch (Exception ex) { }
                finally
                {
                    fs.Close();
                }

            }
            pgb_progress.Dispatcher.Invoke(() => { pgb_progress.Value = 99; });
            ftp = null;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">下载至本地路径</param>
        /// <param name="FtpDir">ftp目标文件路径</param>
        /// <param name="FtpFile">从ftp要下载的文件名</param>
        /// <param name="hostname">ftp地址即IP</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void DownloadFile(string localDir, string FtpDir, string FtpFile, string hostname, string username, string password)
        {
            string URI = "FTP://" + hostname + "/" + FtpDir + "/" + FtpFile;
            string tmpname = Guid.NewGuid().ToString();
            string localfile = localDir + @"\" + tmpname;

            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = false;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (FileStream fs = new FileStream(localfile, FileMode.CreateNew))
                    {
                        try
                        {
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            File.Delete(localfile);
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }



            try
            {
                File.Delete(localDir + @"\" + FtpFile);
                File.Move(localfile, localDir + @"\" + FtpFile);


                ftp = null;
                ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
                ftp.GetResponse();

            }
            catch (Exception ex)
            {
                File.Delete(localfile);
                throw ex;
            }

            // 记录日志 "从" + URI.ToString() + "下载到" + localDir + @"\" + FtpFile + "成功." );
            ftp = null;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">下载至本地路径</param>
        /// <param name="FtpDir">ftp目标文件路径</param>
        /// <param name="FtpFile">从ftp要下载的文件名</param>
        /// <param name="hostname">ftp地址即IP</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static byte[] DownloadFileBytes(string localDir, string FtpDir, string FtpFile, string hostname, string username, string password)
        {
            byte[] bts;
            string URI = "FTP://" + hostname + "/" + FtpDir + "/" + FtpFile;
            string tmpname = Guid.NewGuid().ToString();
            string localfile = localDir + @"\" + tmpname;

            System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (MemoryStream fs = new MemoryStream())
                    {
                        try
                        {
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();

                            //---
                            byte[] mbt = new byte[fs.Length];
                            fs.Read(mbt, 0, mbt.Length);

                            bts = mbt;
                            //---
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            File.Delete(localfile);
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }

            ftp = null;
            return bts;
        }

        /// <summary>
        /// 搜索远程文件
        /// </summary>
        /// <param name="targetDir"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public static List<string> ListDirectory(string targetDir, string hostname, string username, string password, string SearchPattern)
        {
            List<string> result = new List<string>();
            try
            {
                string URI = "FTP://" + hostname + "/" + targetDir + "/" + SearchPattern;

                System.Net.FtpWebRequest ftp = GetRequest(URI, username, password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                ftp.UsePassive = true;
                ftp.UseBinary = true;


                string str = GetStringResponse(ftp);
                str = str.Replace("\r\n", "\r").TrimEnd('\r');
                str = str.Replace("\n", "\r");
                if (str != string.Empty)
                    result.AddRange(str.Split('\r'));

                return result;
            }
            catch { }
            return null;
        }

        private static string GetStringResponse(FtpWebRequest ftp)
        {
            //Get the result, streaming to a string
            string result = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream, System.Text.Encoding.Default))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return result;
        }

        /// 在ftp服务器上创建目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public static void MakeDir(string dirName, string ftpHostIP, string username, string password,int port)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + ":" + port + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public static void delFile(string dirName, string filename, string ftpHostIP, string username, string password)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + "/";
                if (!string.IsNullOrEmpty(dirName))
                {
                    uri += dirName + "/";
                }
                uri += filename;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void delDir(string dirName, string ftpHostIP, string username, string password)
        {
            try
            {
                string uri = "ftp://" + ftpHostIP + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="currentFilename">当前目录名称</param>
        /// <param name="newFilename">重命名目录名称</param>
        /// <param name="ftpServerIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void Rename(string currentFilename, string newFilename, string ftpServerIP, string username, string password)
        {
            try
            {

                FileInfo fileInf = new FileInfo(currentFilename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.Rename;

                ftp.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
            //提供身份验证信息
            result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }

        /*
        /// <summary>
        /// 向Ftp服务器上传文件并创建和本地相同的目录结构
        /// 遍历目录和子目录的文件
        /// </summary>
        /// <param name="file"></param>
        private void GetFileSystemInfos(FileSystemInfo file)
        {
            string getDirecName = file.Name;
            if (!ftpIsExistsFile(getDirecName, "192.168.0.172", "Anonymous", "") && file.Name.Equals(FileName))
            {
                MakeDir(getDirecName, "192.168.0.172", "Anonymous", "");
            }
            if (!file.Exists) return;
            DirectoryInfo dire = file as DirectoryInfo;
            if (dire == null) return;
            FileSystemInfo[] files = dire.GetFileSystemInfos();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = files[i] as FileInfo;
                if (fi != null)
                {
                    DirectoryInfo DirecObj = fi.Directory;
                    string DireObjName = DirecObj.Name;
                    if (FileName.Equals(DireObjName))
                    {
                        UploadFile(fi, DireObjName, "192.168.0.172", "Anonymous", "");
                    }
                    else
                    {
                        Match m = Regex.Match(files[i].FullName, FileName + "+.*" + DireObjName);
                        //UploadFile(fi, FileName+"/"+DireObjName, "192.168.0.172", "Anonymous", "");
                        UploadFile(fi, m.ToString(), "192.168.0.172", "Anonymous", "");
                    }
                }
                else
                {
                    string[] ArrayStr = files[i].FullName.Split('\\');
                    string finame = files[i].Name;
                    Match m = Regex.Match(files[i].FullName, FileName + "+.*" + finame);
                    //MakeDir(ArrayStr[ArrayStr.Length - 2].ToString() + "/" + finame, "192.168.0.172", "Anonymous", "");
                    MakeDir(m.ToString(), "192.168.0.172", "Anonymous", "");
                    GetFileSystemInfos(files[i]);
                }
            }
        }
         * */

        /// <summary>
        /// 判断ftp服务器上该目录是否存在
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="ftpHostIP"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static bool ftpIsExistsFile(string dirName, string ftpHostIP, string username, string password,int port)
        {
            bool flag = true;
            try
            {
                string uri = "ftp://" + ftpHostIP +":" + port + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, username, password);
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }
    }
}
