namespace Phoenix.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
       
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
