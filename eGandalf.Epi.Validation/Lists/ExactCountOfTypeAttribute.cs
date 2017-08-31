using eGandalf.Epi.Validation.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eGandalf.Epi.Validation.Lists
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ExactCountOfTypeAttribute : ValidationAttribute
    {
        public int Limit { get; set; }
        public Type ObjectType { get; set; }

        public ExactCountOfTypeAttribute(int limit, Type t)
        {
            Limit = limit;
            ObjectType = t;
        }

        public override bool IsValid(object value)
        {
            if (value == null && Limit == 0) return true;

            if (value is ContentArea area) return ValidateContentArea(area);

            throw new TypeMismatchException("Exact Count of type validation is only supported with ContentArea properties.");
        }

        private bool ValidateContentArea(ContentArea area)
        {
            if (area == null || !area.Items.Any()) return false;

            var typeCount = 0;
            foreach(var item in area?.Items)
            {
                if (CanLoadContentByType(item.ContentLink))
                {
                    typeCount++;
                }
            }

            return typeCount == Limit;
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
            return ValidationLocalization.GetFormattedErrorMessage("exactcountoftype", new object[] { name, Limit, ObjectType.Name });
        }
    }
}
