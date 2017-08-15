using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace 获取糗事百科的笑话
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            Application.Run(new FrmMain());
        }

        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            var assem=Assembly.GetExecutingAssembly();
            string resourceNmae=null;
            string []resourceNames = assem.GetManifestResourceNames();
            foreach(string s in resourceNames )
            {
                if(s.EndsWith(dllName ))
                {
                    resourceNmae=s;
                }
            }

            if (resourceNmae == null) return null;
            using (var stream = assem.GetManifestResourceStream(resourceNmae))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }

}
