using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TeamCityRestClientNet.RestApi
{
    public abstract class ListDto<T> where T : IdDto
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
}