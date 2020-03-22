namespace DnsTestTreeView.Entities
{
    public class TreeNode
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int OrderId { get; set; }

        public bool IsDirectory { get; set; }
    }
}
