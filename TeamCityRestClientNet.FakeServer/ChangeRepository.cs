// using System.Linq;
// using TeamCityRestClientNet.RestApi;

// namespace TeamCityRestClientNet.FakeServer
// {
//     class ChangeRepository : BaseRepository<ChangeDto>
//     {
//         static ChangeRepository() { }

//         public ChangeRepository() 
//             : base(item => item.Id) { }

//         public UserListDto All() => new UserListDto { User = AllItems() };
//     }
// }