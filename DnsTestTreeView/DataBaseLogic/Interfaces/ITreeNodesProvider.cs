using DnsTestTreeView.Entities;
using System.Collections.Generic;

namespace DnsTestTreeView.DataBaseLogic.Interfaces
{
    // Не реализовывал Delete функционал, потому что в данной задаче он не требовался
    public interface ITreeNodesProvider
    {
        public IEnumerable<TreeNode> GetTreeNodeList(int? parentId);

        public TreeNode GetTreeNode(int id);

        public int GetTreNodeChildCount(int id);

        public void UpdateNode(TreeNode node);

        public int InsertNode(TreeNode node);

        public void ShiftOrderId(int? parentId, int startOrderId, int step);
    }
}
