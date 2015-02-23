/*
    * This file contains the user kernel, which contains all needed functions for managing the users of the program.
    * @license
    *
    * This program is free software; you can redistribute it and/or
    * modify it under the terms of the GNU General Public License as
    * published by the Free Software Foundation; either version 3 of
    * the License, or (at your option) any later version.
    *
    * This program is distributed in the hope that it will be useful, but
    * WITHOUT ANY WARRANTY; without even the implied warranty of
    * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
    * General Public License for more details at
    * http://www.gnu.org/copyleft/gpl.html
    *
    * @author lw1994 <luca@welkr.de>
    *
    * @todo  
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace RedundancyAccessLibrary.Domain.Attributes
{
    public sealed class ApiModuleTypeAttribute : Attribute
    {
        #region constructors

        public ApiModuleTypeAttribute(string typeString)
        {
            if (String.IsNullOrEmpty(typeString))
                throw new ArgumentNullException("typeString");
            Type = typeString;
        }

        #endregion

        #region properties

        public string Type { get; private set; }

        #endregion
    }

    public static class ApiModuleTypeHelper
    {
        #region constructors

        static ApiModuleTypeHelper()
        {
            _typesCache = new Lazy<Dictionary<ApiModule, string>>(() => { return new Dictionary<ApiModule, string>(); });
        }

        #endregion

        #region properties

        private static readonly Lazy<Dictionary<ApiModule, string>> _typesCache;
        private static Dictionary<ApiModule, string> TypesCache { get { return _typesCache.Value; } }

        #endregion

        public static string GetModuleType(this ApiModule module)
        {
            if (!TypesCache.Any())
                UpdateTypesCache();

            if (TypesCache.ContainsKey(module))
                return TypesCache[module];

            throw new NotSupportedException(String.Format("The module \"{0}\" isn't supported currently.", module));
        }

        public static void UpdateTypesCache()
        {
            var members = typeof(ApiModule).GetMembers().Where(m => m.IsDefined(typeof(ApiModuleTypeAttribute), true));
            foreach (var member in members)
            {
                ApiModule module;
                if (!Enum.TryParse<ApiModule>(member.Name, out module))
                    continue;

                var attr = (ApiModuleTypeAttribute)member.GetCustomAttributes(typeof(ApiModuleTypeAttribute), true).First(); // we can use .First() here without problems, because we know that the attribute is defined  => see where clause!

                if (TypesCache.ContainsKey(module))
                    TypesCache[module] = attr.Type;
                else
                    TypesCache.Add(module, attr.Type);
            }
        }
    }
}