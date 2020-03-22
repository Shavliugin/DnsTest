using DnsTestTreeView.Entities;
using DnsTestTreeView.BuisnessLogic.Requests;
using System.Collections.Generic;

namespace DnsTestTreeView.BuisnessLogic
{
    public interface ITreeNodeManager
    {
        TreeNode CreateNode(CreateTreeNodeRequest request);

        void ReplaceTreeNode(ReplaceTreeNodeRequest request);

        IEnumerable<TreeNode> GetTreeNodeList(int? parentId = null);
    }
}
