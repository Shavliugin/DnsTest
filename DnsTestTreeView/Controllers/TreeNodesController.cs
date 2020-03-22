using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DnsTestTreeView.BuisnessLogic;
using DnsTestTreeView.BuisnessLogic.Requests;
using DnsTestTreeView.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DnsTestTreeView.Controllers
{
    [Route("api/[controller]")]
    public class TreeNodesController : Controller
    {
        private readonly ITreeNodeManager _treeNodeManager;

        public TreeNodesController(ITreeNodeManager treeNodeManager)
        {
            _treeNodeManager = treeNodeManager;
        } 

        //Для того, чтобы можно было насладиться ожиданием загрузки :>
        private void Sleep()
        {
            Thread.Sleep(1000);
        }

        [HttpGet]        
        public ActionResult<IEnumerable<TreeNode>> Get()
        {
            return _treeNodeManager.GetTreeNodeList().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<TreeNode>> GetByParent(int id)
        {
            return _treeNodeManager.GetTreeNodeList(id).ToList();
        }

        [HttpGet("{draggableNodeId}/{containerNodeId}")]
        public void Swap(int draggableNodeId, int containerNodeId)
        {
            _treeNodeManager.ReplaceTreeNode(new ReplaceTreeNodeRequest()
            {
                ContainerTreeNodeId = containerNodeId,
                DraggableTreeNodeId = draggableNodeId                
            });

            Sleep();            
        }
    }
}
