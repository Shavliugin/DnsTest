using DnsTestTreeView.BuisnessLogic;
using DnsTestTreeView.Entities;
using DnsTestTreeView.DataBaseLogic.Interfaces;
using DnsTestTreeView.BuisnessLogic.Requests;
using System;

namespace DnsTestTreeView.DataBaseLogic
{
    public class MSSQLFiller : ITreeNodeDbFiller
    {
        private ITreeNodeManager _treeNodeManager;

        private Random _random = new Random();        
        private const string _leafs = "Why, Mr. Anderson, why? Why, why do you do it? Why, why get up? Why keep fighting? Do you believe you're fighting for something, for more than your survival? Can you tell me what it is, do you even know? Is it freedom or truth, perhaps peace — could it be for love? Illusions, Mr. Anderson, vagaries of perception. Temporary constructs of a feeble human intellect trying desperately to justify an existence that is without meaning or purpose. And all of them as artificial as the Matrix itself. Although, only a human mind could invent something as insipid as love. You must be able to see it, Mr. Anderson, you must know it by now! You can't win, it's pointless to keep fighting! Why, Mr. Anderson, why, why do you persist";
        private const string _directories = "Everything that has a beginning has an end. I see the end coming. I see the darkness spreading. I see death. And you are all that stands in his way.";
        public MSSQLFiller(ITreeNodeManager treeNodeManager)
        {
            _treeNodeManager = treeNodeManager;
        }

        public void FillDb()
        {
            var leafs = _leafs.Split(' ');            
            var directories = _directories.Split(' ');            

            foreach (var directory in directories)
            {

                var node = _treeNodeManager.CreateNode(new CreateTreeNodeRequest()
                {
                    IsDirectory = true,
                    Name = directory,                    
                    ParentId = null
                });

                for (int i = 1; i < _random.Next(5); i++)
                {
                    _treeNodeManager.CreateNode(new CreateTreeNodeRequest()
                    {
                        IsDirectory = i % 2 == 0,
                        Name = leafs[_random.Next(leafs.Length)],
                        ParentId = node.Id
                    });
                }
            }
        }        
    }
}
