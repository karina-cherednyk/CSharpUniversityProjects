using System;
using System.Text.Json.Serialization;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }

        [JsonConstructor]
        public Category(Guid id, string name, string description, string color, string icon)
        {
            Id = id;
            Name = name;
            Description = description;
            Color = color;
            Icon = icon;
        }

        public Category(string name, string description, string color, string icon):
            this(Guid.NewGuid(), name, description, color, icon)
        { }

        public Category( string name, string description) :
        this(name, description, null, null)
        { }

      
        public override bool Validate()
        {
            bool validColor = Color == null || Validator.ValidateColor(Color);
            bool validIcon = Icon == null || Validator.ValidateIcon(Icon);

            return
                !string.IsNullOrWhiteSpace(Name) &
                !string.IsNullOrWhiteSpace(Description) &
                validColor & validIcon;
        }
    }
}