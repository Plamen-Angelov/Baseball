using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Baseball.Common.Binders
{
    public class DoubleModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.GetType() == typeof(double))
            {
                return new DoubleModelBinder();
            }

            return null;
        }
    }
}
