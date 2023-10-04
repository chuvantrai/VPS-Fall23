using AutoMapper;
using Minio;
using VPS.MinIO.BusinessObjects;

namespace VPS.MinIO.Repository.AutoMapperProfile
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreatePutObjectResponseMapperProfile();

        }
        public IMappingExpression<PutObjectResponse, PutObjectResponseDto> CreatePutObjectResponseMapperProfile()
        {
            return this.CreateMap<PutObjectResponse, PutObjectResponseDto>()
                .ForMember(x => x.ObjectName, y => y.MapFrom(source => source.ObjectName))
                .ForMember(x => x.ETag, y => y.MapFrom(source => source.Etag))
                .ForMember(x => x.Size, y => y.MapFrom(source => source.Size));
        }
    }
}
