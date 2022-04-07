namespace DontWreckMyHouse.UI
{
    public enum SearchOption
    {
        Exit,
        SearchByEmail,
        PickFromList
    }

    public static class SearchOptionExtensions
    {
        public static string ToLabel(this SearchOption option) => option switch
        {
            SearchOption.Exit => "Exit Search",
            SearchOption.SearchByEmail => "Search By Email",
            SearchOption.PickFromList => "Pick From a List",
            _ => throw new NotImplementedException()
        };
    }
}
