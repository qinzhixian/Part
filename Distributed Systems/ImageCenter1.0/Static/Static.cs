using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCenter
{
    public static class Static
    {
        public const string ServerAesKey = "DC4BF57EA30257894A78EC0128FCD71A";
       
        public const string ImageFileOrg = "ImageServer";

        public static string FileDirPath { get { return string.Format("{0}/Files/Images/", Util.IO.Directory.GetCurrentDirectory()); } }
    }
}