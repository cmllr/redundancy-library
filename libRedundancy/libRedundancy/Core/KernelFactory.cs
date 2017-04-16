using RedundancyLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RedundancyLibrary.Core
{
    public sealed class KernelFactory
    {
        #region constructor

        public KernelFactory(string host)
        {
            _baseHost = host;
            _cachedTypes = new Lazy<Dictionary<Type, object>>(CreateCachedTypes);
        }

        #endregion

        #region properties

        private readonly string _baseHost;

        private readonly Lazy<Dictionary<Type, object>> _cachedTypes;
        private Dictionary<Type, object> CachedTypes { get { return _cachedTypes.Value; } }

        private Dictionary<Type, object> CreateCachedTypes()
        {
            var result = new Dictionary<Type, object>();
            var type = Assembly.GetAssembly(this.GetType());
            var members = type.GetTypes().Where(t => typeof(Kernel).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var m in members)
            {
                var iface = m.GetInterfaces().SingleOrDefault(t => typeof(IKernel).IsAssignableFrom(t));
                if (iface == null)
                    continue;

                var instance = Activator.CreateInstance(m, _baseHost);
                if (instance == null)
                    continue;

                result.Add(iface, instance);
            }

            return result;
        }

        #endregion

        public T CreateKernel<T>() where T : class
        {
            var type = typeof(T);
            if (!CachedTypes.ContainsKey(type))
                return null;

            return (T)CachedTypes[type];
        }
    }
}
