using DnsTestTreeView.BuisnessLogic.Requests;
using DnsTestTreeView.Entities;
using DnsTestTreeView.DataBaseLogic.Interfaces;
using System.Collections.Generic;

namespace DnsTestTreeView.BuisnessLogic
{
    public class TreeNodeManager : ITreeNodeManager
    {
        private readonly ITreeNodesProvider _treeNodeProvider;
        public TreeNodeManager(ITreeNodesProvider treeNodeProvider)
        {
            _treeNodeProvider = treeNodeProvider;
        }

        public TreeNode CreateNode(CreateTreeNodeRequest request)
        {
            TreeNode result = null;

            if (request != null)
            {
                result = _treeNodeProvider.GetTreeNode(_treeNodeProvider.InsertNode(new TreeNode()
                {
                    IsDirectory = request.IsDirectory,
                    Name = request.Name,
                    ParentId = request.ParentId
                }));
            }

            return result;
        }

        public IEnumerable<TreeNode> GetTreeNodeList(int? parentId)
        {
            return _treeNodeProvider.GetTreeNodeList(parentId);
        }

        public void ReplaceTreeNode(ReplaceTreeNodeRequest request)
        {
            var draggableNode = _treeNodeProvider.GetTreeNode(request.DraggableTreeNodeId);

            var containerNode = _treeNodeProvider.GetTreeNode(request.ContainerTreeNodeId);


            if (containerNode.IsDirectory)
            {
                _treeNodeProvider.ShiftOrderId(draggableNode.ParentId, draggableNode.OrderId + 1, -1);
                draggableNode.OrderId = _treeNodeProvider.GetTreNodeChildCount(request.ContainerTreeNodeId) - 1;
                draggableNode.ParentId = request.ContainerTreeNodeId;
            }
            else
            {
                _treeNodeProvider.ShiftOrderId(draggableNode.ParentId, draggableNode.OrderId + 1, -1);
                draggableNode.OrderId = containerNode.OrderId;
                draggableNode.ParentId = containerNode.ParentId;
                _treeNodeProvider.ShiftOrderId(containerNode.ParentId, containerNode.OrderId, 1);
            }            

            _treeNodeProvider.UpdateNode(draggableNode);
        }
    }
}
