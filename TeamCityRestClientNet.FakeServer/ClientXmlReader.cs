
using System.Linq;
using System.Xml;

namespace TeamCityRestClientNet.FakeServer
{
    /// <summary>
    /// Nabb'd from https://stackoverflow.com/questions/44543244/change-properties-to-camelcase-when-serializing-to-xml-in-c-sharp.
    /// </summary>
    public class ClientXmlReader : XmlReader
    {
        private bool _disposedValue;
        private XmlReader _reader;
        private XmlNameTableWrapper _nameTable;

        private class XmlNameTableWrapper : XmlNameTable
        {
            private XmlNameTable _wrapped;
            // Some names that are added by default to this collection. We can skip the lower casing logic on them.
            private string[] _defaultNames = new string[]
            {
                "http://www.w3.org/2001/XMLSchema",
                "http://www.w3.org/2000/10/XMLSchema",
                "http://www.w3.org/1999/XMLSchema",
                "http://microsoft.com/wsdl/types/",
                "http://www.w3.org/2001/XMLSchema-instance",
                "http://www.w3.org/2000/10/XMLSchema-instance",
                "http://www.w3.org/1999/XMLSchema-instance",
                "http://schemas.xmlsoap.org/soap/encoding/",
                "http://www.w3.org/2003/05/soap-encoding",
                "schema",
                "http://schemas.xmlsoap.org/wsdl/",
                "arrayType",
                "null",
                "nil",
                "type",
                "arrayType",
                "itemType",
                "arraySize",
                "Array",
                "anyType"
            };

            public XmlNameTableWrapper(XmlNameTable wrapped)
            {
                this._wrapped = wrapped;
            }

            public override string Add(char[] array, int offset, int length)
            {
                if (array != null && array.Length > 0 && !_defaultNames.Any(n => n == new string(array)))
                {
                    array[0] = char.ToLower(array[0]);
                }
                return _wrapped.Add(array, offset, length);
            }

            public override string Add(string array)
            {
                if (array != null && !_defaultNames.Any(n => n == array))
                {
                    if (array.Length < 2)
                    {
                        array = array.ToLower();
                    }
                    else
                        array = char.ToLower(array[0]) + array.Substring(1);
                }
                return _wrapped.Add(array);
            }

            public override string Get(char[] array, int offset, int length)
            {
                if (array != null && array.Length > 0 && !_defaultNames.Any(n => n == new string(array)))
                {
                    array[0] = char.ToLower(array[0]);
                }
                return _wrapped.Get(array, offset, length);
            }

            public override string Get(string array)
            {
                if (array != null && !_defaultNames.Any(n => n == array))
                {
                    if (array.Length < 2)
                    {
                        array = array.ToLower();
                    }
                    array = char.ToLower(array[0]) + array.Substring(1);
                }
                return _wrapped.Get(array);
            }
        }

        public ClientXmlReader(XmlReader reader)
        {
            this._reader = reader;
            _nameTable = new XmlNameTableWrapper(reader.NameTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _reader.Dispose();
                    base.Dispose(disposing);
                }
                _disposedValue = true;
            }
        }

        public override XmlNameTable NameTable => _nameTable;
        public override XmlNodeType NodeType => _reader.NodeType;
        public override string LocalName => _reader.LocalName;
        public override string NamespaceURI => _reader.NamespaceURI;
        public override string Prefix => _reader.Prefix;
        public override string Value => _reader.Value;
        public override int Depth => _reader.Depth;
        public override string BaseURI => _reader.BaseURI;
        public override bool IsEmptyElement => _reader.IsEmptyElement;
        public override int AttributeCount => _reader.AttributeCount;
        public override bool EOF => _reader.EOF;
        public override ReadState ReadState => _reader.ReadState;
        public override string GetAttribute(string name) => _reader.GetAttribute(name);
        public override string GetAttribute(string name, string namespaceURI) => _reader.GetAttribute(name, namespaceURI);
        public override string GetAttribute(int i) => _reader.GetAttribute(i);
        public override string LookupNamespace(string prefix) => _reader.LookupNamespace(prefix);
        public override bool MoveToAttribute(string name) => _reader.MoveToAttribute(name);
        public override bool MoveToAttribute(string name, string ns) => _reader.MoveToAttribute(name, ns);
        public override bool MoveToElement() => _reader.MoveToElement();
        public override bool MoveToFirstAttribute() => _reader.MoveToFirstAttribute();
        public override bool MoveToNextAttribute() => _reader.MoveToNextAttribute();
        public override bool Read() => _reader.Read();
        public override bool ReadAttributeValue() => _reader.ReadAttributeValue();
        public override void ResolveEntity() => _reader.ResolveEntity();
    }
}