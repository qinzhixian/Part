//-----------------------------------------------------------------------
// <copyright file="IISHelper.cs" company="MY EXPRESS, Ltd.">
//     Copyright (c) 2016 , All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Microsoft.Web.Administration;

namespace Util.Web
{
    /// <summary>
    /// IIS应用 
    /// Microsoft中提供了管理IIS7的一些非常强大的API——Microsoft.Web.Administration，
    /// 可以很方便的让我们以编程的方式管理，设定IIS 7的各项配置。
    /// Microsoft.Web.Administration.dll位于IIS的目录（%WinDir%\System32\InetSrv）下，
    /// 在项目中添加对其的引用后您就可以使用这些API了
    /// 
    /// 
    /// 错误: 由于权限不足而无法读取配置文件 文件名: redirection.config
    /// 解决办法 选择该网站的应用程序池的高级设置里进程模型下的标识选择为LocalSystem
    /// 
    /// 修改纪录
    /// 
    /// 2018-09-10版本：1.0 SongBiao 创建文件。     
    /// 
    /// <author>
    ///     <name>SongBiao</name>
    ///     <date>2018-09-10</date>
    /// </author>
    /// </summary>

    public partial class IISHelper
    {
        /// <summary>
        /// 停止一个站点
        /// </summary>
        public static void StopSite(string site)
        {
            try
            {
                ServerManager iisManager = new ServerManager();
                if (iisManager.Sites[site].State == ObjectState.Stopped || iisManager.Sites[site].State == ObjectState.Stopping)
                {
                    //is stoped
                }
                else
                {
                    ObjectState state = iisManager.Sites[site].Stop();
                    if (state == ObjectState.Stopping || state == ObjectState.Stopped)
                    {
                        //true
                    }
                    else
                    {
                        //stop is error
                    }
                }
            }
            catch (Exception ex)
            {
                //error
            }

        }

        /// <summary>
        /// 启动一个站点
        /// </summary>
        public static void StartSite(string site)
        {
            try
            {
                ServerManager iisManager = new ServerManager();
                if (iisManager.Sites[site].State != ObjectState.Started || iisManager.Sites[site].State != ObjectState.Starting)
                {
                    ObjectState state = iisManager.Sites[site].Start();
                    if (state == ObjectState.Starting || state == ObjectState.Started)
                    {
                        //true
                    }
                    else
                    {
                        //false
                    }
                }
                else
                {
                    //is started
                }
            }
            catch (Exception ex)
            {
                //stop is error
            }

        }

        /// <summary>
        /// 停止应用程序池
        /// </summary>
        /// <param name="appPool"></param>
        public static void StopApplicationPool(string appPool = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appPool))
                {
                    appPool = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
                }
                ServerManager iisManager = new ServerManager();
                if (iisManager.ApplicationPools[appPool].State == ObjectState.Stopped || iisManager.ApplicationPools[appPool].State == ObjectState.Stopping)
                {
                    //is stoped
                }
                else
                {
                    ObjectState state = iisManager.ApplicationPools[appPool].Stop();
                    if (state == ObjectState.Stopping || state == ObjectState.Stopped)
                    {
                        //true
                    }
                    else
                    {
                        //false
                    }
                }
            }
            catch (Exception ex)
            {
                //error
            }

        }

        /// <summary>
        /// 启动应用程序池
        /// </summary>
        /// <param name="appPool"></param>
        public static void StartApplicationPool(string appPool = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appPool))
                {
                    appPool = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
                }
                ServerManager iisManager = new ServerManager();
                if (iisManager.ApplicationPools[appPool].State != ObjectState.Started || iisManager.ApplicationPools[appPool].State != ObjectState.Starting)
                {
                    ObjectState state = iisManager.ApplicationPools[appPool].Start();
                    if (state == ObjectState.Starting || state == ObjectState.Started)
                    {
                        //is started
                    }
                    else
                    {
                        //start error
                    }
                }
                else
                {
                    //is started
                }
            }
            catch (Exception ex)
            {
                //start is error
            }

        }

        /// <summary>
        /// 回收应用程序池
        /// </summary>
        /// <param name="appPool"></param>
        public static void RecycleApplicationPool(string appPool = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appPool))
                {
                    appPool = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
                }
                ServerManager iisManager = new ServerManager();
                ObjectState state = iisManager.ApplicationPools[appPool].Recycle();

                //true

            }
            catch (Exception ex)
            {
                //error
            }

        }

        /// <summary>
        /// 运行时控制：得到当前正在处理的请求
        /// </summary>
        /// <param name="appPool"></param>
        public static void GetWorking(string appPool)
        {
            ServerManager iisManager = new ServerManager();
            foreach (WorkerProcess w3wp in iisManager.WorkerProcesses)
            {
                Console.WriteLine("W3WP ({0})", w3wp.ProcessId);
                foreach (Request request in w3wp.GetRequests(0))
                {
                    Console.WriteLine("{0} - {1},{2},{3}",
                                request.Url,
                                request.ClientIPAddr,
                                request.TimeElapsed,
                                request.TimeInState);
                }
            }
        }

        /// <summary>
        /// 获取IIS日志文件路径
        /// </summary>
        /// <returns></returns>

        public static string GetIISLogPath()
        {
            ServerManager manager = new ServerManager();
            // 获取IIS配置文件：applicationHost.config
            var config = manager.GetApplicationHostConfiguration();
            var log = config.GetSection("system.applicationHost/log");
            var logFile = log.GetChildElement("centralW3CLogFile");
            //获取网站日志文件保存路径
            var logPath = logFile.GetAttributeValue("directory").ToString();
            return logPath;
        }

        /// <summary>
        ///创建新站点
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="port"></param>
        /// <param name="physicalPath"></param>
        public static void CreateSite(string siteName, int port, string physicalPath)
        {
            createSite(siteName, port, physicalPath, true, siteName + "Pool", ProcessModelIdentityType.NetworkService, null, null, ManagedPipelineMode.Integrated, null);
        }

        /// <summary>
        /// 创建新站点
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="port"></param>
        /// <param name="physicalPath"></param>
        /// <param name="createAppPool"></param>
        /// <param name="appPoolName"></param>
        /// <param name="identityType"></param>
        /// <param name="appPoolUserName"></param>
        /// <param name="appPoolPassword"></param>
        /// <param name="appPoolPipelineMode"></param>
        /// <param name="managedRuntimeVersion"></param>
        private static void createSite(string siteName, int port, string physicalPath, bool createAppPool, string appPoolName, ProcessModelIdentityType identityType, string appPoolUserName, string appPoolPassword, ManagedPipelineMode appPoolPipelineMode, string managedRuntimeVersion)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites.Add(siteName, physicalPath, port);

                // PROVISION APPPOOL IF NEEDED
                if (createAppPool)
                {
                    ApplicationPool pool = mgr.ApplicationPools.Add(appPoolName);
                    if (pool.ProcessModel.IdentityType != identityType)
                    {
                        pool.ProcessModel.IdentityType = identityType;
                    }
                    if (!String.IsNullOrEmpty(appPoolUserName))
                    {
                        pool.ProcessModel.UserName = appPoolUserName;
                        pool.ProcessModel.Password = appPoolPassword;
                    }
                    if (appPoolPipelineMode != pool.ManagedPipelineMode)
                    {
                        pool.ManagedPipelineMode = appPoolPipelineMode;
                    }

                    site.Applications["/"].ApplicationPoolName = pool.Name;
                }

                mgr.CommitChanges();
            }
        }

        /// <summary>
        /// Delete an existent web site.
        /// </summary>
        /// <param name="siteName">Site name.</param>
        public static void DeleteSite(string siteName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    mgr.Sites.Remove(site);
                    mgr.CommitChanges();
                }
            }
        }

        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="vDirName"></param>
        /// <param name="physicalPath"></param>
        public static void CreateVDir(string siteName, string vDirName, string physicalPath)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site == null)
                {
                    throw new ApplicationException(String.Format("Web site {0} does not exist", siteName));
                }
                site.Applications.Add("/" + vDirName, physicalPath);
                mgr.CommitChanges();
            }
        }

        /// <summary>
        /// 删除虚拟目录
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="vDirName"></param>
        public static void DeleteVDir(string siteName, string vDirName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    Microsoft.Web.Administration.Application app = site.Applications["/" + vDirName];
                    if (app != null)
                    {
                        site.Applications.Remove(app);
                        mgr.CommitChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Delete an existent web site app pool.
        /// </summary>
        /// <param name="appPoolName">App pool name for deletion.</param>
        public static void DeletePool(string appPoolName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                ApplicationPool pool = mgr.ApplicationPools[appPoolName];
                if (pool != null)
                {
                    mgr.ApplicationPools.Remove(pool);
                    mgr.CommitChanges();
                }
            }
        }

        /// <summary>
        /// 在站点上添加默认文档。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="defaultDocName"></param>
        public static void AddDefaultDocument(string siteName, string defaultDocName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Configuration cfg = mgr.GetWebConfiguration(siteName);
                ConfigurationSection defaultDocumentSection = cfg.GetSection("system.webServer/defaultDocument");
                ConfigurationElement filesElement = defaultDocumentSection.GetChildElement("files");
                ConfigurationElementCollection filesCollection = filesElement.GetCollection();

                foreach (ConfigurationElement elt in filesCollection)
                {
                    if (elt.Attributes["value"].Value.ToString() == defaultDocName)
                    {
                        return;
                    }
                }

                try
                {
                    ConfigurationElement docElement = filesCollection.CreateElement();
                    docElement.SetAttributeValue("value", defaultDocName);
                    filesCollection.Add(docElement);
                }
                catch (Exception) { }   //this will fail if existing

                mgr.CommitChanges();
            }
        }

        /// <summary>
        ///   检查虚拟目录是否存在。
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="path"></param>
        /// <returns></returns>

        public static bool VerifyVirtualPathIsExist(string siteName, string path)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    foreach (Microsoft.Web.Administration.Application app in site.Applications)
                    {
                        if (app.Path.ToUpper().Equals(path.ToUpper()))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///  检查站点是否存在。
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static bool VerifyWebSiteIsExist(string siteName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                for (int i = 0; i < mgr.Sites.Count; i++)
                {
                    if (mgr.Sites[i].Name.ToUpper().Equals(siteName.ToUpper()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///   检查Bindings信息。
        /// </summary>
        /// <param name="bindingInfo"></param>
        /// <returns></returns>
        public static bool VerifyWebSiteBindingsIsExist(string bindingInfo)
        {
            string temp = string.Empty;
            using (ServerManager mgr = new ServerManager())
            {
                for (int i = 0; i < mgr.Sites.Count; i++)
                {
                    foreach (Microsoft.Web.Administration.Binding b in mgr.Sites[i].Bindings)
                    {
                        temp = b.BindingInformation;
                        if (temp.IndexOf('*') < 0)
                        {
                            temp = "*" + temp;
                        }
                        if (temp.Equals(bindingInfo))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

    }
}