using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters.Api.Services.Http.Marvel.Models
{
    public class ContainerData<T>
        where T : class, new()
    {
        public int Offset { get; set; }

        public int Limit { get; set; }

        public int Total { get; set; }

        public int Count { get; set; }

        public List<T> Results { get; set; }
    }


    public class ServiceResult<T>
        where T : class, new()
    {
        public int Code { get; set; }

        public string Status { get; set; }

        public string Etag { get; set; }

        public string Copyright { get; set; }

        public string AttributionText { get; set; }

        public string AttributionHTML { get; set; }

        public ContainerData<T> Data { get; set; }
    }
}