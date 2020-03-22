using DnsTestTreeView.DataBaseLogic.Interfaces;
using DnsTestTreeView.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DnsTestTreeView.DataBaseLogic
{
    public class TreeNodesMSSQLProvider : ITreeNodesProvider
    {
        private readonly SqlConnection _connection;

        public TreeNodesMSSQLProvider()
        {            
            _connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=DnsTestDb");
        }

        public TreeNode GetTreeNode(int id)
        {
            var queryString = "SELECT Id, ParentId, IsDirectory, Name, OrderId FROM TreeNode WHERE Id = @Id";

            var command = new SqlCommand(queryString, _connection);

            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            var reader = command.ExecuteReader();

            TreeNode result = null;

            if (reader.Read())
            {
                result = new TreeNode
                {
                    Id = reader.GetInt32(0),
                    ParentId = reader[1] == DBNull.Value ? null : (int?)reader.GetInt32(1),
                    IsDirectory = reader.GetBoolean(2),
                    Name = reader.GetString(3),
                    OrderId = reader.GetInt32(4)
                };
            }

            _connection.Close();

            return result;
        }

        public IEnumerable<TreeNode> GetTreeNodeList(int? parentId)
        {
            var queryString = "SELECT Id, ParentId, IsDirectory, Name, OrderId FROM TreeNode WHERE ParentId " + (parentId == null ? "IS NULL" : "= @parentId") + " ORDER BY OrderId";

            var command = new SqlCommand(queryString, _connection);

            if (parentId != null)
                command.Parameters.AddWithValue("@parentId", parentId.Value);

            _connection.Open();

            var reader = command.ExecuteReader();

            List<TreeNode> result = new List<TreeNode>();

            while (reader.Read())
            {
                result.Add(new TreeNode
                {
                    Id = reader.GetInt32(0),
                    ParentId = reader[1] == DBNull.Value ? null : (int?)reader.GetInt32(1),
                    IsDirectory = reader.GetBoolean(2),
                    Name = reader.GetString(3),
                    OrderId = reader.GetInt32(4)
                });
            }

            _connection.Close();

            return result;
        }

        public int GetTreNodeChildCount(int parentId)
        {
            var queryString = "SELECT Count(*) FROM TreeNode WHERE ParentId = @ParentId";

            var command = new SqlCommand(queryString, _connection);

            command.Parameters.AddWithValue("@ParentId", parentId);

            _connection.Open();

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = reader.GetInt32(0);
            }

            _connection.Close();

            return result;
        }

        public int InsertNode(TreeNode node)
        {
            var queryString = "INSERT INTO TreeNode (ParentId, IsDirectory, Name, OrderId) VALUES (@ParentId, @IsDirectory, @Name, (SELECT COUNT(*) FROM TreeNode WHERE ParentId " + (node.ParentId != null ? "= @ParentId))" : "IS NULL))");

            var command = new SqlCommand(queryString, _connection);

            if (node.ParentId != null)
                command.Parameters.AddWithValue("@ParentId", node.ParentId);
            else
                command.Parameters.AddWithValue("@ParentId", DBNull.Value);

            command.Parameters.AddWithValue("@IsDirectory", node.IsDirectory);
            command.Parameters.AddWithValue("@Name", node.Name);            

            _connection.Open();

            command.ExecuteNonQuery();

            queryString = "SELECT max(Id) FROM TreeNode";

            command = new SqlCommand(queryString, _connection);
                                    
            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = reader.GetInt32(0);
            }

            _connection.Close();

            return result;
        }

        public void ShiftOrderId(int? parentId, int startOrderId, int step)
        {
            var queryString = "UPDATE TreeNode SET OrderId = OrderId + @Step WHERE ParentId = @ParentId AND OrderId >= @StartOrderId" ;

            var command = new SqlCommand(queryString, _connection);

            command.Parameters.AddWithValue("@StartOrderId", startOrderId);
            command.Parameters.AddWithValue("@Step", step);
            if (parentId != null)
                command.Parameters.AddWithValue("@ParentId", parentId);
            else
                command.Parameters.AddWithValue("@ParentId", DBNull.Value);

            _connection.Open();

            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateNode(TreeNode node)
        {
            var queryString = "UPDATE TreeNode SET ParentId = @ParentId, IsDirectory = @IsDirectory, Name = @Name, OrderId = @OrderId WHERE Id = @Id";

            var command = new SqlCommand(queryString, _connection);

            command.Parameters.AddWithValue("@ParentId", node.ParentId.Value);
            command.Parameters.AddWithValue("@IsDirectory", node.IsDirectory);
            command.Parameters.AddWithValue("@Name", node.Name);
            command.Parameters.AddWithValue("@OrderId", node.OrderId);
            command.Parameters.AddWithValue("Id", node.Id);

            _connection.Open();

            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
