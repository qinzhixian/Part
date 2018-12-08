using System;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                //DataCenter.Global Service = new DataCenter.Global("Model",true);

                //Service.Add(new Model.UserInfo() { UserId = Guid.NewGuid().ToString(), UserName = "qzx", Age = 21 });

                //Service.Update(t => t.UserId == t.UserId, new Model.UserInfo { UserId = "123", Age = 111, UserName = "2222222" });

                //Service.Remove<Model.UserInfo>(t => t.UserName == "2222222");

                //var list = Service.GetList<Model.UserInfo>();

                //Console.WriteLine(list.Count);
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
