﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.2034
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stCoCServerConfig.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("stCoCServerConfig.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на no user setting, run settings default...
        /// </summary>
        internal static string cmdEmptyCmd {
            get {
                return ResourceManager.GetString("cmdEmptyCmd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на build default configuration: &apos;{0}&apos;.
        /// </summary>
        internal static string iniFileBuildNew {
            get {
                return ResourceManager.GetString("iniFileBuildNew", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на error open &apos;{0}&apos; configuration.
        /// </summary>
        internal static string iniFileInternalError {
            get {
                return ResourceManager.GetString("iniFileInternalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на parse configuration ini file: IpFilter out of range.
        /// </summary>
        internal static string iniFilterRange {
            get {
                return ResourceManager.GetString("iniFilterRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на configuration not found sections for field {0}.
        /// </summary>
        internal static string iniSectionsNotFound {
            get {
                return ResourceManager.GetString("iniSectionsNotFound", resourceCulture);
            }
        }
    }
}
