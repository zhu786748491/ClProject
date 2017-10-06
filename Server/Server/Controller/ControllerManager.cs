using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;//反射
using Server.Server;

namespace Server.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server.Server server;

        public ControllerManager (Server.Server server)
        {
            this.server = server;
            InitController();
        }

        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
        }

        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if(isGet ==false)
            {
                Console.WriteLine("无法得到"+requestCode +"所对应的Ctroller,无法处理请求");
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if(mi==null)
            {
                Console.WriteLine("在Controller[" + controller.GetType() + "]中没有对应的方法[" + methodName + "]");
                return;

            }
            object[] parmeters = new object[] { data ,client,server};
            object o = mi.Invoke(controller, parmeters);
        }

    }
}
