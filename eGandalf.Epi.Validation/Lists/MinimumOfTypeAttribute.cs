using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eGandalf.Epi.Validation.Lists
{
    /// <summary>
    /// Detects whether the minimum required items of a specific type within a ContentArea condition has been met. Only supports items that can be loaded by IContentLoader. Supports type inheritance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class MinimumOfTypeAttribute : ValidationAttribute
    {
        public int Limit { get; }
        public Type ObjectType { get; }

        public MinimumOfTypeAttribute(int limit, Type t)
        {
            Limit = limit;
            ObjectType = t;
        }

        public override bool IsValid(object value)
        {
            if (value == null && Limit > 0) return false;

            var area = value as ContentArea;
            if (area != null) return ValidateContentArea(area);

            throw new TypeMismatchException("Minimum of type only works with ContentArea properties.");
        }

        private bool ValidateContentArea(ContentArea area)
        {
            if (area?.Items?.Count < Limit) return false;

            var typeCount = 0;
            foreach (var item in area.Items)
            {
                if (CanLoadContentByType(item.ContentLink))
                {
                    typeCount++;
                    // Return as soon as the validation is true.
                    if (typeCount >= Limit) return true;
                }
            }
            return false;
        }

        private bool CanLoadContentByType(ContentReference reference)
        {
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var loaderType = loader.GetType();
            MethodInfo getMethod = loaderType.GetMethod("Get", new Type[] { typeof(ContentReference) });
            MethodInfo genericGet = getMethod.MakeGenericMethod(new[] { ObjectType });

            try
            {
                var content = genericGet.Invoke(loader, new object[] { reference });
                return content != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return $"ContentArea '{name}' must include at least {Limit} items of type {ObjectType.Name}";
        }
    }
}
