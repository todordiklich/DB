namespace VaporStore
{
	using AutoMapper;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
			this.CreateMap<UserJsonInputModel, User>();
			this.CreateMap<CardJsonInputModel, Card>();
		}
	}
}