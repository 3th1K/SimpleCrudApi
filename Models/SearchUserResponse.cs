namespace CrudApiAssignment.Models;

public class SearchUserResponse
{
    public List<User> Users { get; set; }
    public string SortingOrder { get; set; } = "Ascending";
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
