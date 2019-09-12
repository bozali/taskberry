namespace TaskBerry.Data.Entities
{
    using System.Reflection;
    using System.Linq;
    using System;


    /// <summary>
    /// Base entity class.
    /// </summary>
    /// <typeparam name="TModel">The target type that the entity can be converted to.</typeparam>
    public abstract class EntityBase<TModel> : IEntity where TModel : class, new()
    {
        /// <summary>
        /// Converts a entity to a model.
        /// </summary>
        /// <returns>Returns a model based on the entity.</returns>
        public virtual TModel ToModel()
        {
            TModel model = new TModel();

            // Trying to convert entity to model with the help of reflection.
            PropertyInfo[] genericProperties = model.GetType().GetProperties();
            PropertyInfo[] thisProperties = this.GetType().GetProperties();

            foreach (PropertyInfo thisProperty in thisProperties)
            {
                // Compare the generic property name with this property name.
                PropertyInfo property = genericProperties.FirstOrDefault(p =>
                    thisProperty.Name.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase) &&
                    thisProperty.GetType() == p.GetType());

                if (property == null) continue;

                property.SetValue(model, thisProperty.GetValue(this));
            }

            return model;
        }
    }
}