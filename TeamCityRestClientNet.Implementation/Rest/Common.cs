using System.Collections.Generic;
using System.Xml.Serialization;

namespace TeamCityRestClientNet.RestApi
{
    public interface IListDto<T>
    {
        List<T> Items { get; set; }
        int Count { get; }
        string Href { get; set; }
        string NextHref { get; set; }
    }

    public abstract class ListDto<T> : IListDto<T>
        where T : IdDto
    {
        public abstract List<T> Items { get; set; }
        public int Count => Items.Count;
        public string Href { get; set; }
        public string NextHref { get; set; }
    }

    public abstract class IdDto
    {
        public string Id { get; set; }
    }

    public class ParametersDto
    {
        public List<ParameterDto> Property { get; set; } = new List<ParameterDto>();

        public ParametersDto() { }
        public ParametersDto(List<ParameterDto> properties)
        {
            Property = properties;
        }
    }

    public class ParameterDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool? Own { get; set; }

        public ParameterDto(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
    
    public class NameValuePropertiesDto
    {
        public List<NameValuePropertyDto> Property { get; set; } = new List<NameValuePropertyDto>();
    }

    public class NameValuePropertyDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ReferenceDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class PropertiesDto
    {
        [XmlElement("property")]
        public List<PropertyDto> Property { get; set; }
    }

    public class PropertyDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}