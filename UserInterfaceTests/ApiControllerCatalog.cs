using System;
using System.Collections.Generic;
using Nancy;

namespace UserInterfaceTests
{
    public class ApiControllerCatalog : INancyModuleCatalog
    {
        static List<INancyModule> Modules = new List<INancyModule>();
        static Dictionary<Type, INancyModule> ModulesByType = new Dictionary<Type, INancyModule>();
        public static void AddNancyModule(INancyModule module)
        {
            Modules.Add(module);
            ModulesByType.Add(module.GetType(), module);
        }

        public static void Reset()
        {
            Modules.Clear();
            ModulesByType.Clear();
        }

        public IEnumerable<INancyModule> GetAllModules(NancyContext context)
        {
            return Modules;
        }

        public INancyModule GetModule(Type moduleType, NancyContext context)
        {
            return ModulesByType[moduleType];
        }
    }
}