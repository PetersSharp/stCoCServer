﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.2034
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stSqlite.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("stSqlite.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Ищет локализованную строку, похожую на Bad inherit class: {0}.
        /// </summary>
        internal static string ConvertErrorBadInheritClass {
            get {
                return ResourceManager.GetString("ConvertErrorBadInheritClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на source is not Json array.
        /// </summary>
        internal static string ConvertErrorJsonArray {
            get {
                return ResourceManager.GetString("ConvertErrorJsonArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на errorp parsing Column name : {0} ({1}).
        /// </summary>
        internal static string ConvertErrorToDataTable {
            get {
                return ResourceManager.GetString("ConvertErrorToDataTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SQLiteFunction doesn&apos;t have SQLiteFunctionAttribute.
        /// </summary>
        internal static string DBFunctionEmptyAttribute {
            get {
                return ResourceManager.GetString("DBFunctionEmptyAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на database name is empty.
        /// </summary>
        internal static string DBNameEmpty {
            get {
                return ResourceManager.GetString("DBNameEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на database not open.
        /// </summary>
        internal static string DBNotOpen {
            get {
                return ResourceManager.GetString("DBNotOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на no database specified.
        /// </summary>
        internal static string DBStringEmpty {
            get {
                return ResourceManager.GetString("DBStringEmpty", resourceCulture);
            }
        }
    }
}
