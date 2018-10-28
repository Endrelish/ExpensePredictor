using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Mapper;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public static class MapperService
    {
        public static IMapper Mapper { get; }

        static MapperService()
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            Mapper = mapperConfig.CreateMapper();
        }
        
    }
}