using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region DataCenter 1.0

                //DataCenter.Global service = new DataCenter.Global("Model", true);

                //for (int i = 0; i < 100; i++)
                //{
                //    service.Add(new Model.UserInfo { Age = i, UserId = Guid.NewGuid().ToString(), UserName = i.ToString() });
                //}

                //var data = service.GetList<Model.UserInfo>().OrderBy(t => t.Age);
                //foreach (var item in data)
                //{
                //    Console.WriteLine(item.UserName);
                //}

                #endregion

                #region DataCenter 2.0

                //DataCenter.Global.Start("Model");

                //for (int i = 0; i < 100; i++)
                //{
                //    DataCenter.Global.Add(new Model.UserInfo { Age = i, UserId = Guid.NewGuid().ToString(), UserName = i.ToString() });
                //}

                //var data = DataCenter.Global.GetList<Model.UserInfo>().OrderBy(t => t.Age);

                //foreach (var item in data)
                //{
                //    Console.WriteLine(item.UserName);
                //}

                #endregion

                //Util.Web.IISHelper.CreateSite("mySite", 3389, @"D:\WebSite\XingJiManage");


                //var task = new TaskFactory().StartNew(() =>
                //{
                //    Console.WriteLine("1");
                //}).ContinueWith((s) =>
                //{
                //    Console.WriteLine("2");
                //});

                //List<Task> list = new List<Task>() { task };
                //Task.WaitAll(list.ToArray());

                Console.WriteLine("start");


                Util.Reflection.Assembly assembly = new Util.Reflection.Assembly();
                assembly.Init();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 10000; i++)
                {
                    var res = assembly.CallStaticMethod("DateTime", "ToDateTime", new object[] { "2019-06-05" });
                }

                sw.Stop();

                var first = sw.ElapsedTicks;


                sw.Restart();

                for (int i = 0; i < 10000; i++)
                {
                    Util.DateTime.ToDateTime("2019-06-05");
                }

                sw.Stop();

                var secend = sw.ElapsedTicks;

                Console.WriteLine("end");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("执行完毕");
            Console.ReadKey();
        }
    }
}