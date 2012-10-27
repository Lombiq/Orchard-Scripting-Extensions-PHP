using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Shapes;

namespace OrchardHUN.Scripting.Php.Services
{
    public class PhpShape
    {
        private readonly dynamic _shape;

        public ShapeMetadata Metadata { get { return _shape.Metadata; } }
        public string Id { get { return _shape.Id; } }
        public IList<string> Classes { get { return _shape.Classes; } }
        public IDictionary<string, string> Attributes { get { return _shape.Attributes; } }
        public IEnumerable<dynamic> Items { get { return _shape.Items; } }
        public object Value { get { return _shape; } }


        public PhpShape(dynamic shape)
        {
            _shape = shape;
        }


        // Argument defaults don't work from PHP
        public PhpShape Add(object item)
        {
            return Add(item, null);
        }
        
        public PhpShape Add(object item, string position)
        {
            _shape.Add(item, position);
            return this;
        }

        public PhpShape AddRange(IEnumerable<object> items)
        {
            return AddRange(items, "5");
        }

        public PhpShape AddRange(IEnumerable<object> items, string position)
        {
            _shape.AddRange(items, position);
            return this;
        }

        public PhpShape Get(string name)
        {
            return new PhpShape(_shape[name]);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}