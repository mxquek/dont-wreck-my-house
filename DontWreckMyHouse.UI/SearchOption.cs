namespace DontWreckMyHouse.UI
{
    public enum SearchOption
    {
        Exit,
        SearchByEmail,
        PickFromList,
        SearchByID
    }

    public static class SearchOptionExtensions
    {
        public static string ToLabel(this SearchOption option) => option switch
        {
            SearchOption.Exit => "Exit Search",
            SearchOption.SearchByEmail => "Search By Email",
            SearchOption.PickFromList => "Pick From a List",
            SearchOption.SearchByID => "Search By ID",
            _ => throw new NotImplementedException()
        };
    }
}
