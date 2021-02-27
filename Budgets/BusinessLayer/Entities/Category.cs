using Budgets.Common;

namespace Budgets.BusinessLayer.Entities
{
    public class Category: BaseEntity
    {
        private static int InstanceCount { get; set; }
        private string _name;
        private string _description;
        private string _color;
        private string _icon;

        public Category(int id, string name, string description, string color, string icon)
        {
            Id = id;
            _name = name;
            _description = description;
            _color = color;
            _icon = icon;
        }

        public Category(string name, string description, string color, string icon):
            this(++InstanceCount, name, description, color, icon)
        {
            IsNew = true;
        }

        public Category( string name, string description) :
        this(name, description, null, null)
        { }

        public string MyProperty
        {
            get { return _color; }
            set { _color = value; HasChanges = true;  }
        }


        public string Description
        {
            get { return _description; }
            set { _description = value; HasChanges = true;  }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; HasChanges = true;  }
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; HasChanges = true;  }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; HasChanges = true; }
        }

        public override bool Validate()
        {
            bool validColor = _color == null || Validator.ValidateColor(_color);
            bool validIcon = _color  == null || Validator.ValidateIcon(_icon);

            return
                !string.IsNullOrWhiteSpace(_name) &
                !string.IsNullOrWhiteSpace(_description) &
                validColor & validIcon;
        }
    }
}