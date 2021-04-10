using System;
using System.Text.Json.Serialization;
using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Category: BaseEntity
    {
        private string _name, _description, _color, _icon;
        public string Name { get { return _name;  } set { _name = value; HasChanges = true; } }
        public string Description { get { return _description; } set { _description = value; HasChanges = true; } }
        public string Color { get { return _color; } set { _color = value; HasChanges = true; } }
        public string Icon { get { return _icon; } set { _icon = value; HasChanges = true; } }

        [JsonConstructor]
        public Category(Guid id, string name, string description, string color, string icon)
        {
            Id = id;
            Name = name;
            Description = description;
            Color = color;
            Icon = icon;
            HasChanges = false;
        }

        public Category(string name, string description, string color, string icon):
            this(Guid.NewGuid(), name, description, color, icon)
        {
            IsNew = true;
        }

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