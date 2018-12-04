using AutoMapper;

namespace ExpensePrediction.BusinessLogicLayer.Mapper
{
    public static class MapperService
    {
        static MapperService()
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            Mapper = mapperConfig.CreateMapper();
        }

        public static IMapper Mapper { get; }
    }
}