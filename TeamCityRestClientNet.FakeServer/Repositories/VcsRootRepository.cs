using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    class VcsRootRepository : BaseRepository<VcsRootDto, VcsRootListDto>
    {
        public VcsRootDto Create(string xmlString)
        {
            using (var strReader = new StringReader(xmlString))
            using (var xmlReader = XmlReader.Create(strReader))
            {
                var serializer = new XmlSerializer(typeof(NewVcsRoot));
                var newDto = serializer.Deserialize(xmlReader) as NewVcsRoot;

                // TODO: Refactor id checks elsewhere. TeamCity has a limited set of characters which are suitable
                // for Ids. - is not one of those characters.
                if (newDto.Id.Contains('-'))
                {
                    throw new InvalidOperationException("Invalid character in id.");
                }

                var dto = newDto.ToVcsRootDto();
                _itemsById.Add(dto.Id, dto);
                return dto;
            }
        }
    }

    class VcsRootInstanceRepository : BaseRepository<VcsRootInstanceDto, VcsRootInstanceListDto>
    {
        
    }
}