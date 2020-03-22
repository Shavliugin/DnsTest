using DnsTestTreeView.Entities;

namespace DnsTestTreeView.BuisnessLogic.Requests
{
    public class CreateTreeNodeRequest
    {
        public int? ParentId { get; set; }

        public bool IsDirectory { get; set; }
        
        public string Name { get; set; }
    }
}
