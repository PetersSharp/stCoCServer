using System.ComponentModel;
using System.Reflection;

namespace stLog
{
    [Description("auto set default attribute in UserControl init")]
    public static class stDefaultValue
    {
        public static void InitAttrDefaults(this object obj)
        {
            PropertyInfo[] props = obj.GetType().GetProperties(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.NonPublic
            );
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttributes(true).Length > 0)
                {
                    object[] defaultValueAttributes =
                        prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);

                    if ((defaultValueAttributes != null) && (defaultValueAttributes.Length > 0))
                    {
                        DefaultValueAttribute defaultValueAttribute = defaultValueAttributes[0] as DefaultValueAttribute;

                        if (defaultValueAttribute != null)
                        {
                            prop.SetValue(obj, defaultValueAttribute.Value, null);
                        }
                    }
                }
            }
        }
    }
}
